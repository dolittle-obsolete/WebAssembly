/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace Dolittle.Interaction.WebAssembly.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public QueryRequest()
        {
            Parameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// 
        /// </summary>
        public string NameOfQuery { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GeneratedFrom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> Parameters { get; set; }
    }
}
