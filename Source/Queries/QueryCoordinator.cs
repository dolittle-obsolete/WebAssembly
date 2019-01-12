/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Dolittle.Artifacts;
using Dolittle.Concepts;
using Dolittle.DependencyInversion;
using Dolittle.Dynamic;
using Dolittle.Execution;
using Dolittle.Lifecycle;
using Dolittle.Logging;
using Dolittle.Queries;
using Dolittle.Queries.Coordination;
using Dolittle.Runtime.Commands;
using Dolittle.Runtime.Commands.Coordination;
using Dolittle.Security;
using Dolittle.Serialization.Json;
using Dolittle.Strings;
using Dolittle.Tenancy;
using Dolittle.Types;

namespace Dolittle.Interaction.WebAssembly.Queries
{
    /// <summary>
    /// Represents a CommandCoordinator geared towards interacting with JavaScript client code in WebAssembly scenarios
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
        /// Initializes a new instance of <see cref="QueryCoordinator"/>
        /// </summary>
        /// <param name="typeFinder"></param>
        /// <param name="container"></param>
        /// <param name="queryCoordinator">The underlying <see cref="IQueryCoordinator"/> </param>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with execution contexts</param>
        /// <param name="queries"></param>
        /// <param name="serializer"></param>
        /// <param name="logger"></param>
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
        /// 
        /// </summary>
        /// <param name="queryRequest"></param>
        /// <returns></returns>
        public Task<QueryResult> Execute(QueryRequest queryRequest)
        {
            var taskCompletionSource = new TaskCompletionSource<QueryResult>();
          
            try
            {
                _executionContextManager.CurrentFor(TenantId.Development, Dolittle.Execution.CorrelationId.New(), ClaimsPrincipal.Current.ToClaims());
                _logger.Information($"Executing query : {queryRequest.NameOfQuery}");
                var queryType = _typeFinder.GetQueryTypeByName(queryRequest.GeneratedFrom);
                var query = _container.Get(queryType) as IQuery;

                PopulateProperties(queryRequest, queryType, query);

                _logger.Trace($"Executing runtime query coordinator");
                _queryCoordinator.Execute(query, new PagingInfo()).ContinueWith(t => {
                    _logger.Trace("Result");
                    if (t.Result.Success) AddClientTypeInformation(t.Result);
                    _logger.Trace("Client information added - done");
                    taskCompletionSource.SetResult(t.Result);
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error executing query : '{queryRequest.NameOfQuery}'");
                taskCompletionSource.SetResult(new QueryResult
                {
                    Exception = ex,
                    QueryName = queryRequest.NameOfQuery
                });
            }

            return taskCompletionSource.Task;
        }

        void AddClientTypeInformation(QueryResult result)
        {
            var items = new List<object>();
            foreach (var item in result.Items)
            {
                var dynamicItem = item.AsExpandoObject();
                var type = item.GetType();
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
                        .Name.Equals(key, StringComparison.InvariantCultureIgnoreCase)
                    );
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
                            if (valueType == typeof(Guid)) underlyingValue = Guid.Parse(propertyValue);
                            else underlyingValue = Convert.ChangeType(propertyValue, valueType);
                            value = ConceptFactory.CreateConceptInstance(property.PropertyType, underlyingValue);
                        }
                        catch { }
                    }
                    else
                    {
                        if (property.PropertyType == typeof(Guid)) value = Guid.Parse(propertyValue);
                        else value = Convert.ChangeType(propertyValue, property.PropertyType);
                    }

                    property.SetValue(instance, value, null);
                }
            }
        }

    }
}