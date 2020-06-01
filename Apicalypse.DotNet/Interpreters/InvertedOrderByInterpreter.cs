using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Apicalypse.DotNet.Interpreters
{
    public static class InvertedOrderByInterpreter
    {
        public static string Run(Expression predicate)
        {
            return string.Join(",", MemberPredicateInterpreter.Run(predicate)
                .Split(',').Select(s => s + " desc"));
        }
    }
}
