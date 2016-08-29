using System;
using System.Net;
using System.Web.Http;
using ServiceDomain.Filters;
using System.Web.Http.Routing;
using Newtonsoft.Json.Serialization;
using ServiceDomain.ExceptionHandler;
using ServiceDomain.RouteConstraints;
using System.Web.Http.ExceptionHandling;

namespace ServiceDomain
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("nonzero", typeof(NonZeroConstraint));
            config.MapHttpAttributeRoutes(constraintResolver);

            // Web API serialization configuration
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API filters
            config.Filters.Add(new ValidateModelAttribute());

            // Web API global exception handler
            IExceptionHandlerCustomizer customizer = new ExceptionHandlerCustomizer { DefaultUnhandledStatusCode = HttpStatusCode.InternalServerError };
            customizer.BindToException<ArgumentException>(HttpStatusCode.Conflict, "ArgumentException handling is not implemented (global exc handler)");
            config.Services.Replace(typeof(IExceptionHandler), new UnhandledExceptionHandler
            {
                Customizer = customizer
            });          
        }
    }
}
