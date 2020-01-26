// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Execution;
using Dolittle.Lifecycle;
using Dolittle.Logging;
using Dolittle.Runtime.Commands;
using Dolittle.Runtime.Commands.Coordination;
using Dolittle.Serialization.Json;

namespace Dolittle.Interaction.WebAssembly.Commands
{
    /// <summary>
    /// Represents a CommandCoordinator geared towards interacting with JavaScript client code in WebAssembly scenarios.
    /// </summary>
    [Singleton]
    public class CommandCoordinator
    {
        readonly ISerializer _serializer;
        readonly ICommandCoordinator _commandCoordinator;
        readonly IExecutionContextManager _executionContextManager;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandCoordinator"/> class.
        /// </summary>
        /// <param name="serializer"><see cref="ISerializer"/> for JSON serialization.</param>
        /// <param name="commandCoordinator">The actual runtime <see cref="ICommandCoordinator"/>.</param>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with execution contexts.</param>
        /// <param name="logger"><see cref="ILogger"/> used for logging.</param>
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
        /// Handle a command.
        /// </summary>
        /// <param name="commandRequest">The <see cref="CommandRequest"/>.</param>
        /// <returns><see cref="CommandResult"/>.</returns>
        public CommandResult Handle(CommandRequestWithDefaultGeneration commandRequest)
        {
            return _commandCoordinator.Handle(commandRequest);
        }
    }
}
