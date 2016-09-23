using System;
using System.Diagnostics;
using System.Globalization;
using System.Security;

namespace Arquitetura.Infraestrutura.Logging
{
    public sealed class TraceSourceLog : ILogger
    {
        #region Members

        TraceSource source;

        #endregion

        #region  Constructor

        /// <summary>
        /// Create a new instance of this trace manager
        /// </summary>
        public TraceSourceLog()
        {
            // Create default source
            source = new TraceSource("SanNuvens");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Trace internal message in configured listeners
        /// </summary>
        /// <param name="eventType">Event type to trace</param>
        /// <param name="message">Message of event</param>
        void TraceInternal(TraceEventType eventType, string message)
        {

            if (source != null)
            {
                try
                {
                    source.TraceEvent(eventType, (int)eventType, message);
                }
                catch (SecurityException)
                {
                    //Cannot access to file listener or cannot have
                    //privileges to write in event log etc...
                }
            }
        }
        #endregion

        #region ILogger Members

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILogger"/></param>
        /// <param name="args"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILogger"/></param>
        public void LogInfo(string message, params object[] args)
        {
            if (!String.IsNullOrEmpty(message) &&
                !String.IsNullOrEmpty(message))
            {
                var traceData = string.Format(CultureInfo.InvariantCulture, message, args);

                TraceInternal(TraceEventType.Information, traceData);
            }
        }
        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILogger"/></param>
        /// <param name="args"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILogger"/></param>
        public void LogWarning(string message, params object[] args)
        {

            if (!String.IsNullOrEmpty(message) &&
                !String.IsNullOrEmpty(message))
            {
                var traceData = string.Format(CultureInfo.InvariantCulture, message, args);

                TraceInternal(TraceEventType.Warning, traceData);
            }
        }

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILogger"/></param>
        /// <param name="args"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILogger"/></param>
        public void LogError(string message, params object[] args)
        {
            if (!String.IsNullOrEmpty(message) &&
                !String.IsNullOrEmpty(message))
            {
                var traceData = string.Format(CultureInfo.InvariantCulture, message, args);

                TraceInternal(TraceEventType.Error, traceData);
            }
        }

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILogger"/></param>
        /// <param name="exception"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILogger"/></param>
        /// <param name="args"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILogger"/></param>
        public void LogError(string message, Exception exception, params object[] args)
        {
            if (!String.IsNullOrEmpty(message) &&
                !String.IsNullOrEmpty(message))
            {
                var traceData = string.Format(CultureInfo.InvariantCulture, message, args);

                var exceptionData = exception.ToString(); // The ToString() create a string representation of the current exception

                TraceInternal(TraceEventType.Error, string.Format(CultureInfo.InvariantCulture, "{0} Exception:{1}", traceData, exceptionData));
            }
        }

        public void LogError(Exception exception, params object[] args)
        {
            string message;

            if (exception.InnerException == null)
                message = exception.Message;
            else
                message = exception.Message + " - " + exception.InnerException.Message;

            var traceData = string.Format(CultureInfo.InvariantCulture, message, args);

            var exceptionData = exception.ToString(); // The ToString() create a string representation of the current exception

            TraceInternal(TraceEventType.Error, string.Format(CultureInfo.InvariantCulture, "{0} Exception:{1}", traceData, exceptionData));
        }

        #endregion
    }
}
