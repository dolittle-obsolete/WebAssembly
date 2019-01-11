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

namespace Dolittle.Interaction.WebAssembly.Interop
{
    /// <summary>
    /// Represents a system that is capable of invoking methods on types
    /// </summary>
    public class MethodInvoker
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
        /// Invoke a method
        /// </summary>
        /// <param name="typeName">Full typename of type to invoke on - not the assembly qualified name, but the namespace+type</param>
        /// <param name="methodName">Name of the method to invoke</param>
        /// <param name="argumentsAsJson">Serialized arguments - expecting a serialized array of arguments that are each serialized as JSON</param>
        /// <returns>JSON serialized representation of the result</returns>
        /// <remarks>
        /// The invoker will do a best effort on type discovery and also on deserialization of any arguments. It does this by deserializing to the
        /// parameter type of the matching parameter by index of the argument list coming in.
        /// 
        /// Note: There is some overhead in doing the serialization, it is therefor not recommended call this from tight inner lops
        /// </remarks>
        public static string Invoke(string typeName, string methodName, string argumentsAsJson)
        {
            Console.WriteLine($"Calling '{methodName}' on '{typeName}' with '{argumentsAsJson}'");

            try
            {

                var type = _typeFinder.FindTypeByFullName(typeName);
                Console.WriteLine($"Type found : {type.AssemblyQualifiedName}");

                var instance = _container.Get(type);

                var method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
                
                Console.WriteLine($"Method found : {method}");

                var arguments = _serializer.FromJson<string[]>(argumentsAsJson);
                var deserializedArguments = new List<object>();
                var parameters = method.GetParameters();
                for (var parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
                {
                    var argument = arguments[parameterIndex];
                    if( argument.IndexOf("\"") == 0 ) argument = argument.Substring(1);
                    if( argument.IndexOf("\"") == argument.Length-1) argument = argument.Substring(0,argument.Length-1);
                    argument = argument.Replace("\\\"", "\"");

                    var parameter = parameters[parameterIndex];

                    Console.WriteLine($"Parameter '{parameter.Name}' with Type '{parameter.ParameterType.Name}' - value '{argument}'");

                    if (parameter.ParameterType == typeof(string))
                    {
                        deserializedArguments.Add(argument);
                    }
                    else if (parameter.ParameterType == typeof(Guid))
                    {
                        deserializedArguments.Add(Guid.Parse(argument));
                    }
                    else if( parameter.ParameterType == typeof(bool))
                    {
                        deserializedArguments.Add(bool.Parse(arguments[parameterIndex]));
                    }
                    else
                    {
                        var deserializedArgument = _serializer.FromJson(parameter.ParameterType, argument, SerializationOptions.CamelCase);
                        deserializedArguments.Add(deserializedArgument);
                    }
                }

                var result = method.Invoke(instance, deserializedArguments.ToArray());
                var resultAsJson = _serializer.ToJson(result, SerializationOptions.CamelCase);
                return resultAsJson;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : "+ex);
                return string.Empty;

            }
        }
    }
}