using Apicalypse.DotNet.Configuration;
using Apicalypse.DotNet.Interpreters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Apicalypse.DotNet
{
    /// <summary>
    /// Entry point for the lib.<br/>The request builder provides methods to prepare the
    /// query statements to send to the API.
    /// </summary>
    /// <typeparam name="T">The API model on witch the query is based</typeparam>
    public class RequestBuilder<T>
    {
        private readonly RequestBuilderConfiguration configuration;
        private string selects;
        private string filters;
        private string excludes;
        private string orders;
        private string search;
        private int take;
        private int skip;

        public RequestBuilder()
            : this(new RequestBuilderConfiguration { CaseContract = CaseContract.SnakeCase })
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RequestBuilder(RequestBuilderConfiguration configuration)
        {
            selects = "*";
            orders = "";
            this.configuration = configuration;
        }


        /// <summary>
        /// Sets the list of fields to gather from the API model with the public
        /// properties of the <em>TSelect</em> generic parameter.<br/>
        /// Each call replace the previous<br/>
        /// Prepares the <strong>fields</strong> statement of the Apicalypse query.
        /// </summary>
        /// <typeparam name="TSelect"></typeparam>
        /// <returns>The request builder, to chain the statements</returns>
        public RequestBuilder<T> Select<TSelect>()
        {
            if (!string.IsNullOrEmpty(excludes))
                throw new InvalidOperationException("Can't combine Exclude and Select methods.");

            selects = SelectTypeInterpreter.Run<TSelect>(configuration);

            return this;
        }

        /// <summary>
        /// Sets the list of fields to gather from the API model with predicate passed
        /// as parameter.<br/>
        /// Each call replace the previous<br/>
        /// Prepares the <strong>fields</strong> statement of the Apicalypse query.
        /// </summary>
        /// <param name="predicate">A predicate expression that provides a list of fields or a single fields</param>
        /// <returns>The request builder, to chain the statements</returns>
        public RequestBuilder<T> Select(Expression<Func<T, object>> predicate)
        {
            if(!string.IsNullOrEmpty(excludes))
                throw new InvalidOperationException("Can't combine Exclude and Select methods.");

            selects = MemberPredicateInterpreter.Run(predicate.Body, configuration);

            return this;
        }

        /// <summary>
        /// Sets the list of fields to exclude from the API model using the predicate used
        /// in parameters.<br/>
        /// Each call replace the previous.<br/>
        /// Prepares the <strong>excludes</strong> statement of the Apicalypse query.<br/>
        /// Can't be combine with the <em>Select(Expression&lt;Func&lt;T, object&gt;&gt;)</em> or the
        /// <em>Select&lt;TSelect&gt;()</em> methods.
        /// </summary>
        /// <param name="predicate">A predicate expression that provides a list of fields or a single field</param>
        /// <returns>The request builder, to chain the statements</returns>
        public RequestBuilder<T> Exclude(Expression<Func<T, object>> predicate)
        {
            if (selects != "*")
                throw new InvalidOperationException("Can't combine Exclude and Select methods.");

            excludes = MemberPredicateInterpreter.Run(predicate.Body, configuration);

            return this;
        }

        /// <summary>
        /// Sets the where clause to send to the API.<br/>
        /// Each call replace the previous<br/>
        /// Prepares the <strong>where</strong> statement of the Apicalypse query.
        /// </summary>
        /// <param name="predicate">A predicate expression that provides a conditional test</param>
        /// <returns>The request builder, to chain the statements</returns>
        public RequestBuilder<T> Where(Expression<Func<T, bool>> predicate)
        {
            filters = WherePredicateInterpreter.Run(predicate.Body, configuration);

            return this;
        }

        /// <summary>
        /// Adds one or multiple <em>order by ascending</em> statements to the order by clause of
        /// the API.<br/>
        /// Each call adds a statement to the orders clause<br/>
        /// Prepares the <strong>orders</strong> statement of the Apicalypse query.
        /// </summary>
        /// <param name="predicate">A predicate expression that provides a list of fields or a single fields</param>
        /// <returns>The request builder, to chain the statements</returns>
        public RequestBuilder<T> OrderBy(Expression<Func<T, object>> predicate)
        {
            if (!string.IsNullOrEmpty(orders))
                orders += ",";
            orders += MemberPredicateInterpreter.Run(predicate.Body, configuration);
            return this;
        }

        /// <summary>
        /// Adds one or multiple <em>order by descending</em> statements to the order by clause of
        /// the API.<br/>
        /// Each call adds a statement to the orders clause<br/>
        /// Prepares the <strong>orders</strong> statement of the Apicalypse query.
        /// </summary>
        /// <param name="predicate">A predicate expression that provides a list of fields or a single fields</param>
        /// <returns>The request builder, to chain the statements</returns>
        public RequestBuilder<T> OrderByDescending(Expression<Func<T, object>> predicate)
        {
            if (!string.IsNullOrEmpty(orders))
                orders += ",";
            orders += InvertedOrderByInterpreter.Run(predicate.Body, configuration);

            return this;
        }

        /// <summary>
        /// Sets a string to search in the API.<br/>
        /// Each call replace the previous<br/>
        /// Prepares the <strong>search</strong> statement of the Apicalypse query.
        /// </summary>
        /// <param name="search"></param>
        /// <returns>The request builder, to chain the statements</returns>
        public RequestBuilder<T> Search(string search)
        {
            if (string.IsNullOrEmpty(search))
                this.search = "";
            else
                this.search = PrepareSearchString(search);

            return this;
        }

        public RequestBuilder<T> Search(string search, Expression<Func<T, string>> field)
        {
            if (string.IsNullOrEmpty(search))
                this.search = "";
            else
                this.search = $"{MemberPredicateInterpreter.Run(field.Body, configuration)} {PrepareSearchString(search)}";

            return this;
        }

        /// <summary>
        /// Sets the number of items to gather from the API.<br/>
        /// Usefull to pagination.<br/>
        /// Each call replace the previous<br/>
        /// Prepares the <strong>limit</strong> statement of the Apicalypse query.
        /// </summary>
        /// <param name="count"></param>
        /// <returns>The request builder, to chain the statements</returns>
        public RequestBuilder<T> Take(int count)
        {
            take = count;

            return this;
        }

        /// <summary>
        /// Sets the number of items to skip from the API. Usefull to pagination is combined to
        /// <em>Take(int)</em><br/>
        /// Each call replace the previous<br/>
        /// Prepares the <strong>offset</strong> statement of the Apicalypse query.
        /// </summary>
        /// <param name="count"></param>
        /// <returns>The request builder, to chain the statements</returns>
        public RequestBuilder<T> Skip(int count)
        {
            skip = count;

            return this;
        }

        /// <summary>
        /// Builds the request and provides an <em>Apicalypse</em> request.
        /// </summary>
        /// <returns>An Apicalypse request that can send the query to the API</returns>
        public ApicalipseRequest Build()
        {
            return new ApicalipseRequest(
                RequestBuilderInterpreter.Run(selects, filters, excludes, orders, search, take, skip, configuration)
            );
        }

        private string PrepareSearchString(string search)
        {
            var escaped = search.Replace("\"", "\\\"");

            return $"\"{escaped}\"";
        }
    }
}
