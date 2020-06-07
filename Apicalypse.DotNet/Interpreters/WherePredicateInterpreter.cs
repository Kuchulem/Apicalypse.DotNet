using Apicalypse.DotNet.Configuration;
using Apicalypse.DotNet.Exceptions;
using Apicalypse.DotNet.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Apicalypse.DotNet.Interpreters
{
    /// <summary>
    /// Interpreter for predicates in Where method of the request builder
    /// </summary>
    public class WherePredicateInterpreter
    {
        public enum ArrayPostfixMode { ContainsAny, ExactMatch, ContainsAll };
        /// <summary>
        /// Interpretes a predicate to convert it to a string usable by IGDB API
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static string Run(Expression predicate, RequestBuilderConfiguration configuration, bool invert = false, ArrayPostfixMode arrayPostfixMode = ArrayPostfixMode.ContainsAny)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            var binaryPredicate = predicate as BinaryExpression;
            switch (predicate.NodeType)
            {
                /*
                 * First part of switch : binary operators
                 * -------
                 * if Node type referes to a binary operators we set the operator
                 * and let the method continue after the switch to create the
                 * expression.
                 * binary operators are either logical operators (&& or ||),
                 * relational operators (>, >=, <, <=) or equality operators (==, !=)
                 */
                case ExpressionType.AndAlso:
                    return ComputeBinaryOperator(binaryPredicate.Left, binaryPredicate.Right, "&", configuration);
                case ExpressionType.OrElse:
                    return ComputeBinaryOperator(binaryPredicate.Left, binaryPredicate.Right, "|", configuration);
                case ExpressionType.GreaterThan:
                    return ComputeBinaryOperator(binaryPredicate.Left, binaryPredicate.Right, ">", configuration);
                case ExpressionType.GreaterThanOrEqual:
                    return ComputeBinaryOperator(binaryPredicate.Left, binaryPredicate.Right, ">=", configuration);
                case ExpressionType.LessThan:
                    return ComputeBinaryOperator(binaryPredicate.Left, binaryPredicate.Right, "<", configuration);
                case ExpressionType.LessThanOrEqual:
                    return ComputeBinaryOperator(binaryPredicate.Left, binaryPredicate.Right, "<=", configuration);
                case ExpressionType.NotEqual:
                    return ComputeBinaryOperator(binaryPredicate.Left, binaryPredicate.Right, "!=", configuration);
                case ExpressionType.Equal:
                    return ComputeBinaryOperator(binaryPredicate.Left, binaryPredicate.Right, "=", configuration);
                /*
                 * Second part of switch : members
                 * -------
                 * If node type refers to a member, a constant or an array then
                 * then return an interpretation.
                 */
                case ExpressionType.MemberAccess:
                    return ComputeMemberAccess(predicate, configuration);
                case ExpressionType.Constant:
                    return ComputeConstant(predicate, configuration);
                case ExpressionType.NewArrayInit:
                    return ComputeArray(predicate, arrayPostfixMode, configuration);
                /*
                 * Third part of switch : methods
                 * -------
                 * if node type referes to a method corresponding to 
                 */
                case ExpressionType.Not:
                    return ComputeNotCall(predicate, configuration);
                case ExpressionType.Call:
                    return ComputeMethodCall(predicate, configuration, invert);
                default:
                    throw new InvalidPredicateException(predicate);
            }
        }

        private static string ComputeMemberAccess(Expression predicate, RequestBuilderConfiguration configuration)
        {
            return MemberPredicateInterpreter.Run(predicate, configuration);
        }

        private static string ComputeBinaryOperator(Expression left, Expression right, string binaryOperator, RequestBuilderConfiguration configuration)
        {
            var leftMember = Run(left, configuration);
            var rightMember = Run(right, configuration);

            return $"{leftMember} {binaryOperator} {rightMember}";
        }

        private static string ComputeConstant(Expression constant, RequestBuilderConfiguration configuration)
        {
            var value = (constant as ConstantExpression).Value;
            if (value is string)
                return $"\"{(value as string).Replace("\"", "\\\"")}\"";
            if (value is bool boolean)
                return boolean ? "true" : "false";
            if (value is null)
                return "null";
            if (value is IConvertible)
                return (value as IConvertible).ToString(CultureInfo.InvariantCulture);

            return value.ToString();
        }

        private static string ComputeArray(Expression array, ArrayPostfixMode arrayPostfixMode, RequestBuilderConfiguration configuration)
        {
            var list = string.Join(
                ",",
                (array as NewArrayExpression).Expressions.Select(e => Run(e as ConstantExpression, configuration))
            );

            return arrayPostfixMode switch
            {
                ArrayPostfixMode.ContainsAny => $"({list})",
                ArrayPostfixMode.ContainsAll => $"[{list}]",
                ArrayPostfixMode.ExactMatch => $"{{{list}}}",
                _ => throw new Exception("Unknown array postfix mode"),
            };
        }

        private static string ComputeNotCall(Expression notCall, RequestBuilderConfiguration configuration)
        {
            return Run((notCall as UnaryExpression).Operand, configuration, true);
        }

        private static string ComputeMethodCall(Expression methodCall, RequestBuilderConfiguration configuration, bool invert = false)
        {
            var method = (methodCall as MethodCallExpression);

            if (method.Object != null && method.Object.NodeType == ExpressionType.MemberAccess)
                return ComputeStringComparison(method, configuration);
            if (
                method.Object is null
                && method.Arguments.Count() >= 1
                && method.Arguments[0].NodeType == ExpressionType.NewArrayInit
                ||
                method.Object != null
                && method.Object.NodeType == ExpressionType.NewArrayInit
            )
                return ComputeArrayComparison(method, configuration, invert);

            throw new InvalidPredicateException(methodCall);
        }

        private static string ComputeArrayComparison(MethodCallExpression method, RequestBuilderConfiguration configuration, bool inverted)
        {
            var invert = inverted ? "!" : "";
            string left;
            string right;
            switch (method.Method.Name)
            {
                case nameof(Enumerable.Contains):
                    left = Run(method.Arguments[1], configuration);
                    right = Run(method.Arguments[0], configuration, arrayPostfixMode: ArrayPostfixMode.ContainsAny);
                    break;
                case nameof(IEnumerableExtensions.IsContainedIn):
                    left = Run(method.Arguments[1], configuration);
                    right = Run(method.Arguments[0], configuration, arrayPostfixMode: ArrayPostfixMode.ContainsAll);
                    break;
                case nameof(Enumerable.Equals):
                    left = Run(method.Arguments[0], configuration);
                    right = Run(method.Object, configuration, arrayPostfixMode: ArrayPostfixMode.ExactMatch);
                    break;
                default:
                    throw new NotImplementedException($"Array method {method.Method.Name} is not implemented");
            }

            return $"{left} = {invert}{right}";
        }

        private static string ComputeStringComparison(MethodCallExpression method, RequestBuilderConfiguration configuration)
        {

            switch (method.Method.Name)
            {
                case nameof(string.Contains):
                    return MakeStringComparisonString(method, configuration, true, true, DoesMethodIgnoreCase(method));
                case nameof(string.StartsWith):
                    return MakeStringComparisonString(method, configuration, false, true, DoesMethodIgnoreCase(method));
                case nameof(string.EndsWith):
                    return MakeStringComparisonString(method, configuration, true, false, DoesMethodIgnoreCase(method));
                default:
                    throw new NotImplementedException($"The string comparison method {method.Method.Name} is not implemented");
            }
        }

        private static bool DoesMethodIgnoreCase(MethodCallExpression method)
        {
            if (method.Arguments.Count > 1
                && (method.Arguments[1] as ConstantExpression).Value is StringComparison)
                return StringComparisonArgIgnoreCase(method);
            if (method.Arguments.Count > 2
                && (method.Arguments[1] as ConstantExpression).Value is bool)
                return BoolAndCultureArgsIgnoreCase(method);

            return false;
        }

        private static bool StringComparisonArgIgnoreCase(MethodCallExpression method)
        {
            StringComparison[] flags = new StringComparison[]
            {
                StringComparison.OrdinalIgnoreCase, StringComparison.CurrentCultureIgnoreCase, StringComparison.InvariantCultureIgnoreCase
            };
            return (
                method.Arguments.Count() > 1
                && flags.Contains((StringComparison)(method.Arguments[1] as ConstantExpression).Value));

        }

        private static bool BoolAndCultureArgsIgnoreCase(MethodCallExpression method)
        {
            return (
                method.Arguments.Count() >= 2
                && ((bool)(method.Arguments[1] as ConstantExpression).Value) == true
            );
        }

        private static string MakeStringComparisonString(MethodCallExpression method, RequestBuilderConfiguration configuration, bool startsAny, bool endsAny, bool ignoreCase)
        {
            var comparison = ignoreCase ? "~" : "=";
            var startingStar = startsAny ? "*" : "";
            var endingStar = endsAny ? "*" : "";
            return $"{Run(method.Object, configuration)} {comparison} {startingStar}{Run(method.Arguments.First(), configuration)}{endingStar}";
        }
    }
}
