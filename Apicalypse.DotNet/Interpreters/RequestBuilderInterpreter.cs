﻿using Apicalypse.DotNet.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Apicalypse.DotNet.Interpreters
{
    public static class RequestBuilderInterpreter
    {
        /// <summary>
        /// Assembles a request from the statements
        /// </summary>
        /// <param name="selects"></param>
        /// <param name="filters"></param>
        /// <param name="excludes"></param>
        /// <param name="orders"></param>
        /// <param name="search"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string Run(
            string selects,
            string filters,
            string excludes,
            string orders,
            string search,
            int take,
            int skip,
            RequestBuilderConfiguration configuration
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
                body += $"\nsearch {search};";

            if (take != default)
                body += $"\nlimit {take.ToString(CultureInfo.InvariantCulture)};";

            if (skip != default)
                body += $"\noffset {skip.ToString(CultureInfo.InvariantCulture)};";

            return body;
        }
    }
}
