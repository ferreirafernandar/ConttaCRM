using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arquitetura.Dominio.Aggregates.Base
{
    public static class Util
    {
        public static bool IsValidFolderName(string folderName)
        {
            bool validName = true;
            foreach (char ch in Path.GetInvalidFileNameChars())
            {
                if (folderName.Contains(ch))
                {
                    validName = false;
                    break;
                }
            }

            return validName;
        }

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

        public static string Capitalize(string value)
        {
            if (value == null)
                return string.Empty;

            if (value.Length == 0)
                return value;

            value = value.ToLower();

            StringBuilder result = new StringBuilder(value);
            result[0] = char.ToUpper(result[0]);
            for (int i = 1; i < result.Length; ++i)
            {
                if (char.IsWhiteSpace(result[i - 1]))
                    result[i] = char.ToUpper(result[i]);
            }

            result = result.Replace(" Da ", " da ");
            result = result.Replace(" Das ", " das ");
            result = result.Replace(" De ", " de ");
            result = result.Replace(" De, ", " de, ");
            result = result.Replace(" Do ", " do ");
            result = result.Replace(" Dos ", " dos ");
            result = result.Replace(" E ", " e ");
            result = result.Replace(" Ao ", " ao ");

            return result.ToString();
        }

        public static bool TryParseDateTime(string text, out Nullable<DateTime> nDate)
        {
            DateTime date;
            bool isParsed = DateTime.TryParse(text, out date);
            if (isParsed)
                nDate = new Nullable<DateTime>(date);
            else
                nDate = new Nullable<DateTime>();
            return isParsed;
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

        public static bool ValidarCPF(string cpf)
        {
            cpf = OnlyNumbers(cpf);

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

        public static bool ValidarCNPJ(string cnpj)
        {
            int[] digitos, soma, resultado;
            int nrDig;
            string ftmt;
            bool[] CNPJOk;
            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;

            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(cnpj.Substring(nrDig, 1));
                    if (nrDig <= 11)
                        soma[0] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1)));

                    if (nrDig <= 12)
                        soma[1] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1)));
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);

                    if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                        CNPJOk[nrDig] = (digitos[12 + nrDig] == 0);
                    else
                        CNPJOk[nrDig] = (digitos[12 + nrDig] == (11 - resultado[nrDig]));
                }

                return (CNPJOk[0] && CNPJOk[1]);
            }
            catch
            {
                return false;
            }
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
                return null;

            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = null;

            if (fi != null)
            {
                attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            }

            if ((attributes != null) && (attributes.Length > 0))
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string GetEnumDescription(string value, Type enumType)
        {
            if (value == null)
                return null;

            FieldInfo fi = enumType.GetField(value.ToString());
            DescriptionAttribute[] attributes = null;

            if (fi != null)
            {
                attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            }

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

        public static decimal Truncar(decimal valor)
        {
            valor *= 100;
            valor = Math.Truncate(valor);
            valor /= 100;

            return valor;
        }

        public static double Truncar(double valor)
        {
            valor *= 100;
            valor = Math.Truncate(valor);
            valor /= 100;

            return valor;
        }

        public static DateTime? ParseDateTime(string dateToParse)
        {
            DateTime? dataReferencia = null;
            DateTime parseDataReferencia;

            if (DateTime.TryParse(dateToParse, out parseDataReferencia))
                dataReferencia = parseDataReferencia;

            return dataReferencia;
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
            string emailDestinatario)
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
                destinatarios);
        }

        public static void SendEmail(
            string subject,
            string message,
            string contaEmail,
            string contaSenha,
            string contaSmtp,
            int contaPorta,
            bool ssl,
            List<string> destinatarios)
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

        public static void CrieNovoUsuario(string nomeUsuario, string senha, string nomePessoa)
        {
            if (ObtenhaUsuario(nomeUsuario) == null)
            {
                PrincipalContext principalContext = new PrincipalContext(ContextType.Machine);

                UserPrincipal userPrincipal = new UserPrincipal(principalContext);
                userPrincipal.Name = nomeUsuario;
                userPrincipal.SetPassword(senha);
                userPrincipal.Description = "Usuário criado pelo sistema Visão Web.";
                userPrincipal.DisplayName = nomePessoa;

                userPrincipal.Save();
            }
            else
            {
                throw new Exception("Já existe um usuário com este nome.");
            }
        }

        private static UserPrincipal ObtenhaUsuario(string nomeUsuario)
        {
            PrincipalContext principalContext = new PrincipalContext(ContextType.Machine);
            var user = UserPrincipal.FindByIdentity(principalContext, IdentityType.Name, nomeUsuario);

            return user;
        }

        private static UserPrincipal AltereSenhaUsuario(string nomeUsuario, string senhaAtual, string senhaNova)
        {
            PrincipalContext principalContext = new PrincipalContext(ContextType.Machine);
            var user = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, nomeUsuario);
            user.ChangePassword(senhaAtual, senhaNova);

            return user;
        }

        private static UserPrincipal RedefinaSenhaUsuario(string nomeUsuario, string senhaNova)
        {
            PrincipalContext principalContext = new PrincipalContext(ContextType.Machine);
            var user = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, nomeUsuario);
            user.SetPassword(senhaNova);

            return user;
        }

        //public static void Permissao(string nomeArquivo, string nomeUsuario)
        //{
        //    AddFileSecurity(nomeArquivo, nomeUsuario, FileSystemRights.ReadData, AccessControlType.Allow);
        //    RemoveFileSecurity(nomeArquivo, nomeUsuario, FileSystemRights.ReadData, AccessControlType.Allow);
        //}

        public static void AddDirectorySecurity(string directoryName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            DirectorySecurity dSecurity = Directory.GetAccessControl(directoryName);
            dSecurity.AddAccessRule(new FileSystemAccessRule(account, rights, controlType));
            Directory.SetAccessControl(directoryName, dSecurity);

        }

        public static void RemoveDirectorySecurity(string directoryName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            DirectorySecurity dSecurity = Directory.GetAccessControl(directoryName);
            dSecurity.RemoveAccessRule(new FileSystemAccessRule(account, rights, controlType));
            Directory.SetAccessControl(directoryName, dSecurity);
        }

        public static void AddFileSecurity(string fileName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            FileSecurity fSecurity = File.GetAccessControl(fileName);
            fSecurity.AddAccessRule(new FileSystemAccessRule(account, rights, controlType));
            File.SetAccessControl(fileName, fSecurity);

        }

        public static void RemoveFileSecurity(string fileName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            FileSecurity fSecurity = File.GetAccessControl(fileName);
            fSecurity.RemoveAccessRule(new FileSystemAccessRule(account, rights, controlType));
            File.SetAccessControl(fileName, fSecurity);
        }
    }
}
