//using Arquitetura.DTO;
//using System.Configuration;
//using System.Web;

//namespace Arquitetura.Web.Helpers
//{
//    public class AppManagerSession
//    {
//        public UsuarioDTO Usuario { get; set; }
//    }

//    public class AppManagerCookie
//    {
//        private readonly string _cookieName = ConfigurationManager.AppSettings["CookieAppManager"];
//        /*
//        public void SetEmailLembrarMe(string email)
//        {
//            CookieHelper cookieHelper = new CookieHelper(_cookieName);
//            cookieHelper.SetCookieValue("emailLembrarMe", email);
//        }

//        public string GetEmailLembrarMe()
//        {
//            CookieHelper cookieHelper = new CookieHelper(_cookieName);
//            var xx = cookieHelper.GetCookieValue("emailLembrarMe"); ;
//            return cookieHelper.GetCookieValue("emailLembrarMe");
//        }

//        public void EsquecerEmailLembrarMe(string email)
//        {
//            CookieHelper cookieHelper = new CookieHelper(_cookieName);
//            cookieHelper.RemoveCookieKey("emailLembrarMe");
//        }
//     * */
//    }

//    public static class AppManager
//    {
//        #region Membros privados

//        private static AppManagerSession _appSession { get; set; }

//        private static AppManagerSession GetSession()
//        {
//            if (HttpContext.Current == null)
//            {
//                if (_appSession == null)
//                {
//                    _appSession = new AppManagerSession();
//                    _appSession.Usuario = null;
//                }
//                return _appSession;
//            }
//            else
//            {
//                var appManagerSession = (AppManagerSession)HttpContext.Current.Session["arquitetura_session"];
//                if (appManagerSession == null)
//                {
//                    appManagerSession = new AppManagerSession();
//                    appManagerSession.Usuario = null;

//                }
//                return appManagerSession;
//            }
//        }

//        private static void SetSession(AppManagerSession appManagerSession)
//        {
//            if (HttpContext.Current != null)
//                HttpContext.Current.Session["arquitetura_session"] = appManagerSession;
//        }


//        #endregion

//        #region Membros públicos

//        public static void SetUsuario(UsuarioDTO usuario)
//        {
//            var appManagerSession = GetSession();
//            appManagerSession.Usuario = usuario;
//            SetSession(appManagerSession);
//        }

//        public static UsuarioDTO GetUsuario()
//        {
//            var appManager = GetSession();
//            if (appManager == null)
//                return null;

//            return appManager.Usuario;
//        }

//        public static bool IsValidSession()
//        {
//            var appManager = GetSession();
//            if (appManager.Usuario == null || appManager.Usuario.Id == 0)
//                return false;
            
//            return true;
//        }


//        public static string GetEmailLogin()
//        {
//            if (string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
//                return "";

//            else
//                return HttpContext.Current.User.Identity.Name;
//        }

//        public static AppManagerCookie AppCookie()
//        {
//            return new AppManagerCookie();
//        }

//        #endregion
//    }
//}