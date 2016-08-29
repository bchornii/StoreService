using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace ServiceDomain.Logging
{
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            base.Log(context);
        }
        
        private string RequestToString(HttpRequestMessage request) => 
            request?.Method.ToString() + " " + request?.RequestUri.ToString();
    }
}