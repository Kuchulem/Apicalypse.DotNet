using Apicalypse.DotNet.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apicalypse.DotNet.Interpreters
{
    /// <summary>
    /// Interpreter for generic types in Select method of the request builder
    /// </summary>
    public static class SelectTypeInterpreter
    {
        /// <summary>
        /// Interprets the public properties of the generic type to generate a list of selects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string Run<T>()
        {
            var properties = typeof(T)
                .GetProperties();

            return string.Join(
                ",", 
                properties
                    .Select(p => p.Name.ToUnderscoreCase())
                    .ToList());
        }
    }
}
