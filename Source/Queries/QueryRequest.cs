// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Dolittle.Interaction.WebAssembly.Queries
{
    /// <summary>
    /// Represents the request for a query.
    /// </summary>
    public class QueryRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryRequest"/> class.
        /// </summary>
        public QueryRequest()
        {
            Parameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets the name of the query.
        /// </summary>
        public string NameOfQuery { get; set; }

        /// <summary>
        /// Gets or sets where it was generated from.
        /// </summary>
        public string GeneratedFrom { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        public IDictionary<string, object> Parameters { get; set; }
    }
}
