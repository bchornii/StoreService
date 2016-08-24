using Newtonsoft.Json.Serialization;
using ServiceDomain.RouteConstraints;
using System.Web.Http;
using System.Web.Http.Routing;

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

            // Serialization
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API routes

            // Convention routing
            // config.Routes.MapHttpRoute(
            //     name: "DefaultApi",
            //     routeTemplate: "api/{controller}/{id}/{name}",
            //     defaults: new { id = RouteParameter.Optional }
            // );            
        }
    }
}
