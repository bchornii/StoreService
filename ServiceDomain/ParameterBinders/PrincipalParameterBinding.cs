using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace ServiceDomain.ParameterBinders
{
    public class PrincipalParameterBinding : HttpParameterBinding
    {
        public PrincipalParameterBinding(HttpParameterDescriptor descriptor) 
            : base(descriptor) { }
        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, 
            HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            IPrincipal p = Thread.CurrentPrincipal;
            SetValue(actionContext, p);                        

            var tsc = new TaskCompletionSource<object>();
            tsc.SetResult(null);
            return tsc.Task;
        }
    }
}