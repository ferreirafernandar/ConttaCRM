using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Reflection;
using System.ComponentModel;
using System.Linq.Expressions;
using System.IO;
using Arquitetura.DTO;
using System.Net;

namespace Arquitetura.Infraestrutura.Util
{
    public class Util
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress ma = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string TitleToUrl(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            byte[] bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(text);
            text = System.Text.Encoding.UTF8.GetString(bytes);

            return Regex.Replace(text, @"[^A-Za-z0-9_\.~]+", "-");
        }

        public static bool ValidarCPF(string cpf)
        {
            if (cpf.Length != 11)
                return false;

            bool igual = true;
            for (int i = 1; i < 11 && igual; i++)
                if (cpf[i] != cpf[0])
                    igual = false;

            if (igual || cpf == "12345678909")
                return false;

            int[] numeros = new int[11];
            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(
                    cpf[i].ToString());

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else if (numeros[10] != 11 - resultado)
                return false;

            return true;
        }

        public static string OnlyNumbers(string toNormalize)
        {
            if (toNormalize == null)
                return null;

            List<char> numbers = new List<char>("0123456789");
            StringBuilder toReturn = new StringBuilder(toNormalize.Length);
            CharEnumerator enumerator = toNormalize.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (numbers.Contains(enumerator.Current))
                    toReturn.Append(enumerator.Current);
            }

            return toReturn.ToString();
        }

        public static string FormataCpfCnpj(string text)
        {
            text = OnlyNumbers(text);

            if (string.IsNullOrWhiteSpace(text))
                return null;

            if (text.Length == 11)
                return Convert.ToUInt64(text).ToString(@"000\.000\.000\-00");

            else if (text.Length == 14)
                return Convert.ToUInt64(text).ToString(@"00\.000\.000\/0000\-00");

            else
                return null;
        }

        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            if (value == null)
                return string.Empty;

            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if ((attributes != null) && (attributes.Length > 0))
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string FormatarTelefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return null;

            if (telefone.Length < 10)
                return telefone;

            int tam = telefone.Length - 6;
            string aux = string.Empty;
            for (int cont = 0; cont < tam; cont++)
                aux = aux + "#";

            long aa = long.Parse(telefone);

            var formatado = String.Format("{0:(##) ####-" + aux + "}", aa);

            return formatado;
        }

        public static string GerarNumeroProtocolo()
        {
            var data = GetDataComMilissecondMaiorQueZero();

            string ano = data.Year.ToString();
            string toReverse = data.Day.ToString().PadLeft(2, '0') + data.Month.ToString().PadLeft(2, '0') + data.Hour.ToString().PadLeft(2, '0') + data.Minute.ToString().PadLeft(2, '0') + data.Millisecond.ToString().PadLeft(2, '0').Substring(0, 2);
            string protocolo = Reverse(toReverse) + ano;

            return protocolo;
        }

        public static string Reverse(string texto)
        {
            if (texto == null)
            {
                return null;
            }

            char[] charArray = texto.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static bool IsDebugMode()
        {
            bool isDebug = false;

#if DEBUG
            isDebug = true;
#endif

            return isDebug;
        }

        public static LambdaExpression GetExpression<T>(string propertyName)
        {
            Type type = typeof(T);
            var parameter = Expression.Parameter(type);
            var memberExpression = Expression.Property(parameter, propertyName);
            var lambdaExpression = Expression.Lambda(memberExpression, parameter);
            return lambdaExpression;
        }

        public static byte[] StreamToByte(Stream input, long position = 0)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.Position = position;
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static void SendEmailAsync(
            string assunto,
            string mensagem,
            string contaEmail,
            string contaSenha,
            string contaSmtp,
            int contaPorta,
            bool ssl,
            string emailDestinatario)
        {
            SmtpClient client = new SmtpClient(contaSmtp, contaPorta);
            MailAddress from = new MailAddress(contaEmail);
            MailAddress to = new MailAddress(emailDestinatario);
            MailMessage message = new MailMessage(from, to);
            message.Body = mensagem;
            message.IsBodyHtml = true;
            message.Subject = assunto;
            client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            string userState = Guid.NewGuid().ToString();
            client.SendAsync(message, userState);
        }

        public static void SendEmail(
            string subject,
            string message,
            string contaEmail,
            string contaSenha,
            string contaSmtp,
            int contaPorta,
            bool ssl,
            string emailDestinatario,
            List<AnexoEmailDTO> files = null)
        {
            List<string> destinatarios = new List<string>();
            if (!string.IsNullOrWhiteSpace(emailDestinatario))
                destinatarios.Add(emailDestinatario);

            SendEmail(subject,
                message,
                contaEmail,
                contaSenha,
                contaSmtp,
                contaPorta,
                ssl,
                destinatarios,
                files);
        }

        public static void SendEmail(
            string subject,
            string message,
            string contaEmail,
            string contaSenha,
            string contaSmtp,
            int contaPorta,
            bool ssl,
            List<string> destinatarios,
            List<AnexoEmailDTO> files = null)
        {
            var loginInfo = new NetworkCredential(contaEmail, contaSenha);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient(contaSmtp, contaPorta);

            msg.From = new MailAddress(contaEmail);
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            if (destinatarios == null || destinatarios.Count == 0)
            {
                throw new Exception("Nenhum destinatário informado.");
            }
            else
            {
                foreach (var destinatario in destinatarios)
                {
                    if (Util.IsValidEmail(destinatario))
                        msg.To.Add(new MailAddress(destinatario));
                }
            }

            if (files != null)
            {
                foreach (var file in files)
                {
                    Attachment anexo = new Attachment(file.ContentStream, file.FileName);
                    msg.Attachments.Add(anexo);
                }
            }

            smtpClient.EnableSsl = ssl;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                //Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                //Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                //Console.WriteLine("Message sent.");
            }
        }

        private static DateTime GetDataComMilissecondMaiorQueZero()
        {
            DateTime data = DateTime.Now;
            if (data.Millisecond == 0)
            {
                return GetDataComMilissecondMaiorQueZero();
            }

            return data;
        }
    }
}
