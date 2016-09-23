using Arquitetura.Aplicacao.AOP;
using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Aggregates.ClienteAgg;
using Arquitetura.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Arquitetura.Aplicacao.Services.Interface
{
    public interface IClienteAppService : IDisposable
    {
        [Autenticacao]
        ClienteDTO AddCliente(ClienteDTO ClienteDTO);

        [Autenticacao]
        ClienteDTO UpdateCliente(ClienteDTO ClienteDTO);

        [Autenticacao]
        ClienteDTO UpdatePerfilCliente(ClienteDTO ClienteDTO);

        [Autenticacao]
        void RemoveCliente(int ClienteId);

        [Autenticacao]
        ClienteDTO FindCliente(int ClienteId);

        [Autenticacao]
        ClienteDTO FindCliente(string email);

        [Autenticacao]
        List<ClienteListDTO> FindClientes<KProperty>(string texto, Expression<Func<Cliente, KProperty>> orderByExpression, bool ascending, int pageIndex, int pageCount);

        [Autenticacao]
        List<ClienteListDTO> FindClientes<KProperty>(string texto, Expression<Func<Cliente, KProperty>> orderByExpression, bool ascending = true);

        [Autenticacao]
        long CountClientes(string texto);

        [Autenticacao]
        List<ClienteListDTO> GetMatriz();

        [Autenticacao]
        List<ClienteListDTO> GetFilial(int MatrizId);

        //Sem autenticacao
        ClienteDTO FindClienteNull(string login);
    }
}
