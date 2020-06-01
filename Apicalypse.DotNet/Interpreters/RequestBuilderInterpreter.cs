using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Apicalypse.DotNet.Interpreters
{
    public static class RequestBuilderInterpreter
    {
        public static string Run(
            string selects,
            string filters,
            string excludes,
            string orders,
            string search,
            int take,
            int skip
        )
        {
            if (string.IsNullOrEmpty(selects))
                throw new ArgumentNullException(nameof(selects));

            string body = $"fields {selects};";

            if (!string.IsNullOrEmpty(filters))
                body += $"\nwhere {filters};";

            if (!string.IsNullOrEmpty(excludes))
                body += $"\nexclude {excludes};";

            if (!string.IsNullOrEmpty(orders))
                body += $"\nsort {orders};";
            
            if (!string.IsNullOrEmpty(search))
                body += $"\nsearch \"{search.Replace("\"", "\\\"")}\";";

            if (take != default)
                body += $"\nlimit {take.ToString(CultureInfo.InvariantCulture)};";

            if (skip != default)
                body += $"\noffset {skip.ToString(CultureInfo.InvariantCulture)};";

            return body;
        }
    }
}
