using Arquitetura.Aplicacao.Base;
using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.Dominio.Aggregates.ResponsavelAgg;
using Arquitetura.Dominio.Repositories.Interfaces;
using Arquitetura.DTO;
using Arquitetura.Infraestrutura.Adapter;
using Arquitetura.Infraestrutura.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Arquitetura.Aplicacao.Services
{
    public class ResponsavelAppService : IResponsavelAppService
    {
        #region Membros

        private readonly IResponsavelRepository _responsavelRespository;

        #endregion Membros

        #region Construtor

        public ResponsavelAppService(
            IResponsavelRepository ResponsavelRepository,
            IConfiguracaoServidorEmailRepository configuracaoRepository)
        {
            if (ResponsavelRepository == null)
                throw new ArgumentNullException("ResponsavelRepository");

            _responsavelRepository = ResponsavelRepository;
        }

        #endregion Construtor

        #region Membros de IResponsavelAppService

        public ResponsavelDTO AddResponsavel(ResponsavelDTO responsavelDTO)
        {
            try
            {
                if (responsavelDTO == null)
                    throw new ArgumentNullException("ResponsavelDTO");

                if (responsavelDTO.Cpf != null)
                    responsavelDTO.Cpf = responsavelDTO.Cpf.Replace("-", "").Replace(".", "").Replace("_", "").Trim();
                if (responsavelDTO.Telefone != null)
                    responsavelDTO.Telefone = responsavelDTO.Telefone.Replace("_", "").Replace("-", "").Trim();
                if (responsavelDTO.Celular != null)
                    responsavelDTO.Celular = responsavelDTO.Celular.Replace("_", "").Replace("-", "").Trim();

                var Responsavel = ResponsavelFactory.CreateResponsavel(
                     responsavelDTO.Nome,
                     responsavelDTO.Cpf,
                     responsavelDTO.Telefone,
                     responsavelDTO.Celular,
                     responsavelDTO.Email,
                     responsavelDTO.Sexo,
                     DateTime.Now,
                     responsavelDTO.TipoAbertura,
                     responsavelDTO.EnviarEmail,
                     responsavelDTO.Rg,
                     responsavelDTO.EstadoCivil
                    );

                SalvarResponsavel(Responsavel);

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Responsavel, ResponsavelDTO>(Responsavel);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public ResponsavelDTO UpdateResponsavel(ResponsavelDTO responsavelDTO)
        {
            try
            {
                if (responsavelDTO == null)
                    throw new ArgumentNullException("ResponsavelDTO");

                if (responsavelDTO.Cpf != null)
                    responsavelDTO.Cpf = responsavelDTO.Cpf.Replace("-", "").Replace(".", "").Replace("_", "").Trim();
                if (responsavelDTO.Telefone != null)
                    responsavelDTO.Telefone = responsavelDTO.Telefone.Replace("_", "").Replace("-", "").Trim();
                if (responsavelDTO.Celular != null)
                    responsavelDTO.Celular = responsavelDTO.Celular.Replace("_", "").Replace("-", "").Trim();

                var persistido = _responsavelRepository.Get(responsavelDTO.Id);
                if (persistido == null)
                    throw new Exception("Responsável não encontrado.");

                var corrente = ResponsavelFactory.CreateResponsavel(
                     responsavelDTO.Nome,
                     responsavelDTO.Cpf,
                     responsavelDTO.Telefone,
                     responsavelDTO.Celular,
                     responsavelDTO.Email,
                     responsavelDTO.Sexo,
                     persistido.DataCadastro,
                     responsavelDTO.TipoAbertura,
                     responsavelDTO.EnviarEmail,
                     responsavelDTO.Rg,
                     responsavelDTO.EstadoCivil
                    );

                corrente.Id = persistido.Id;

                AlterarResponsavel(persistido, corrente);

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Responsavel, ResponsavelDTO>(corrente);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public void RemoveResponsavel(int responsavelId)
        {
            try
            {
                if (responsavelId <= 0)
                    throw new ArgumentException("Valor inválido.", "responsavelId");

                var Responsavel = _responsavelRepository.Get(responsavelId);
                if (Responsavel == null)
                    throw new Exception("Responsável não encontrado.");

                _responsavelRepository.Remove(Responsavel);
                _responsavelRepository.Commit();
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public ResponsavelDTO FindResponsavel(int responsavelId)
        {
            try
            {
                if (responsavelId <= 0)
                    throw new Exception("Id do responsável inválido.");

                var Responsavel = _responsavelRepository.Get(responsavelId);
                if (Responsavel == null)
                    throw new Exception("Responsável não encontrado.");

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Responsavel, ResponsavelDTO>(Responsavel);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public ResponsavelDTO FindResponsavel(string nomeResponsavel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomeResponsavel))
                    throw new AppException("Informe o nome de responsavel.");

                var spec = ResponsavelSpecification.ConsultaTexto(nomeResponsavel);
                var Responsavel = _responsavelRepository.AllMatching(spec).SingleOrDefault();
                if (Responsavel == null)
                    throw new AppException("Responsável não encontrado.");

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Responsavel, ResponsavelDTO>(Responsavel);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public List<ResponsavelListDTO> FindResponsaveis<KProperty>(string texto, Expression<Func<Responsavel, KProperty>> orderByExpression, bool ascending = true)
        {
            try
            {
                var spec = ResponsavelSpecification.ConsultaTexto(texto);
                List<Responsavel> Responsaveis = _responsavelRepository.AllMatching<KProperty>(spec, orderByExpression, ascending).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<Responsavel>, List<ResponsavelListDTO>>(Responsaveis);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public List<ResponsavelListDTO> FindResponsaveis<KProperty>(string texto, Expression<Func<Responsavel, KProperty>> orderByExpression, bool ascending, int pageIndex, int pageCount)
        {
            try
            {
                var spec = ResponsavelSpecification.ConsultaTexto(texto);
                List<Responsavel> Responsaveis = _responsavelRepository.GetPaged<KProperty>(pageIndex, pageCount, spec, orderByExpression, ascending).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<Responsavel>, List<ResponsavelListDTO>>(Responsaveis);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public long CountResponsaveis(string texto)
        {
            try
            {
                var spec = ResponsavelSpecification.ConsultaTexto(texto);
                return _responsavelRepository.Count(spec);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        #endregion Membros de IResponsavelAppService

        #region Métodos Privados

        private void SalvarResponsavel(Responsavel responsavel)
        {
            var validator = EntityValidatorFactory.CreateValidator();
            if (!validator.IsValid(responsavel))
                throw new AppException(validator.GetInvalidMessages<Responsavel>(responsavel));

            var specExisteResponsavel = ResponsavelSpecification.ConsultaTexto(responsavel.Nome);
            if (_responsavelRepository.AllMatching(specExisteResponsavel).Any())
                throw new AppException("Já existe um responsável cadastrado com este nome.");

            _responsavelRepository.Add(responsavel);
            _responsavelRepository.Commit();
        }

        private void AlterarResponsavel(Responsavel persistido, Responsavel corrente)
        {
            var validator = EntityValidatorFactory.CreateValidator();
            if (!validator.IsValid(corrente))
                throw new AppException(validator.GetInvalidMessages<Responsavel>(corrente));

            var specExisteResponsavel = ResponsavelSpecification.ConsultaTexto(corrente.Nome);
            if (_responsavelRepository.AllMatching(specExisteResponsavel).Where(c => c.Id != persistido.Id).Any())
                throw new AppException("Já existe um responsável cadastrado com este nome.");

            _responsavelRepository.Merge(persistido, corrente);
            _responsavelRepository.Commit();
        }

        #endregion Métodos Privados

        #region IDisposable

        public void Dispose()
        {
            _responsavelRepository.Dispose();
        }

        #endregion IDisposable

        public IResponsavelRepository _responsavelRepository { get; set; }
    }
}