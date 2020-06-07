using Apicalypse.DotNet.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Apicalypse.DotNet.Interpreters
{
    public static class InvertedOrderByInterpreter
    {
        public static string Run(Expression predicate, RequestBuilderConfiguration configuration)
        {
            return string.Join(",", MemberPredicateInterpreter.Run(predicate, configuration)
                .Split(',').Select(s => s + " desc"));
        }
    }
}
