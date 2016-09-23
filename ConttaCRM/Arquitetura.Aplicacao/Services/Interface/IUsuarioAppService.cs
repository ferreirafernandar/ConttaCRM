using Arquitetura.Aplicacao.AOP;
using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Aggregates.UsuarioAgg;
using Arquitetura.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Arquitetura.Aplicacao.Services.Interface
{
    public interface IUsuarioAppService : IDisposable
    {
        [Autenticacao]
        UsuarioDTO AddUsuario(UsuarioDTO usuarioDTO);

        [Autenticacao]
        UsuarioDTO UpdateUsuario(UsuarioDTO usuarioDTO);

        [Autenticacao]
        UsuarioDTO UpdatePerfilUsuario(UsuarioDTO usuarioDTO);

        [Autenticacao]
        void RemoveUsuario(int usuarioId);

        [Autenticacao]
        UsuarioDTO FindUsuario(int usuarioId);

        [Autenticacao]
        UsuarioDTO FindUsuario(string email);

        [Autenticacao]
        List<UsuarioListDTO> FindUsuarios<KProperty>(string texto, Expression<Func<Usuario, KProperty>> orderByExpression, bool ascending, int pageIndex, int pageCount);

        [Autenticacao]
        List<UsuarioListDTO> FindUsuarios<KProperty>(string texto, Expression<Func<Usuario, KProperty>> orderByExpression, bool ascending = true);

        [Autenticacao]
        long CountUsuarios(string texto);

        void AutenticarUsuario(string email, string senha, bool continuarConectado);

        [Autenticacao]
        void AlterarSenha(string senhaAtual, string novaSenha, string confirmaNovaSenha);

        //Sem autenticacao
        UsuarioDTO FindUsuarioNull(string login);

        //Sem autenticacao
        string GerarTokenSenha(int usuarioId);

        //Sem autenticacao
        void ValidarTokenSenha(string token);

        //Sem autenticacao
        void RedefinirSenhaComToken(string token, string senha, string confirmaSenha);
    }
}
