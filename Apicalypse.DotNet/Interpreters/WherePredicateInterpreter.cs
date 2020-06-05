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
        public enum ArrayPostfixMode { ContainsAny, ExactMatch, ContainsAll};
        /// <summary>
        /// Interpretes a predicate to convert it to a string usable by IGDB API
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static string Run(Expression predicate, bool invert = false, ArrayPostfixMode arrayPostfixMode = ArrayPostfixMode.ContainsAny)
        {
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
                    return ComputeBinaryOperator(binaryPredicate.Left, binaryPredicate.Right, "&");
                case ExpressionType.OrElse:
                    return ComputeBinaryOperator(binaryPredicate.Left, binaryPredicate.Right, "|");
                case ExpressionType.GreaterThan:
                    return ComputeBinaryOperator(binaryPredicate.Left, binaryPredicate.Right, ">");
                case ExpressionType.GreaterThanOrEqual:
                    return ComputeBinaryOperator(binaryPredicate.Left, binaryPredicate.Right, ">=");
                case ExpressionType.LessThan:
                    return ComputeBinaryOperator(binaryPredicate.Left, binaryPredicate.Right, "<");
                case ExpressionType.LessThanOrEqual:
                    return ComputeBinaryOperator(binaryPredicate.Left, binaryPredicate.Right, "<=");
                case ExpressionType.NotEqual:
                    return ComputeBinaryOperator(binaryPredicate.Left, binaryPredicate.Right, "!=");
                case ExpressionType.Equal:
                    return ComputeBinaryOperator(binaryPredicate.Left, binaryPredicate.Right, "=");
                /*
                 * Second part of switch : members
                 * -------
                 * If node type refers to a member, a constant or an array then
                 * then return an interpretation.
                 */
                case ExpressionType.MemberAccess:
                    return ComputeMemberAccess(predicate);
                case ExpressionType.Constant:
                    return ComputeConstant(predicate);
                case ExpressionType.NewArrayInit:
                    return ComputeArray(predicate, arrayPostfixMode);
                /*
                 * Third part of switch : methods
                 * -------
                 * if node type referes to a method corresponding to 
                 */
                case ExpressionType.Not:
                    return ComputeNotCall(predicate);
                case ExpressionType.Call:
                    return ComputeMethodCall(predicate, invert);
                default:
                    throw new Exception($"Can not compute that expression : {predicate.NodeType}");
            }
        }

        private static string ComputeMemberAccess(Expression predicate)
        {
            return MemberPredicateInterpreter.Run(predicate);
        }

        private static string ComputeBinaryOperator(Expression left, Expression right, string binaryOperator)
        {
            var leftMember = Run(left);
            var rightMember = Run(right);

            return $"{leftMember} {binaryOperator} {rightMember}";
        }

        private static string ComputeConstant(Expression constant)
        {
            var value = (constant as ConstantExpression).Value;
            if (value is string)
                return $"\"{(value as string).Replace("\"", "\\\"")}\"";
            if (value is IConvertible)
                return (value as IConvertible).ToString(CultureInfo.InvariantCulture);
            return value.ToString();
        }

        private static string ComputeArray(Expression array, ArrayPostfixMode arrayPostfixMode)
        {
            var list = string.Join(
                ",",
                (array as NewArrayExpression).Expressions.Select(e => Run(e as ConstantExpression))
            );
            switch(arrayPostfixMode)
            {
                case ArrayPostfixMode.ContainsAny:
                    return $"({list})";
                case ArrayPostfixMode.ContainsAll:
                    return $"[{list}]";
                case ArrayPostfixMode.ExactMatch:
                    return $"{{{list}}}";
            }

            throw new Exception("Invalid array postfix mode");
        }

        private static string ComputeNotCall(Expression notCall)
        {
            return Run((notCall as UnaryExpression).Operand, true);
        }

        private static string ComputeMethodCall(Expression methodCall, bool invert = false)
        {
            var method = (methodCall as MethodCallExpression);

            if (method.Object != null && method.Object.NodeType == ExpressionType.MemberAccess)
                return ComputeStringComparison(method);
            if (
                method.Object is null
                && method.Arguments.Count() >= 1
                && method.Arguments[0].NodeType == ExpressionType.NewArrayInit
                ||
                method.Object != null
                && method.Object.NodeType == ExpressionType.NewArrayInit
            )
                return ComputeArrayComparison(method, invert);

            throw new Exception("Method call not implemented");
        }

        private static string ComputeArrayComparison(MethodCallExpression method, bool inverted)
        {
            var invert = inverted ? "!" : "";
            string left;
            string right;
            switch (method.Method.Name)
            {
                case nameof(Enumerable.Contains):
                    left = Run(method.Arguments[1]);
                    right = Run(method.Arguments[0], arrayPostfixMode: ArrayPostfixMode.ContainsAny);
                    break;
                case nameof(IEnumerableExtensions.IsContainedIn):
                    left = Run(method.Arguments[1]);
                    right = Run(method.Arguments[0], arrayPostfixMode: ArrayPostfixMode.ContainsAll);
                    break;
                case nameof(Enumerable.Equals):
                    left = Run(method.Arguments[0]);
                    right = Run(method.Object, arrayPostfixMode: ArrayPostfixMode.ExactMatch);
                    break;
                default:
                    throw new NotImplementedException($"Array method {method.Method.Name} is not implemented");
            }

            return $"{left} = {invert}{right}";
        }

        private static string ComputeStringComparison(MethodCallExpression method)
        {

            switch (method.Method.Name)
            {
                case nameof(string.Contains):
                    return MakeStringComparisonString(method, true, true, DoesMethodIgnoreCase(method));
                case nameof(string.StartsWith):
                    return MakeStringComparisonString(method, false, true, DoesMethodIgnoreCase(method));
                case nameof(string.EndsWith):
                    return MakeStringComparisonString(method, true, false, DoesMethodIgnoreCase(method));
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

        private static string MakeStringComparisonString(MethodCallExpression method, bool startsAny, bool endsAny, bool ignoreCase)
        {
            var comparison = ignoreCase ? "~" : "=";
            var startingStar = startsAny ? "*" : "";
            var endingStar = endsAny ? "*" : "";
            return $"{Run(method.Object)} {comparison} {startingStar}{Run(method.Arguments.First())}{endingStar}";
        }
    }
}
