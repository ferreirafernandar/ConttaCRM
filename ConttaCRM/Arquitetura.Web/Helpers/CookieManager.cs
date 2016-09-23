using System;
using System.Web;

namespace Arquitetura.Web.Helpers
{
    public static class CookieManager
    {
        const string COOKIE_NAME = "visaoweb.appsettings";

        public static bool CreateCookie(DateTime? expiresDate, bool httpOnly = true, bool update = true)
        {
            try
            {
                HttpContext context = HttpContext.Current;
                HttpCookie cookie = context.Request.Cookies.Get(COOKIE_NAME);

                if (cookie == null)
                {
                    cookie = new HttpCookie(COOKIE_NAME);
                    cookie.Expires = expiresDate ?? DateTime.Now.AddYears(100);
                    cookie.HttpOnly = httpOnly;
                    context.Response.Cookies.Add(cookie);
                }
                else if (update)
                {
                    cookie.Expires = expiresDate ?? DateTime.Now.AddYears(100);
                    cookie.HttpOnly = httpOnly;
                    context.Response.Cookies.Set(cookie);
                }
                else
                {
                    return true;
                }

                HttpCookie readCookie = context.Request.Cookies.Get(COOKIE_NAME);
                return (readCookie != null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool SetCookieValue(string key, string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    return false;

                HttpContext context = HttpContext.Current;
                HttpCookie cookie = context.Request.Cookies.Get(COOKIE_NAME);

                if (cookie == null)
                {
                    CreateCookie(null);
                    cookie = context.Request.Cookies.Get(COOKIE_NAME);
                    if (cookie == null)
                        return false;
                }

                cookie.Values.Set(key, value ?? string.Empty);
                context.Response.Cookies.Add(cookie);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtém o valor de uma respectiva chave
        /// </summary>
        /// <param name="key">Chave referente ao valor a ser buscado</param>
        /// <returns>Valor referente à chave</returns>
        public static string GetCookieValue(string key)
        {
            try
            {
                HttpContext context = HttpContext.Current;
                HttpCookie cookie = context.Request.Cookies.Get(COOKIE_NAME);

                if (cookie == null)
                    return null;

                return cookie.Values.Get(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}