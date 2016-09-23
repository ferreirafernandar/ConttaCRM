using Arquitetura.Aplicacao.AOP;
using Arquitetura.Dominio.Aggregates.ConfiguracaoAgg;
using Arquitetura.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Arquitetura.Aplicacao.Services.Interface
{
    public interface IConfiguracaoAppService : IDisposable
    {
        [Autenticacao]
        void UpdateConfiguracaoServidorEmail(ConfiguracaoServidorEmailDTO configuracaoServidorEmailDTO);

        ConfiguracaoServidorEmailDTO GetConfiguracaoServidorEmail();
    }
}
