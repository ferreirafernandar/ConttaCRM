using Arquitetura.Aplicacao.AOP;
using Arquitetura.Dominio.Aggregates.NumeroOficialAgg;
using Arquitetura.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Arquitetura.Aplicacao.Services.Interface
{
    public interface INumeroOficialAppService : IDisposable
    {
        [Autenticacao]
        NumeroOficialDTO AddNumeroOficial(NumeroOficialDTO NumeroOficialDTO);

        [Autenticacao]
        NumeroOficialDTO UpdateNumeroOficial(NumeroOficialDTO NumeroOficialDTO);

        [Autenticacao]
        void RemoveNumeroOficial(int NumeroOficialId);

        [Autenticacao]
        NumeroOficialDTO FindNumeroOficial(int NumeroOficialId);

        [Autenticacao]
        List<NumeroOficialListDTO> FindNumerosOficiais<KProperty>(string texto, Expression<Func<NumeroOficial, KProperty>> orderByExpression, bool ascending, int pageIndex, int pageCount);

        [Autenticacao]
        List<NumeroOficialListDTO> FindNumerosOficiais<KProperty>(string texto, Expression<Func<NumeroOficial, KProperty>> orderByExpression, bool ascending = true);

        [Autenticacao]
        long CountNumerosOficiais(string texto);

        [Autenticacao]
        List<NumeroOficialDTO> FindNumeroOficialPorResponsavel(int ResponsavelId);

    }
}