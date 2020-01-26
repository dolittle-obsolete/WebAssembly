// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Booting;
using Dolittle.DependencyInversion;
using Dolittle.Execution;
using Dolittle.Interaction.WebAssembly.Interop;
using Dolittle.ResourceTypes.Configuration;
using Dolittle.Tenancy;

namespace Dolittle.ReadModels.MongoDB.WebAssembly
{
    /// <summary>
    /// Represents an implementation of a <see cref="ICanPerformBootProcedure">boot procedure</see>.
    /// </summary>
    public class BootProcedure : ICanPerformBootProcedure
    {
        readonly IJSRuntime _jsRuntime;
        readonly IConfigurationFor<Configuration> _configurationFor;

        /// <summary>
        /// Initializes a new instance of the <see cref="BootProcedure"/> class.
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/>.</param>
        /// <param name="executionContextManager">The <see cref="IExecutionContextManager"/> for managing <see cref="ExecutionContext"/>.</param>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        public BootProcedure(
            IContainer container,
            IExecutionContextManager executionContextManager,
            IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;

            executionContextManager.CurrentFor(TenantId.Development);
            _configurationFor = container.Get<IConfigurationFor<Configuration>>();
        }

        /// <inheritdoc/>
        public bool CanPerform() => true;

        /// <inheritdoc/>
        public void Perform()
        {
            _jsRuntime.Invoke($"{MiniMongoDatabase._globalObject}.initialize", _configurationFor.Instance.Database);
        }
    }
}