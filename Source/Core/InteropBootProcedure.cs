/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Booting;
using Dolittle.DependencyInversion;
using Dolittle.Logging;

namespace Dolittle.Interaction.WebAssembly
{
    /// <summary>
    /// 
    /// </summary>
    public class InteropBootProcedure : ICanPerformBootProcedure
    {
        readonly IContainer _container;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="InteropBootProcedure"/>
        /// </summary>
        /// <param name="container"><see cref="IContainer"/> to use to setup instances</param>
        /// <param name="logger"><see cref="ILogger"/> for logging</param>
        public InteropBootProcedure(IContainer container, ILogger logger)
        {
            _container = container;
            _logger = logger;
        }

        /// <inheritdoc/>
        public bool CanPerform() => true;

        /// <inheritdoc/>
        public void Perform()
        {
            _logger.Trace("Initializing WebAssembly Client Interop");
            Interop.Initialize(_container);
        }
    }
}