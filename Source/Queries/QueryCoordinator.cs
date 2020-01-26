// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dolittle.Concepts;
using Dolittle.DependencyInversion;
using Dolittle.Dynamic;
using Dolittle.Execution;
using Dolittle.Lifecycle;
using Dolittle.Logging;
using Dolittle.Queries;
using Dolittle.Queries.Coordination;
using Dolittle.Security;
using Dolittle.Serialization.Json;
using Dolittle.Tenancy;
using Dolittle.Types;

namespace Dolittle.Interaction.WebAssembly.Queries
{
    /// <summary>
    /// Represents a CommandCoordinator geared towards interacting with JavaScript client code in WebAssembly scenarios.
    /// </summary>
    [Singleton]
    public class QueryCoordinator
    {
        readonly ITypeFinder _typeFinder;
        readonly IContainer _container;
        readonly IQueryCoordinator _queryCoordinator;
        readonly IExecutionContextManager _executionContextManager;
        readonly IInstancesOf<IQuery> _queries;
        readonly ILogger _logger;
        readonly ISerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryCoordinator"/> class.
        /// </summary>
        /// <param name="typeFinder"><see cref="ITypeFinder"/> for finding types.</param>
        /// <param name="container"><see cref="IContainer"/> for getting instances of queries.</param>
        /// <param name="queryCoordinator">The underlying <see cref="IQueryCoordinator"/>.</param>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with execution contexts.</param>
        /// <param name="queries">All <see cref="IQuery"/> instances.</param>
        /// <param name="serializer">JSON <see cref="ISerializer"/>.</param>
        /// <param name="logger"><see cref="ILogger"/> for logging.</param>
        public QueryCoordinator(
            ITypeFinder typeFinder,
            IContainer container,
            IQueryCoordinator queryCoordinator,
            IExecutionContextManager executionContextManager,
            IInstancesOf<IQuery> queries,
            ISerializer serializer,
            ILogger logger)
        {
            _typeFinder = typeFinder;
            _container = container;
            _queryCoordinator = queryCoordinator;
            _queries = queries;
            _serializer = serializer;
            _executionContextManager = executionContextManager;
            _logger = logger;
        }

        /// <summary>
        /// Execute a query.
        /// </summary>
        /// <param name="queryRequest"><see cref="QueryRequest"/> to execute.</param>
        /// <returns>Task with <see cref="QueryResult"/>.</returns>
        public async Task<QueryResult> Execute(QueryRequest queryRequest)
        {
            QueryResult result;
            try
            {
                _executionContextManager.CurrentFor(TenantId.Development, Dolittle.Execution.CorrelationId.New(), ClaimsPrincipal.Current.ToClaims());

                var queryType = _typeFinder.GetQueryTypeByName(queryRequest.GeneratedFrom);
                var query = _container.Get(queryType) as IQuery;

                PopulateProperties(queryRequest, queryType, query);

                _logger.Trace($"Executing runtime query coordinator");
                result = await _queryCoordinator.Execute(query, new PagingInfo()).ConfigureAwait(false);

                if (result.Success) AddClientTypeInformation(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error executing query : '{queryRequest.NameOfQuery}'");
                result = new QueryResult
                {
                    Exception = ex,
                    QueryName = queryRequest.NameOfQuery
                };
            }

            return result;
        }

        void AddClientTypeInformation(QueryResult result)
        {
            var items = new List<object>();
            foreach (var item in result.Items)
            {
                var dynamicItem = item.AsExpandoObject();
                items.Add(dynamicItem);
            }

            result.Items = items;
        }

        void PopulateProperties(QueryRequest descriptor, Type queryType, object instance)
        {
            foreach (var key in descriptor.Parameters.Keys)
            {
                var property = queryType
                    .GetProperties()
                    .SingleOrDefault(_ => _
                        .Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));

                if (property != null)
                {
                    var propertyValue = descriptor.Parameters[key].ToString();
                    object value = null;
                    if (property.PropertyType.IsConcept())
                    {
                        var valueType = property.PropertyType.GetConceptValueType();
                        object underlyingValue = null;
                        try
                        {
                            if (valueType == typeof(Guid))
                            {
                                underlyingValue = Guid.Parse(propertyValue);
                            }
                            else
                            {
                                underlyingValue = Convert.ChangeType(propertyValue, valueType, CultureInfo.InvariantCulture);
                            }

                            value = ConceptFactory.CreateConceptInstance(property.PropertyType, underlyingValue);
                        }
                        catch { }
                    }
                    else
                    {
                        if (property.PropertyType == typeof(Guid))
                        {
                            value = Guid.Parse(propertyValue);
                        }
                        else
                        {
                            value = Convert.ChangeType(propertyValue, property.PropertyType, CultureInfo.InvariantCulture);
                        }
                    }

                    property.SetValue(instance, value, null);
                }
            }
        }
    }
}