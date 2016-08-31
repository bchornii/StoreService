using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;

namespace ServiceDomain.ValueProvider
{
    public class CookieValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(HttpActionContext actionContext)
        {
            return new CookieValueProvider(actionContext);
        }
    }
}