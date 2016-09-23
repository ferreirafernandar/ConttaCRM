namespace Arquitetura.Infraestrutura.Logging
{
    /// <summary>
    /// A Trace Source base, log factory
    /// </summary>
    public class EmailTraceSourceLogFactory : ILoggerFactory
    {
        /// <summary>
        /// Create the trace source log
        /// </summary>
        /// <param name="configuration">Configuration string</param>
        /// <returns>New ILog based on Trace Source infrastructure</returns>
        public ILogger Create()
        {
            return new EmailTraceSourceLog();
        }
    }
}
