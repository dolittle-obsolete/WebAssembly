/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Dolittle.ReadModels.MongoDB.WebAssembly
{
    /// <summary>
    /// Represents the configuration for MongoDB running in the browser using MiniMongo
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Gets or set the Database name
        /// </summary>
        public string Database { get; set; }
    }
}