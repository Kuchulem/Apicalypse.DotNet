using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Apicalypse.DotNet.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string to snake case : FooBarBaz => foo_bar_baz
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSnakeCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }

        /// <summary>
        /// Converts a string to pascal case : foo_bar_baz => FooBarBaz
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            StringBuilder resultBuilder = new StringBuilder();

            foreach (char c in str)
            {
                // Replace anything, but letters and digits, with space
                if (!char.IsLetterOrDigit(c))
                {
                    resultBuilder.Append(" ");
                }
                else if(char.IsUpper(c))
                {
                    resultBuilder.Append(" ");
                    resultBuilder.Append(c);
                }
                else
                {
                    resultBuilder.Append(c);
                }
            }

            string result = resultBuilder.ToString();

            // Make result string all lowercase, because ToTitleCase does not change all uppercase correctly
            result = result.ToLower();

            // Creates a TextInfo based on the "en-US" culture.
            TextInfo myTI = CultureInfo.InvariantCulture.TextInfo;

            result = myTI.ToTitleCase(result).Replace(" ", string.Empty);

            return result;
        }

        /// <summary>
        /// Converts a string to camel case : foo_bar_baz => fooBarBaz
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            var pascalCase = str.ToPascalCase();

            char[] a = pascalCase.ToCharArray();
            a[0] = char.ToLower(a[0]);

            return new string(a);
        }
    }
}
