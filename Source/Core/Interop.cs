/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Reflection;
using Dolittle.DependencyInversion;
using Dolittle.Serialization.Json;
using Dolittle.Types;

namespace Dolittle.Interaction.WebAssembly
{
    /// <summary>
    /// 
    /// </summary>
    public class Interop
    {
        static IContainer _container;
        static ISerializer _serializer;
        static ITypeFinder _typeFinder;
        

        internal static void Initialize(IContainer container)
        {
            _container = container;
            _serializer = _container.Get<ISerializer>();
            _typeFinder = _container.Get<ITypeFinder>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <param name="argumentsAsJson"></param>
        /// <returns></returns>
        public static string InvokeMethod(string typeName, string methodName, string argumentsAsJson)
        {
            Console.WriteLine($"CallMethodOnType : {typeName}, {methodName}, {argumentsAsJson}");

            var type = _typeFinder.FindTypeByFullName(typeName);
            var instance = _container.Get(type);
            var method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);

            var arguments = _serializer.FromJson<string[]>(argumentsAsJson);
            var deserializedArguments = new List<object>();
            var parameters = method.GetParameters();
            for (var parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
            {
                if (parameters[parameterIndex].ParameterType == typeof(string))
                {
                    deserializedArguments.Add(arguments[parameterIndex]);
                }
                else
                {
                    var deserializedArgument = _serializer.FromJson(parameters[parameterIndex].ParameterType, arguments[parameterIndex]);
                    deserializedArguments.Add(deserializedArgument);
                }
            }

            var result = method.Invoke(instance, deserializedArguments.ToArray());
            var resultAsJson = _serializer.ToJson(result, SerializationOptions.CamelCase);
            return resultAsJson;
        }
    }
}