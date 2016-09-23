using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Domino.Base.Specification;
using System;

namespace Arquitetura.Dominio.Aggregates.UsuarioAgg
{
    public static class UsuarioSpecifications
    {
        public static Specification<Usuario> ConsultaTexto(string texto)
        {
            Specification<Usuario> spec = new DirectSpecification<Usuario>(c => true);
            
            if (!string.IsNullOrWhiteSpace(texto))
            {
                spec &= new DirectSpecification<Usuario>(c => 
                    c.Nome.ToUpper().Contains(texto.ToUpper()) ||
                    c.NomeUsuario.ToUpper().Contains(texto.ToUpper()) ||
                    c.Email.ToUpper().Contains(texto.ToUpper()));
            }

            return spec;
        }

        public static Specification<Usuario> ConsultaNomeUsuario(string nomeUsuario)
        {
            nomeUsuario = nomeUsuario.Trim();
            Specification<Usuario> spec = new DirectSpecification<Usuario>(c => c.NomeUsuario.ToLower() == nomeUsuario.ToLower());

            return spec;
        }

        public static Specification<TokenSenha> ConsulteTokenSenhaUsuario(int usuarioId)
        {
            return new DirectSpecification<TokenSenha>(c => c.UsuarioId == usuarioId);
        }

        public static Specification<TokenSenha> ConsulteTokenSenha(string token)
        {
            return new DirectSpecification<TokenSenha>(c => c.Token == token);
        }
    }
}
