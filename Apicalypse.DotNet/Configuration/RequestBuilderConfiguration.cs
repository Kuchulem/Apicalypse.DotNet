using System;
using System.Collections.Generic;
using System.Text;

namespace Apicalypse.DotNet.Configuration
{
    public enum CaseContract 
    { 
        // Use snake_case fields in query
        SnakeCase, 
        // Use camelCase fields in query
        CamelCase, 
        // User PascalCase fields in query
        PascalCase,
        // Don't change the case of the fields
        AsIs 
    }
    public class RequestBuilderConfiguration
    {
        public CaseContract CaseContract { get; set; } = CaseContract.SnakeCase;
    }
}
