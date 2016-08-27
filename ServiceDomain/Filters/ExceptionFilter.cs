using System;
using System.Net.Http;
using System.Web.Http.Filters;

namespace ServiceDomain.Filters
{
    // Exception filter could be register in several ways :
    // - by action
    // - by controller
    // - globally
    public class NotImpExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if(actionExecutedContext.Exception is NotImplementedException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.NotImplemented);
            }
        }
    }
}