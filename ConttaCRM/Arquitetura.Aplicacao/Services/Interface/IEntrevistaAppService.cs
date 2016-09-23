using Arquitetura.Aplicacao.AOP;
using Arquitetura.Dominio.Aggregates.EntrevistaAgg;
using Arquitetura.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Arquitetura.Aplicacao.Services.Interface
{
    public interface IEntrevistaAppService : IDisposable
    {
        [Autenticacao]
        EntrevistaDTO AddEntrevista(EntrevistaDTO entrevistaDTO);

        [Autenticacao]
        EntrevistaDTO UpdateEntrevista(EntrevistaDTO EntrevistaDTO);

        [Autenticacao]
        EntrevistaDTO UpdatePerfilEntrevista(EntrevistaDTO EntrevistaDTO);

        [Autenticacao]
        void RemoveEntrevista(int EntrevistaId);

        [Autenticacao]
        EntrevistaDTO FindEntrevista(int EntrevistaId);

        [Autenticacao]
        EntrevistaDTO FindEntrevista(string email);

        [Autenticacao]
        List<EntrevistaListDTO> FindEntrevistas<KProperty>(string texto, Expression<Func<Entrevista, KProperty>> orderByExpression, bool ascending, int pageIndex, int pageCount);

        [Autenticacao]
        List<EntrevistaListDTO> FindEntrevistas<KProperty>(string texto, Expression<Func<Entrevista, KProperty>> orderByExpression, bool ascending = true);

        [Autenticacao]
        long CountEntrevistas(string texto);

        //Sem autenticacao
        EntrevistaDTO FindEntrevistaNull(string login);

        [Autenticacao]
        List<EntrevistaDTO> FindEntrevistaPorResponsavel(int ResponsavelId);
    }
}