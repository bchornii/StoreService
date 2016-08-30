using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http.Tracing;

namespace ServiceDomain.Tracing
{
    public sealed class NLogTracer : ITraceWriter
    {
        private static readonly Logger _logger;
        private static readonly Lazy<Dictionary<TraceLevel, Action<string>>> _loggingMap;
        public Dictionary<TraceLevel, Action<string>> Logger => _loggingMap.Value;
        static NLogTracer()
        {
            _logger = LogManager.GetCurrentClassLogger();
            _loggingMap = new Lazy<Dictionary<TraceLevel, Action<string>>>(() => new Dictionary<TraceLevel, Action<string>>
            {
                {TraceLevel.Info, _logger.Info },
                {TraceLevel.Debug, _logger.Debug },
                {TraceLevel.Error, _logger.Error },
                {TraceLevel.Fatal, _logger.Fatal },
                {TraceLevel.Warn, _logger.Warn }
            });
        }

        public void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            if (level != TraceLevel.Off)
            {
                var record = new TraceRecord(request, category, level);
                traceAction(record);
                Log(record);
            }
        }

        private void Log(TraceRecord record)
        {
            var message = new StringBuilder();

            if (record.Request != null)
            {
                if (record.Request.Method != null)
                {
                    message.Append(" Method : ").Append(record.Request.Method);
                }
                if (record.Request.RequestUri != null)
                {
                    message.Append(" | Uri : ").Append(record.Request.RequestUri);
                }
            }

            if (!string.IsNullOrEmpty(record.Category))
            {
                message.Append(" | Cat : ").Append(record.Category);
            }

            if (true)
            {
                message.Append(" | Type : ").Append(record.Kind);
            }


            if (!string.IsNullOrEmpty(record.Operator))
            {
                message.Append(" | Operator : ").Append(record.Operator);
            }

            if (!string.IsNullOrEmpty(record.Operation))
            {
                message.Append(" | Operation : ").Append(record.Operation);
            }

            if (!string.IsNullOrEmpty(record.Message))
            {
                message.Append(" | Msg : ").Append(record.Message);
            }

            if (record.Exception != null && !string.IsNullOrEmpty(record.Exception.GetBaseException().Message))
            {
                message.Append(" | ExcMsg : ").Append(record.Exception.GetBaseException().Message);
            }

            Logger[record.Level](message.ToString());
        }
    }
}