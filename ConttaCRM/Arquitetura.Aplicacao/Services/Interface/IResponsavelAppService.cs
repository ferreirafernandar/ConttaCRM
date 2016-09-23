using Arquitetura.Aplicacao.AOP;
using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Aggregates.ResponsavelAgg;
using Arquitetura.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Arquitetura.Aplicacao.Services.Interface
{
    public interface IResponsavelAppService : IDisposable
    {
        [Autenticacao]
        ResponsavelDTO AddResponsavel(ResponsavelDTO ResponsavelDTO);

        [Autenticacao]
        ResponsavelDTO UpdateResponsavel(ResponsavelDTO ResponsavelDTO);

        [Autenticacao]
        void RemoveResponsavel(int ResponsavelId);

        [Autenticacao]
        ResponsavelDTO FindResponsavel(int ResponsavelId);

        [Autenticacao]
        List<ResponsavelListDTO> FindResponsaveis<KProperty>(string texto, Expression<Func<Responsavel, KProperty>> orderByExpression, bool ascending, int pageIndex, int pageCount);

        [Autenticacao]
        List<ResponsavelListDTO> FindResponsaveis<KProperty>(string texto, Expression<Func<Responsavel, KProperty>> orderByExpression, bool ascending = true);

        [Autenticacao]
        long CountResponsaveis(string texto);
    }
}
