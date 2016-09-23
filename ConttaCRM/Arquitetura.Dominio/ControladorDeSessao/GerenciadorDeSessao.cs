using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Aggregates.UsuarioAgg;
using System;

namespace Arquitetura.Dominio.ControladorDeSessao
{
    public class GerenciadorDeSessao
    {
        public Usuario Usuario { get; private set; }

        public DateTime? DataHoraLogin { get; private set; }

        public string PastaRaiz { get; private set; }

        public void SetUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                Usuario = null;
                DataHoraLogin = null;
            }
            else
            {
                Usuario = usuario;
                DataHoraLogin = DateTime.Now;
            }
        }
    }
}
