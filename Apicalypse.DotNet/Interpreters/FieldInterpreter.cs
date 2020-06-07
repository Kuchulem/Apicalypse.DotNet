using Apicalypse.DotNet.Configuration;
using Apicalypse.DotNet.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apicalypse.DotNet.Interpreters
{
    /// <summary>
    /// Static class holding the method to convert fields case depending on configuration provided
    /// </summary>
    public static class FieldInterpreter
    {
        /// <summary>
        /// Converts fields case depending on configuration provided
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string Run(string fieldName, RequestBuilderConfiguration configuration)
        {
            return configuration.CaseContract switch
            {
                CaseContract.SnakeCase => fieldName.ToSnakeCase(),
                CaseContract.PascalCase => fieldName.ToPascalCase(),
                CaseContract.CamelCase => fieldName.ToCamelCase(),
                _ => fieldName
            };
        }
    }
}
