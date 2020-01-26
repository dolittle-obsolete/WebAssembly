// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.ReadModels.MongoDB.WebAssembly
{
    /// <summary>
    /// Represents the configuration for MongoDB running in the browser using MiniMongo.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Gets or sets the Database name.
        /// </summary>
        public string Database { get; set; }
    }
}