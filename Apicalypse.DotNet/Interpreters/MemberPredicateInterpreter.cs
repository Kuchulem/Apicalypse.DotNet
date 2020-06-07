using Apicalypse.DotNet.Configuration;
using Apicalypse.DotNet.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Apicalypse.DotNet.Interpreters
{
    public class MemberPredicateInterpreter
    {
        /// <summary>
        /// Returns a member or list of members as a string from a predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string Run(Expression predicate, RequestBuilderConfiguration configuration)
        {
            switch(predicate.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return ComputeMemberAccess(predicate, configuration);
                case ExpressionType.New:
                    return ComputeNewObject(predicate, configuration);
                default:
                    throw new Exception("Invalid predicate provided");
            }
        }

        private static string ComputeNewObject(Expression predicate, RequestBuilderConfiguration configuration)
        {
            var newPredicate = (predicate as NewExpression);

            var properties = newPredicate.Arguments.Select(a => ComputeMemberAccess(a, configuration));

            return string.Join(",", properties);
        }

        private static string ComputeMemberAccess(Expression predicate, RequestBuilderConfiguration configuration)
        {
            var memberExpression = (predicate as MemberExpression);
            switch (memberExpression.Member.MemberType)
            {
                case System.Reflection.MemberTypes.Property:
                    var path = UnrollMemberPath(memberExpression, configuration);
                    return path;
                default:
                    throw new NotImplementedException($"Works only with properties of the Generic object");
            }
        }

        private static string UnrollMemberPath(MemberExpression predicate, RequestBuilderConfiguration configuration)
        {
            var path = "";
            if (predicate.Expression != null && predicate.Expression is MemberExpression)
                path = UnrollMemberPath(predicate.Expression as MemberExpression, configuration) + ".";

            return path + FieldInterpreter.Run(predicate.Member.Name, configuration);
        }
    }
}
