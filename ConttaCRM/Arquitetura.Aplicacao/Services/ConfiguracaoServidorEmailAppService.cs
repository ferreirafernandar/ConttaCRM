using Arquitetura.Aplicacao.Base;
using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.Dominio.Aggregates.ConfiguracaoAgg;
using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.ControladorDeSessao;
using Arquitetura.Dominio.Repositories.Interfaces;
using Arquitetura.DTO;
using Arquitetura.Infraestrutura.Adapter;
using Arquitetura.Infraestrutura.Logging;
using Arquitetura.Infraestrutura.Validator;
using System;
using System.IO;
using System.Linq;

namespace Arquitetura.Aplicacao.Services
{
    public class ConfiguracaoAppService : IConfiguracaoAppService
    {
        #region Membros

        readonly IConfiguracaoServidorEmailRepository _configuracaoServidorEmailRepository;

        #endregion

        #region Construtor

        public ConfiguracaoAppService(IConfiguracaoServidorEmailRepository configuracaoServidorEmailRepository)
        {
            if (configuracaoServidorEmailRepository == null)
                throw new ArgumentNullException("configuracaoServidorEmailRepository");

            _configuracaoServidorEmailRepository = configuracaoServidorEmailRepository;
        }

        #endregion

        #region Membros de IUsuarioAppService

        public void UpdateConfiguracaoServidorEmail(ConfiguracaoServidorEmailDTO configuracaoServidorEmailDTO)
        {
            try
            {
                if (configuracaoServidorEmailDTO == null)
                    throw new ArgumentNullException("configuracaoServidorEmailDTO.");

                ConfiguracaoServidorEmail persistido;

                var lista = _configuracaoServidorEmailRepository.GetAll().ToList();

                if (lista.Count == 0)
                {
                    GetConfiguracaoServidorEmail();
                    persistido = _configuracaoServidorEmailRepository.GetAll().Single();
                }
                else if (lista.Count == 1)
                {
                    persistido = lista.Single();
                }
                else
                {
                    throw new Exception("Existe mais de uma ConfiguracaoServidorEmail quando deveria existir só uma.");
                }

                string senha = persistido.Senha;
                if (!string.IsNullOrWhiteSpace(configuracaoServidorEmailDTO.Senha))
                {
                    senha = configuracaoServidorEmailDTO.Senha;
                }

                var corrente = ConfiguracaoServidorEmailFactory.CreateConfiguracaoServidorEmail(
                    configuracaoServidorEmailDTO.UtilizarEnvioDeEmail, 
                    configuracaoServidorEmailDTO.Conta,
                    senha, 
                    configuracaoServidorEmailDTO.Smtp, 
                    configuracaoServidorEmailDTO.Porta, 
                    configuracaoServidorEmailDTO.Ssl,
                    configuracaoServidorEmailDTO.PastaRaiz);

                corrente.Id = persistido.Id;
                AlterarConfiguracaoServidorEmail(persistido, corrente);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public ConfiguracaoServidorEmailDTO GetConfiguracaoServidorEmail()
        {
            try
            {
                ConfiguracaoServidorEmail configuracaoServidorEmail;

                var lista = _configuracaoServidorEmailRepository.GetAll().ToList();

                if (lista.Count == 0)
                {
                    configuracaoServidorEmail = ConfiguracaoServidorEmailFactory.CreateConfiguracaoServidorEmail(false, null, null, null, null, false, null);
                    SalvarConfiguracaoServidorEmail(configuracaoServidorEmail);
                }
                else if (lista.Count == 1)
                {
                    configuracaoServidorEmail = lista.Single();
                }
                else
                {
                    throw new Exception("Existe mais de uma ConfiguracaoServidorEmail quando deveria existir só uma.");
                }

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<ConfiguracaoServidorEmail, ConfiguracaoServidorEmailDTO>(configuracaoServidorEmail);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        #endregion

        #region Métodos Privados

        void SalvarConfiguracaoServidorEmail(ConfiguracaoServidorEmail configuracaoServidorEmail)
        {
            var validator = EntityValidatorFactory.CreateValidator();
            if (!validator.IsValid(configuracaoServidorEmail))
                throw new AppException(validator.GetInvalidMessages(configuracaoServidorEmail));

            _configuracaoServidorEmailRepository.Add(configuracaoServidorEmail);
            _configuracaoServidorEmailRepository.Commit();
        }

        void AlterarConfiguracaoServidorEmail(ConfiguracaoServidorEmail persistido, ConfiguracaoServidorEmail corrente)
        {
            var validator = EntityValidatorFactory.CreateValidator();
            if (!validator.IsValid(corrente))
                throw new AppException(validator.GetInvalidMessages(corrente));

            if (!Directory.Exists(corrente.PastaRaiz))
            {
                throw new AppException("A pasta raiz informada não existe.");
            }

            _configuracaoServidorEmailRepository.Merge(persistido, corrente);
            _configuracaoServidorEmailRepository.Commit();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _configuracaoServidorEmailRepository.Dispose();
        }

        #endregion
    }
}
