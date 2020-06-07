using Apicalypse.DotNet.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Apicalypse.DotNet.Interpreters
{
    /// <summary>
    /// Static class holding the method to interpret a list of fields and add
    /// to each of them a "desc" sort modificator
    /// </summary>
    public static class InvertedOrderByInterpreter
    {
        /// <summary>
        /// Interprets a list of fields and add to each of them a "desc" sort modificator 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string Run(Expression predicate, RequestBuilderConfiguration configuration)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return string.Join(",", MemberPredicateInterpreter.Run(predicate, configuration)
                .Split(',').Select(s => s + " desc"));
        }
    }
}
