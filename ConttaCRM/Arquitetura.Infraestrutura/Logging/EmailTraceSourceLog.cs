using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Security;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace Arquitetura.Infraestrutura.Logging
{
    /// <summary>
    /// Implementation of contract <see cref="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILogger"/>
    /// using System.Diagnostics API.
    /// </summary>
    public sealed class EmailTraceSourceLog : ILogger
    {
        #region Members

        string nomeAplicacao;
        string emitenteEmail;
        string emitenteNome;
        string emitenteSenha;
        string destinatarioEmail;
        string clienteSmtp;
        int porta = 25;
        bool ssl = false;
        int timeOut = 100000;
        readonly string _configuration;
        bool enableDebugMode = false;

        #endregion

        #region  Constructor

        /// <summary>
        /// Create a new instance of this trace manager
        /// </summary>
        public EmailTraceSourceLog()
        {
            _configuration = ConfigurationManager.AppSettings["EmailTraceSourceLog"];

            if (_configuration != null)
            {

                nomeAplicacao = GetValueConfig("nomeAplicacao");
                emitenteEmail = GetValueConfig("emitenteEmail");
                emitenteSenha = GetValueConfig("emitenteSenha");
                destinatarioEmail = GetValueConfig("destinatarioEmail");
                clienteSmtp = GetValueConfig("clienteSmtp");
                ssl = Convert.ToBoolean(GetValueConfig("ssl"));
                if (!string.IsNullOrWhiteSpace(GetValueConfig("porta")))
                    porta = int.Parse(GetValueConfig("porta"));
                enableDebugMode = Convert.ToBoolean(GetValueConfig("enableDebugMode"));
            }
        }

        #endregion

        #region Private Methods

        string GetValueConfig(string value)
        {
            string result = _configuration.Substring(_configuration.IndexOf(value));
            result = result.Substring(0, result.IndexOf(';'));
            result = result.Substring(value.Length + 1);
            return result;
        }

        /// <summary>
        /// Trace internal message in configured listeners
        /// </summary>
        /// <param name="eventType">Event type to trace</param>
        /// <param name="message">Message of event</param>
        void TraceInternal(TraceEventType eventType, string message)
        {
            if (!string.IsNullOrWhiteSpace(emitenteEmail) &&
                !string.IsNullOrWhiteSpace(emitenteSenha) &&
                !string.IsNullOrWhiteSpace(destinatarioEmail) &&
                !string.IsNullOrWhiteSpace(clienteSmtp))
            {
                try
                {
                    bool isDebug = false;
                    #if DEBUG
                    isDebug = true;
                    #endif

                    if (enableDebugMode || !isDebug)
                    {
                        NetworkCredential loginInfo = new NetworkCredential(emitenteEmail, emitenteSenha);
                        MailMessage msg = new MailMessage();
                        msg.From = new MailAddress(emitenteEmail, emitenteNome);

                        foreach (var item in destinatarioEmail.Split(','))
                            msg.To.Add(new MailAddress(item));

                        msg.Subject = "Relatório de erro de " + nomeAplicacao;
                        message = String.Format("Event Type: {0}<br />{1}", eventType, message);
                        msg.Body = message;
                        msg.IsBodyHtml = true;
                        SmtpClient client = new SmtpClient(clienteSmtp);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.EnableSsl = ssl;
                        client.Port = porta;
                        client.Timeout = timeOut;
                        client.UseDefaultCredentials = false;
                        client.Credentials = loginInfo;
                        client.Send(msg);
                    }
                }
                catch
                {
                    //Cannot send e-mail
                }
            }
        }

        void TraceInternal(TraceEventType eventType, Exception ex)
        {
            if (!string.IsNullOrWhiteSpace(emitenteEmail) &&
                !string.IsNullOrWhiteSpace(emitenteSenha) &&
                !string.IsNullOrWhiteSpace(destinatarioEmail) &&
                !string.IsNullOrWhiteSpace(clienteSmtp))
            {
                try
                {
                    bool isDebug = false;
                    string modo = "Release";
                    string rawException = ex.ToString();

#if DEBUG
                    isDebug = true;
                    modo = "Debug";
#endif

                    if (enableDebugMode || !isDebug)
                    {
                        StringBuilder sb = new StringBuilder();
                        StackTrace trace = new StackTrace(ex, true);
                        StringBuilder erro = new StringBuilder();

                        if (ex.InnerException == null)
                            erro.Append(ex.Message);
                        else
                        {
                            do
                            {
                                erro.Append(string.Format("{0}<br />{1}<br /><br />", ex.Message, ex.InnerException.Message));
                                ex = ex.InnerException;
                            }
                            while (ex.InnerException != null);

                        }

                        int nomeArquivoId = trace.GetFrame(trace.FrameCount - 1).GetFileName().LastIndexOf('\\') + 1;
                        string arquivo = trace.GetFrame(trace.FrameCount - 1).GetFileName().Substring(nomeArquivoId);
                        string metodo = trace.GetFrame(trace.FrameCount - 1).GetMethod().Name;
                        string linha = trace.GetFrame(trace.FrameCount - 1).GetFileLineNumber().ToString();

                        sb.Append("<body>");
                        sb.Append("<center>");
                        sb.Append("<b>Relatório de Erro</b>");
                        sb.Append("<br>");
                        sb.Append("<br>");
                        sb.Append("<table border=\"1\" cellpadding=\"5\">");
                        sb.Append("<tr align=left>");
                        sb.Append("        <th>Empresa</td>");
                        sb.Append("        <td>" + nomeAplicacao + "</td>");
                        sb.Append("      </tr>");

                        sb.Append("      <tr align=left>");
                        sb.Append("        <th>Sistema</td>");
                        sb.Append("        <td>Nutratus: v1</td>");
                        sb.Append("      </tr>");

                        sb.Append("      <tr align=left>");
                        sb.Append("        <th>Data/Hora</td>");
                        sb.Append("        <td>" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "</td>");
                        sb.Append("      </tr>");

                        sb.Append("      <tr align=left>");
                        sb.Append("        <th>Modo</td>");
                        sb.Append("        <td>" + modo + "</td>");
                        sb.Append("      </tr>");

                        sb.Append("      <tr align=left>");
                        sb.Append("        <th>Origem</td>");
                        sb.Append("        <td>" + arquivo + "<br></td>");
                        sb.Append("      </tr>");

                        sb.Append("      <tr align=left>");
                        sb.Append("        <th>Método</td>");
                        sb.Append("        <td>" + metodo + "<br></td>");
                        sb.Append("      </tr>");

                        sb.Append("      <tr align=left>");
                        sb.Append("        <th>Linha</td>");
                        sb.Append("        <td>" + linha + "<br></td>");
                        sb.Append("      </tr>");

                        sb.Append("      <tr align=left>");
                        sb.Append("        <th>Erro Gerado</td>");
                        sb.Append("        <td>" + erro + "</td>");
                        sb.Append("      </tr>");

                        sb.Append("      <tr align=left>");
                        sb.Append("        <th>Raw Exception</td>");
                        sb.Append("        <td>" + rawException + "</td>");
                        sb.Append("      </tr>");

                        sb.Append("    </table>");
                        sb.Append("  </center>");
                        sb.Append("</body>");

                        NetworkCredential loginInfo = new NetworkCredential(emitenteEmail, emitenteSenha);
                        MailMessage msg = new MailMessage();
                        msg.From = new MailAddress(emitenteEmail, emitenteNome);

                        foreach (var item in destinatarioEmail.Split(','))
                            msg.To.Add(new MailAddress(item));

                        msg.Subject = "Relatório de erro de " + nomeAplicacao;
                        msg.Body = sb.ToString();
                        msg.IsBodyHtml = true;
                        SmtpClient client = new SmtpClient(clienteSmtp);
                        client.EnableSsl = ssl;
                        client.Port = porta;
                        client.Timeout = timeOut;
                        client.UseDefaultCredentials = false;
                        client.Credentials = loginInfo;
                        client.Send(msg);
                    }
                }
                catch
                {
                    //Cannot send e-mail
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

            TraceInternal(TraceEventType.Error, exception);
        }

        public void LogError(Exception exception)
        {
            TraceInternal(TraceEventType.Error, exception);
        }

        #endregion
    }
}
