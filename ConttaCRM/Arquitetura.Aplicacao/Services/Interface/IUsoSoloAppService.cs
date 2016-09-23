using Arquitetura.Aplicacao.AOP;
using Arquitetura.Dominio.Aggregates.UsoSoloAgg;
using Arquitetura.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Arquitetura.Aplicacao.Services.Interface
{
    public interface IUsoSoloAppService : IDisposable
    {
        [Autenticacao]
        UsoSoloDTO AddUsoSolo(UsoSoloDTO usoSoloDTO);

        [Autenticacao]
        UsoSoloDTO UpdateUsoSolo(UsoSoloDTO usoSoloDTO);

        [Autenticacao]
        void RemoveUsoSolo(int usoSoloId);

        [Autenticacao]
        UsoSoloDTO FindUsoSolo(int usoSoloId);

        [Autenticacao]
        List<UsoSoloListDTO> FindUsoSolos<KProperty>(string texto, Expression<Func<UsoSolo, KProperty>> orderByExpression, bool ascending, int pageIndex, int pageCount);

        [Autenticacao]
        List<UsoSoloListDTO> FindUsoSolos<KProperty>(string texto, Expression<Func<UsoSolo, KProperty>> orderByExpression, bool ascending = true);

        [Autenticacao]
        long CountUsoSolos(string texto);

        [Autenticacao]
        List<UsoSoloDTO> FindUsoSoloPorResponsavel(int responsavelId);
    }
}