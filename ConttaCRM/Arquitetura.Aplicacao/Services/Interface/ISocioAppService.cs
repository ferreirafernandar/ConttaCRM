using Arquitetura.Aplicacao.AOP;
using Arquitetura.Dominio.Aggregates.SocioAgg;
using Arquitetura.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Arquitetura.Aplicacao.Services.Interface
{
    public interface ISocioAppService : IDisposable
    {
        [Autenticacao]
        SocioDTO AddSocio(SocioDTO socioDTO);

        [Autenticacao]
        SocioDTO UpdateSocio(SocioDTO socioDTO);

        [Autenticacao]
        void RemoveSocio(int socioId);

        [Autenticacao]
        SocioDTO FindSocio(int socioId);

        [Autenticacao]
        List<SocioListDTO> FindSocios<KProperty>(string texto, Expression<Func<Socio, KProperty>> orderByExpression, bool ascending, int pageIndex, int pageCount);

        [Autenticacao]
        List<SocioListDTO> FindSocios<KProperty>(string texto, Expression<Func<Socio, KProperty>> orderByExpression, bool ascending = true);

        [Autenticacao]
        long CountSocios(string texto);

        [Autenticacao]
        bool JaExisteAdministrador(int entrevistaId);

        [Autenticacao]
        bool JaExisteAssinaNaReceita(int entrevistaId);

        [Autenticacao]
        bool JaExisteCpf(string cpf, int entrevistaId, int? socioId);

    }
}