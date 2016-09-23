using System.Linq;
using Arquitetura.Aplicacao.Base;
using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.Dominio.Repositories.Interfaces;
using Arquitetura.DTO;
using Arquitetura.Infraestrutura.Adapter;
using Arquitetura.Infraestrutura.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Arquitetura.Infraestrutura.Util;
using Arquitetura.Infraestrutura.Validator;
using System.Threading.Tasks;
using Arquitetura.Dominio.Aggregates.ConfiguracaoAgg;

namespace Arquitetura.Aplicacao.Services
{
    public class EmailAppService : IEmailAppService
    {
        #region Membros

        readonly IConfiguracaoAppService _configuracaoAppService;

        #endregion

        #region Construtor

        public EmailAppService(IConfiguracaoAppService configuracaoAppService)
        {
            if (configuracaoAppService == null)
                throw new ArgumentNullException("configuracaoAppService");

            _configuracaoAppService = configuracaoAppService;
        }

        #endregion

        #region Membros de IEmailAppService

        public void EnviarEmailAssync(List<EmailDTO> listaDeEmailDTO)
        {
            try
            {
                var configuracaoServidorEmailDTO = _configuracaoAppService.GetConfiguracaoServidorEmail();
                if (configuracaoServidorEmailDTO.UtilizarEnvioDeEmail)
                {
                    List<Task> tasks = new List<Task>();
                    foreach (var item in listaDeEmailDTO)
                    {
                        item.ConfiguracaoServidorEmailDTO = configuracaoServidorEmailDTO;
                        tasks.Add(Task.Factory.StartNew(AssyncEmailAction, item));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError(ex);
            }
        }

        #endregion

        #region Métodos Privados

        void AssyncEmailAction(object state)
        {
            try
            {
                EmailDTO emailDTO = state as EmailDTO;

                bool emailEnviado;
                string mensagemErro = null;

                try
                {
                    Util.SendEmail(
                        emailDTO.Assunto,
                        emailDTO.Mensagem,
                        emailDTO.ConfiguracaoServidorEmailDTO.Conta,
                        emailDTO.ConfiguracaoServidorEmailDTO.Senha,
                        emailDTO.ConfiguracaoServidorEmailDTO.Smtp,
                        emailDTO.ConfiguracaoServidorEmailDTO.Porta.Value,
                        emailDTO.ConfiguracaoServidorEmailDTO.Ssl,
                        emailDTO.Para);

                    emailEnviado = true;
                }
                catch (Exception ex)
                {
                    mensagemErro = ex.Message;
                    emailEnviado = false;
                }

                // TODO: Salvar registro de email
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError(ex);
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _configuracaoAppService.Dispose();
        }

        #endregion
    }
}
