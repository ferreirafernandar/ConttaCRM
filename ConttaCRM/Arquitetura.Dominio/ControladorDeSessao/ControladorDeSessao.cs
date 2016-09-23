using Arquitetura.Dominio.Aggregates.UsuarioAgg;
using System;
using System.Web;
using System.Web.Security;

namespace Arquitetura.Dominio.ControladorDeSessao
{
    public static class ControladorDeSessao
    {
        #region Membros privados

        const string NOME_SESSAO_AUTENTICACAO = "VisaoWeb_FormsAuthentication_Session";

        private static GerenciadorDeSessao _gerenciador { get; set; }

        private static GerenciadorDeSessao ObtenhaSessao()
        {
            if (HttpContext.Current == null)
            {
                if (_gerenciador == null)
                {
                    _gerenciador = new GerenciadorDeSessao();
                }
                return _gerenciador;
            }
            else
            {
                var gerenciador = (GerenciadorDeSessao)HttpContext.Current.Session[NOME_SESSAO_AUTENTICACAO];
                if (gerenciador == null)
                {
                    gerenciador = new GerenciadorDeSessao();
                    FormsAuthentication.SignOut();
                }
                return gerenciador;
            }
        }

        private static void SetSession(GerenciadorDeSessao gerenciador)
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Session[NOME_SESSAO_AUTENTICACAO] = gerenciador;
        }


        #endregion

        #region Membros públicos

        public static void Autenticar(Usuario usuario, bool continuarConectado = false)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException("usuario");
            }

            if (!usuario.Ativo)
            {
                throw new Exception("Usuário inativo.");
            }

            var gerenciador = ObtenhaSessao();
            gerenciador.SetUsuario(usuario);
            FormsAuthentication.SetAuthCookie(usuario.NomeUsuario, continuarConectado);

            SetSession(gerenciador);
        }

        public static void Desautenticar()
        {
            SetSession(null);
            FormsAuthentication.SignOut();
        }

        public static Usuario GetUsuario()
        {
            ValidaAutenticacao();
            var appManager = ObtenhaSessao();
            return appManager.Usuario;
        }

        public static string GetPastaRaiz()
        {
            ValidaAutenticacao();
            var appManager = ObtenhaSessao();

            if (string.IsNullOrWhiteSpace(appManager.PastaRaiz))
                throw new Exception("Sem permissão.");

            return appManager.PastaRaiz;
        }

        public static bool EstaAutenticado()
        {
            var gerenciador = ObtenhaSessao();
            if (gerenciador.Usuario == null || gerenciador.Usuario.Id == 0)
                return false;

            return true;
        }

        public static void ValidaAutenticacao()
        {
            if (!EstaAutenticado())
                throw new Exception("Usuário não autenticado.");
        }

        #endregion
    }
}
