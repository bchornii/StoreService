﻿using Newtonsoft.Json.Serialization;
using ServiceDomain.ExceptionHandler;
using ServiceDomain.Filters;
using ServiceDomain.RouteConstraints;
using System.Net;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using System;
using ServiceDomain.Tracing;
using System.Web.Http.ValueProviders;
using ServiceDomain.ValueProvider;
using System.Security.Principal;
using ServiceDomain.ParameterBinders;
using ServiceDomain.DTOs;
using ServiceDomain.Logging;

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

            // Web API global exception logger
            config.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger());

            // Web API custom tracer
            config.Services.Replace(typeof(System.Web.Http.Tracing.ITraceWriter), new NLogTracer());

            // Web API custom value provider (cookie)
            config.Services.Add(typeof(ValueProviderFactory), new CookieValueProviderFactory());

            // Web API custom HttpParameterBindings
            config.ParameterBindingRules.Add(typeof(uBookDto), param => new BookParamaterBinding(param));
            config.ParameterBindingRules.Add(typeof(IPrincipal), param => new PrincipalParameterBinding(param));
            
        }
    }
}
