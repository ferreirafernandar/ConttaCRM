using Arquitetura.Dominio.ControladorDeSessao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Arquitetura.Web
{
    /// <summary>
    /// Summary description for contentresult
    /// </summary>
    public class contentresult : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/png";

            string path = context.Request.QueryString["path"];
            path = ObtenhaCaminho(path);

            context.Response.WriteFile(path);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private string ObtenhaPastaRaiz()
        {
            return ControladorDeSessao.GetPastaRaiz();
        }

        private string ObtenhaCaminho(string path)
        {
            //caminho absoluto
            if (path != null)
            {
                if (path.StartsWith("/"))
                {
                    path = path.Substring(1);
                }

                path = path.Replace("/", "\\");
                path = Path.Combine(ObtenhaPastaRaiz(), path);
            }

            return path;

            //caminho virtual
            //return Server.MapPath(path);
        }
    }
}