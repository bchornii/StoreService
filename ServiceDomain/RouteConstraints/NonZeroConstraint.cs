﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Web.Http.Routing;

namespace ServiceDomain.RouteConstraints
{
    public class NonZeroConstraint : IHttpRouteConstraint
    {
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            object value = null;
            if(values.TryGetValue(parameterName, out value) && value != null)
            {
                long longValue;
                if(value is long)
                {
                    longValue = (long)value;
                    return longValue != 0;
                }

                string valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
                if(long.TryParse(valueString, out longValue))
                {
                    return longValue != 0;
                }
            }
            return false;
        }
    }
}