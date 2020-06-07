using Apicalypse.DotNet.Configuration;
using Apicalypse.DotNet.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apicalypse.DotNet.Interpreters
{
    public static class FieldInterpreter
    {
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
