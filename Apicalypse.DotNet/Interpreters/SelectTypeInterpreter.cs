using Apicalypse.DotNet.Attributes;
using Apicalypse.DotNet.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            var fields = new List<string>();

            foreach(var p in properties)
            {
                fields.Add(GetFields(p));
            }

            return string.Join(",",fields.Where(f => !string.IsNullOrEmpty(f)));
        }

        private static string GetFields(PropertyInfo property, string parentPath = "")
        {
            var fields = new List<string>();
            var path = parentPath + property.Name.ToUnderscoreCase();
            if (property.GetCustomAttribute<IncludeAttribute>() != null)
            {
                foreach(var p in property.PropertyType.GetProperties())
                {
                    fields.Add(GetFields(p, path + "."));
                }
            }
            else if(property.GetCustomAttribute<ExcludeAttribute>() == null)
            {
                fields.Add(path);
            }

            return string.Join(",", fields);
        }
    }
}
