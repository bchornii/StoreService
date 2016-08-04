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

            // Web API routes
            var constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("nonzero", typeof(NonZeroConstraint));
            config.MapHttpAttributeRoutes(constraintResolver);

            // Convention routing
            // config.Routes.MapHttpRoute(
            //     name: "DefaultApi",
            //     routeTemplate: "api/{controller}/{id}/{name}",
            //     defaults: new { id = RouteParameter.Optional }
            // );            
        }
    }
}
