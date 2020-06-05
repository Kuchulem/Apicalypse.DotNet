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
        public static string Run(Expression predicate)
        {
            switch(predicate.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return ComputeMemberAccess(predicate);
                case ExpressionType.New:
                    return ComputeNewObject(predicate);
                default:
                    throw new Exception("Invalid predicate provided");
            }
        }

        private static string ComputeNewObject(Expression predicate)
        {
            var newPredicate = (predicate as NewExpression);

            var properties = newPredicate.Arguments.Select(a => ComputeMemberAccess(a));

            return string.Join(",", properties);
        }

        private static string ComputeMemberAccess(Expression predicate)
        {
            var memberExpression = (predicate as MemberExpression);
            switch (memberExpression.Member.MemberType)
            {
                case System.Reflection.MemberTypes.Property:
                    var path = UnrollMemberPath(memberExpression);
                    return path;
                default:
                    throw new NotImplementedException($"Works only with properties of the Generic object");
            }
        }

        private static string UnrollMemberPath(MemberExpression predicate)
        {
            var path = "";
            if (predicate.Expression != null && predicate.Expression is MemberExpression)
                path = UnrollMemberPath(predicate.Expression as MemberExpression) + ".";

            return path + predicate.Member.Name.ToUnderscoreCase();
        }
    }
}
