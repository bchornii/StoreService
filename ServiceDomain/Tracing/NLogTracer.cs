using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Tracing;

namespace ServiceDomain.Tracing
{
    public sealed class NLogTracer : ITraceWriter
    {
        private static readonly Logger _logger;
        private static readonly Dictionary<TraceLevel, LogLevel> _loggingMap;
        #region
        //private static readonly Lazy<Dictionary<TraceLevel, Action<string>>> _loggingMap;
        //public Dictionary<TraceLevel, Action<string>> Logger => _loggingMap.Value; 
        #endregion
        static NLogTracer()
        {
            _logger = LogManager.GetCurrentClassLogger();
            _loggingMap = new Dictionary<TraceLevel, LogLevel>
            {
                { TraceLevel.Info, LogLevel.Info },
                { TraceLevel.Debug, LogLevel.Debug },
                { TraceLevel.Error, LogLevel.Error },
                { TraceLevel.Fatal, LogLevel.Fatal },
                { TraceLevel.Warn, LogLevel.Warn }
            };
            #region trace level - action map
            //_loggingMap = new Lazy<Dictionary<TraceLevel, Action<string>>>(() => new Dictionary<TraceLevel, Action<string>>
            //{
            //    {TraceLevel.Info, _logger.Info },
            //    {TraceLevel.Debug, _logger.Debug },
            //    {TraceLevel.Error, _logger.Error },
            //    {TraceLevel.Fatal, _logger.Fatal },
            //    {TraceLevel.Warn, _logger.Warn }
            //});
            #endregion
        }

        public void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            if (level != TraceLevel.Off)
            {
                var record = new TraceRecord(request, category, level);
                traceAction(record);
                LogEvent(record);
            }
        }

        private void LogEvent(TraceRecord record)
        {
            var _event = new LogEventInfo();
            var message = new StringBuilder();

            if (record.Request != null)
            {
                _event.Properties["correlationId"] = record.Request.GetCorrelationId();

                if (record.Request.Method != null)
                {
                    _event.Properties["method"] = record.Request.Method;
                    message.Append(" Method : ").Append(record.Request.Method);
                }
                if (record.Request.RequestUri != null)
                {
                    _event.Properties["uri"] = record.Request.RequestUri;
                    message.Append(" | Uri : ").Append(record.Request.RequestUri);
                }
            }
            
            if (!string.IsNullOrEmpty(record.Category))
            {
                _event.Properties["category"] = record.Category;
                message.Append(" | Cat : ").Append(record.Category);
            }

            if (true)
            {
                _event.Properties["type"] = record.Kind.ToString();
                message.Append(" | Type : ").Append(record.Kind);
            }


            if (!string.IsNullOrEmpty(record.Operator))
            {
                _event.Properties["operator"] = record.Operator;
                message.Append(" | Operator : ").Append(record.Operator);
            }

            if (!string.IsNullOrEmpty(record.Operation))
            {
                _event.Properties["operation"] = record.Operation;
                message.Append(" | Operation : ").Append(record.Operation);
            }

            if (!string.IsNullOrEmpty(record.Message))
            {
                _event.Properties["message"] = record.Message;
                message.Append(" | Msg : ").Append(record.Message);
            }

            if (record.Exception != null && !string.IsNullOrEmpty(record.Exception.GetBaseException().Message))
            {
                _event.Properties["excmessage"] = record.Exception.GetBaseException().Message;
                message.Append(" | ExcMsg : ").Append(record.Exception.GetBaseException().Message);
            }
            
            _event.LoggerName = _logger.Name;
            _event.Level = _loggingMap[record.Level];
            _event.Message = message.ToString();                        
            _logger.Log(_event);
        }
    }
}