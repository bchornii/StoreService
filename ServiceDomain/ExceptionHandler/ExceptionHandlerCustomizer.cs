using System;
using System.Collections.Generic;
using System.Net;

namespace ServiceDomain.ExceptionHandler
{
    public interface IExceptionHandlerCustomizer
    {
        IDictionary<Type, KeyValuePair<HttpStatusCode, string>> Collection { get; }
        HttpStatusCode GetStatusCode(Type type);
        HttpStatusCode GetStatusCode<T>() where T : Exception;
        string GetMessage(Type type);
        string GetMessage<T>() where T : Exception;
        void BindToException<T>(HttpStatusCode statusCode) where T : Exception;
        void BindToException<T>(HttpStatusCode statusCode, string message) where T : Exception;
    }
    public class ExceptionHandlerCustomizer : IExceptionHandlerCustomizer
    {
        public IDictionary<Type, KeyValuePair<HttpStatusCode, string>> Collection { get; private set; }
        public HttpStatusCode DefaultUnhandledStatusCode { get; set; }
        public ExceptionHandlerCustomizer() : this(HttpStatusCode.InternalServerError)
        {
            Collection = new Dictionary<Type, KeyValuePair<HttpStatusCode, string>>();
        }
        public ExceptionHandlerCustomizer(HttpStatusCode statusCode)
        {
            DefaultUnhandledStatusCode = statusCode;
        }
        public HttpStatusCode GetStatusCode(Type type)
        {
            KeyValuePair<HttpStatusCode, string> _value;
            return Collection.TryGetValue(type, out _value) ? _value.Key : DefaultUnhandledStatusCode;
        }
        public HttpStatusCode GetStatusCode<T>() where T : Exception
        {
            return GetStatusCode(typeof(T));
        }
        public string GetMessage(Type type)
        {
            KeyValuePair<HttpStatusCode, string> _value;
            return Collection.TryGetValue(type, out _value) ? _value.Value : string.Empty;
        }
        public string GetMessage<T>() where T : Exception
        {
            return GetMessage(typeof(T));
        }
        public void BindToException<T>(HttpStatusCode statusCode) where T : Exception
        {
            BindToException<T>(statusCode, string.Empty);
        }
        public void BindToException<T>(HttpStatusCode statusCode, string message) where T : Exception
        {
            Collection[typeof(T)] = new KeyValuePair<HttpStatusCode, string>(statusCode, message);
        }
    }
}