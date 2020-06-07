using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Apicalypse.DotNet.Exceptions
{
    public class InvalidPredicateException : Exception
    {
        private const string MESSAGE = "Could not parse predicate :";

        /// <summary>
        /// The string representation of the incriminated predicate.
        /// </summary>
        public string Body { get; private set; }

        /// <summary>
        /// Exception thrown when a predicate could not be parsed.<br/>
        /// Check the Body property to see what predicate is incriminated.
        /// </summary>
        /// <param name="predicate"></param>
        public InvalidPredicateException(Expression predicate)
            : base($"{MESSAGE} {predicate.NodeType}")
        {
            Body = predicate.ToString();
        }
    }
}
