/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dolittle.DependencyInversion;
using Dolittle.Serialization.Json;
using Dolittle.Types;
using WebAssembly;

namespace Dolittle.Interaction.WebAssembly.Interop
{
    /// <summary>
    /// Represents a system that is capable of invoking methods on types
    /// </summary>
    public class DotNetRuntime
    {
        static IContainer _container;
        static ISerializer _serializer;
        static ITypeFinder _typeFinder;

        static JSObject _dotNetRuntime;

        internal static void Initialize(IContainer container)
        {
            _container = container;
            _serializer = _container.Get<ISerializer>();
            _typeFinder = _container.Get<ITypeFinder>();

            _dotNetRuntime = (JSObject)global::WebAssembly.Runtime.GetGlobalObject("_dotNetRuntime");
        }

        /// <summary>
        /// Invoke a method
        /// </summary>
        /// <param name="invocationId"></param>
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
        public static void BeginInvoke(string invocationId, string typeName, string methodName, string argumentsAsJson)
        {
            try
            {
                var methodAndInstance = GetMethodAndInstanceOfType(typeName, methodName);
                var deserializedArguments = DeserializeArguments(methodAndInstance.method, argumentsAsJson);
                var task = methodAndInstance.method.Invoke(methodAndInstance.instance, deserializedArguments.ToArray()) as Task;
                task.ContinueWith(t => {
                    var resultProperty = t.GetType().GetProperty("Result");
                    if( resultProperty != null ) 
                    {
                        var result = resultProperty.GetValue(t);
                        var serializedResult = SerializeResult(result);
                        _dotNetRuntime.Invoke("succeeded", invocationId, serializedResult);
                    } else _dotNetRuntime.Invoke("succeeded", invocationId);
                });
            }
            catch (Exception ex)
            {
                _dotNetRuntime.Invoke("failed", invocationId, $"{ex.Message}\n{ex.StackTrace}");
            }
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
            try
            {
                var methodAndInstance = GetMethodAndInstanceOfType(typeName, methodName);
                var deserializedArguments = DeserializeArguments(methodAndInstance.method, argumentsAsJson);
                var result = methodAndInstance.method.Invoke(methodAndInstance.instance, deserializedArguments.ToArray());
                var resultAsJson = SerializeResult(result);
                return resultAsJson;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static(object instance, MethodInfo method) GetMethodAndInstanceOfType(string typeName, string methodName)
        {
            var type = _typeFinder.FindTypeByFullName(typeName);
            var instance = _container.Get(type);
            var method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);

            return (
                instance: instance,
                method: method
            );
        }

        static string SerializeResult(object result)
        {
            return _serializer.ToJson(result, SerializationOptions.CamelCase);
        }

        static IEnumerable<object> DeserializeArguments(MethodInfo method, string argumentsAsJson)
        {
            var deserialized = _serializer.FromJson<object[]>(argumentsAsJson);
            var arguments = method.GetParameters().Select((parameter, index) => {
                var argumentAsJson = _serializer.ToJson(deserialized[index], SerializationOptions.CamelCase);
                return _serializer.FromJson(parameter.ParameterType, argumentAsJson);
            });
            return arguments;
        }
    }
}