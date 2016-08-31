using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;

namespace ServiceDomain.ValueProvider
{
    public class CookieValueProvider : IValueProvider
    {
        private Dictionary<string, string> _values;
        public CookieValueProvider(HttpActionContext actionContext)
        {

        }
        public bool ContainsPrefix(string prefix)
        {
            throw new NotImplementedException();
        }

        public ValueProviderResult GetValue(string key)
        {
            throw new NotImplementedException();
        }
    }
}