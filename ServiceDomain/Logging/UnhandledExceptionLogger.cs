using NLog;
using System;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace ServiceDomain.Logging
{
    public sealed class UnhandledExceptionLogger : ExceptionLogger
    {
        private static readonly Logger _logger;
        static UnhandledExceptionLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }
        public override void Log(ExceptionLoggerContext context)
        {
            var message = new StringBuilder();
            var _event = new LogEventInfo();

            if (context.Request != null)
            {
                _event.Properties["correlationId"] = context.Request.GetCorrelationId();

                if (context.Request.Method != null)
                {
                    _event.Properties["method"] = context.Request.Method;
                    message.Append(" Method : ").Append(context.Request.Method);
                }
                if (context.Request.RequestUri != null)
                {
                    _event.Properties["uri"] = context.Request.RequestUri;
                    message.Append(" | Uri : ").Append(context.Request.RequestUri);
                }
            }
                        
            if(context.Exception != null)
            {
                var _exceptionMessage = GetAllExceptionMessages(context.Exception);                
                _event.Properties["excmessage"] = _exceptionMessage;
                message.Append(" | ExceptionMessage : ").Append(_exceptionMessage);
            }

            _event.LoggerName = _logger.Name;
            _event.Level = LogLevel.Error;                
            _event.Message = message.ToString();
            _logger.Log(_event);
        }

        private string GetAllExceptionMessages(Exception ex, string separator = " | ")
        {
            if(ex.InnerException == null)
            {
                return ex.Message;
            }
            return ex.Message + separator + GetAllExceptionMessages(ex.InnerException, separator);
        }                     
    }
}