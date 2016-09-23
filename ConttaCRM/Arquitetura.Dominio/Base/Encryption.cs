using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Arquitetura.Dominio.Base
{
    /// <summary>
    /// Uma classe para criptografar texto
    /// </summary>
    public class Encryption
    {
        private const string myPassword = "qea508d985swd25";
        private const string mySaltBase64 = "cWVhNTA4ZDk4NXN3ZDI1";

        /// <summary>
        /// Criptografar texto
        /// </summary>
        /// <param name="inputText">O texto a ser criptografado</param>
        /// <returns>O texto criptografado</returns>
        public static string Encrypt(string inputText)
        {
            string password = myPassword;
            byte[] salt = Encoding.Unicode.GetBytes(mySaltBase64);

            var inputBytes = Encoding.Unicode.GetBytes(inputText);

            var pdb = new Rfc2898DeriveBytes(password, salt);

            using (var ms = new MemoryStream())
            {
                var alg = Rijndael.Create();

                alg.Key = pdb.GetBytes(32);
                alg.IV = pdb.GetBytes(16);

                using (var cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                }

                var result = HttpServerUtility.UrlTokenEncode(ms.ToArray());

                return result;
            }
        }

        /// <summary>
        /// Criptografar inteiro
        /// </summary>
        /// <param name="inputText">O inteiro a ser criptografado</param>
        /// <returns>O texto criptografado</returns>
        public static string Encrypt(Int32 inputInt)
        {
            string inputText = inputInt.ToString();

            string password = myPassword;
            byte[] salt = Encoding.Unicode.GetBytes(mySaltBase64);

            var inputBytes = Encoding.Unicode.GetBytes(inputText);

            var pdb = new Rfc2898DeriveBytes(password, salt);

            using (var ms = new MemoryStream())
            {
                var alg = Rijndael.Create();

                alg.Key = pdb.GetBytes(32);
                alg.IV = pdb.GetBytes(16);

                using (var cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                }

                return HttpServerUtility.UrlTokenEncode(ms.ToArray());
            }
        }

        /// <summary>
        /// Descriptografa o texto para string
        /// </summary>
        /// <param name="inputText">O texto a ser descriptografado</param>
        /// <returns>O texto descriptografado</returns>
        public static string DecryptToString(string inputText)
        {
            string password = myPassword;
            byte[] salt = Encoding.Unicode.GetBytes(mySaltBase64);

            var inputBytes = HttpServerUtility.UrlTokenDecode(inputText);

            var pdb = new Rfc2898DeriveBytes(password, salt);

            using (var ms = new MemoryStream())
            {
                var alg = Rijndael.Create();

                alg.Key = pdb.GetBytes(32);
                alg.IV = pdb.GetBytes(16);

                using (var cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                }

                return Encoding.Unicode.GetString(ms.ToArray());
            }

        }

        /// <summary>
        /// Descriptografa o texto para inteiro
        /// </summary>
        /// <param name="inputText">O texto a ser descriptografado</param>
        /// <returns>Um inteiro descriptografado</returns>
        public static Int32 DecryptToInt32(string inputText)
        {
            string password = myPassword;
            byte[] salt = Encoding.Unicode.GetBytes(mySaltBase64);

            var inputBytes = HttpServerUtility.UrlTokenDecode(inputText);

            var pdb = new Rfc2898DeriveBytes(password, salt);

            using (var ms = new MemoryStream())
            {
                var alg = Rijndael.Create();

                alg.Key = pdb.GetBytes(32);
                alg.IV = pdb.GetBytes(16);

                using (var cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                }

                return Int32.Parse(Encoding.Unicode.GetString(ms.ToArray()));
            }

        }
    }
}
