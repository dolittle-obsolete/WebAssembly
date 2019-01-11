/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Runtime.InteropServices;
using Dolittle.Artifacts;
using Dolittle.Execution;
using Dolittle.Lifecycle;
using Dolittle.Logging;
using Dolittle.Runtime.Commands;
using Dolittle.Runtime.Commands.Coordination;
using Dolittle.Serialization.Json;
using Dolittle.Strings;
using Dolittle.Tenancy;

namespace Dolittle.Interaction.WebAssembly.Commands
{
    /// <summary>
    /// Represents a CommandCoordinator geared towards interacting with JavaScript client code in WebAssembly scenarios
    /// </summary>
    [Singleton]
    public class CommandCoordinator
    {
        readonly ISerializer _serializer;
        readonly ICommandCoordinator _commandCoordinator;
        readonly IExecutionContextManager _executionContextManager;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="CommandCoordinator"/>
        /// </summary>
        /// <param name="serializer"><see cref="ISerializer"/> for JSON serialization</param>
        /// <param name="commandCoordinator">The actual runtime <see cref="ICommandCoordinator"/></param>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with execution contexts</param>
        /// <param name="logger"><see cref="ILogger"/> used for logging</param>
        public CommandCoordinator(
            ISerializer serializer,
            ICommandCoordinator commandCoordinator,
            IExecutionContextManager executionContextManager,
            ILogger logger)
        {
            _serializer = serializer;
            _commandCoordinator = commandCoordinator;
            _executionContextManager = executionContextManager;
            _logger = logger;
        }

        /// <summary>
        /// Handle a command
        /// </summary>
        /// <param name="json">JSON representation of a <see cref="CommandRequest"/></param>
        /// <returns><see cref="CommandResult"/></returns>
        public CommandResult Handle(string json)
        {
            _logger.Information($"Handle : {json}");

            var commandRequestKeyValues = _serializer.GetKeyValuesFromJson(json);
            var correlationId = Guid.Parse(commandRequestKeyValues["correlationId"].ToString());
            _executionContextManager.CurrentFor(TenantId.Development, correlationId);

            var content = _serializer.GetKeyValuesFromJson(commandRequestKeyValues["content"].ToString());

            var commandRequest = new CommandRequest(
                Guid.Parse(commandRequestKeyValues["correlationId"].ToString()),
                Guid.Parse(commandRequestKeyValues["type"].ToString()),
                ArtifactGeneration.First,
                content.ToDictionary(keyValue => keyValue.Key.ToPascalCase(), keyValue => keyValue.Value)
            );

            var result = _commandCoordinator.Handle(commandRequest);
            return result;
        }
    }  
}
