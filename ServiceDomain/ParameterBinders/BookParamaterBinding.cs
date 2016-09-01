using ServiceDomain.DTOs;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace ServiceDomain.ParameterBinders
{
    public class BookParamaterBinding : HttpParameterBinding
    {
        public BookParamaterBinding(HttpParameterDescriptor descriptor) 
            : base(descriptor) { }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, 
            HttpActionContext actionContext, CancellationToken cancellationToken)
        {           
            var queryStrCollection = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);

            var parameterName = Descriptor.ParameterName;
            actionContext.ActionArguments[parameterName] = uBookDto.Parse(queryStrCollection[parameterName]);

            var tsc = new TaskCompletionSource<object>();
            tsc.SetResult(null);
            return tsc.Task;
        }
    }
}