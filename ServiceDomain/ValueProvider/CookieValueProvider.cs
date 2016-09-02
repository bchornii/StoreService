using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;

namespace ServiceDomain.ValueProvider
{
    public class CookieValueProvider : IValueProvider
    {
        private Dictionary<string, string> _values;
        public CookieValueProvider(HttpActionContext actionContext)
        {
            if(actionContext == null)
            {
                throw new ArgumentException("actionContext is null");
            }
            _values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var cookie in actionContext.Request.Headers.GetCookies())
            {
                foreach (var state in cookie.Cookies)
                {
                    _values[state.Name] = state.Value;
                }
            }
        }
        public bool ContainsPrefix(string prefix)
        {
            return _values.Keys.Contains(prefix);
        }

        public ValueProviderResult GetValue(string key)
        {
            string value;
            if(_values.TryGetValue(key, out value))
            {
                return new ValueProviderResult(value, value, CultureInfo.InvariantCulture);
            }
            return null;
        }
    }
}
