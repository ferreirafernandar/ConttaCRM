using Arquitetura.Aplicacao.Base;
using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.Dominio.Aggregates.NumeroOficialAgg;
using Arquitetura.DTO;
using Arquitetura.Infraestrutura.Adapter;
using Arquitetura.Infraestrutura.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Arquitetura.Aplicacao.Services
{
    public class NumeroOficialAppService : INumeroOficialAppService
    {
        #region Membros

        private readonly INumeroOficialRepository _numeroOficialRepository;

        #endregion Membros

        #region Construtor

        public NumeroOficialAppService(
            INumeroOficialRepository numeroOficialRepository)
        {
            if (numeroOficialRepository == null)
                throw new ArgumentNullException("numeroOficialRepository");

            _numeroOficialRepository = numeroOficialRepository;
        }

        #endregion Construtor

        #region Membros de INumeroOficialAppService

        public NumeroOficialDTO AddNumeroOficial(NumeroOficialDTO numeroOficialDTO)
        {
            try
            {
                if (numeroOficialDTO == null)
                    throw new ArgumentNullException("NumeroOficialDTO");

                if (numeroOficialDTO.Telefone != null)
                    numeroOficialDTO.Telefone.Replace("_", "").Replace("-", "").Trim();

                var numeroOficial = NumeroOficialFactory.CreateNumeroOficial(
                    numeroOficialDTO.Requerente,
                    numeroOficialDTO.Rg,
                    numeroOficialDTO.PossuiIptu,
                    numeroOficialDTO.Iptu,
                    numeroOficialDTO.Rua,
                    numeroOficialDTO.ExisteEdificacao,
                    numeroOficialDTO.Atividade,
                    numeroOficialDTO.Telefone,
                    numeroOficialDTO.SituacaoLocal,
                    numeroOficialDTO.GerarNumeroOficial,
                    numeroOficialDTO.NumeroOficialB,
                    numeroOficialDTO.NumeroOficialC,
                    numeroOficialDTO.Observacoes,
                    DateTime.Now,
                    numeroOficialDTO.ResponsavelId
                    );

                SalvarNumeroOficial(numeroOficial);

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<NumeroOficial, NumeroOficialDTO>(numeroOficial);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public NumeroOficialDTO UpdateNumeroOficial(NumeroOficialDTO numeroOficialDTO)
        {
            try
            {
                if (numeroOficialDTO == null)
                    throw new ArgumentNullException("NumeroOficialFactoryDTO");

                var persistido = _numeroOficialRepository.Get(numeroOficialDTO.Id);
                if (persistido == null)
                    throw new Exception("NumeroOficial não encontrada.");

                var corrente = NumeroOficialFactory.CreateNumeroOficial(
                    numeroOficialDTO.Requerente,
                    numeroOficialDTO.Rg,
                    numeroOficialDTO.PossuiIptu,
                    numeroOficialDTO.Iptu,
                    numeroOficialDTO.Rua,
                    numeroOficialDTO.ExisteEdificacao,
                    numeroOficialDTO.Atividade,
                    numeroOficialDTO.Telefone,
                    numeroOficialDTO.SituacaoLocal,
                    numeroOficialDTO.GerarNumeroOficial,
                    numeroOficialDTO.NumeroOficialB,
                    numeroOficialDTO.NumeroOficialC,
                    numeroOficialDTO.Observacoes,
                    persistido.DataCadastro,
                    persistido.ResponsavelId);

                corrente.Id = persistido.Id;

                AlterarNumeroOficial(persistido, corrente);

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<NumeroOficial, NumeroOficialDTO>(corrente);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public void RemoveNumeroOficial(int numeroOficialId)
        {
            try
            {
                if (numeroOficialId <= 0)
                    throw new ArgumentException("Valor inválido.", "NumeroOficialId");

                var numeroOficial = _numeroOficialRepository.Get(numeroOficialId);
                if (numeroOficial == null)
                    throw new Exception("Número oficial não encontrado.");

                _numeroOficialRepository.Remove(numeroOficial);
                _numeroOficialRepository.Commit();
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public NumeroOficialDTO FindNumeroOficial(int numeroOficialId)
        {
            try
            {
                if (numeroOficialId <= 0)
                    throw new Exception("Id da número oficial inválido.");

                var numeroOficial = _numeroOficialRepository.Get(numeroOficialId);
                if (numeroOficial == null)
                    throw new Exception("Número oficial não encontrado.");

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<NumeroOficial, NumeroOficialDTO>(numeroOficial);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public NumeroOficialDTO FindNumeroOficial(string requerente)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(requerente))
                    throw new AppException("Informe o nome do requerente.");

                var spec = NumeroOficialSpecification.ConsultaTexto(requerente);
                var numeroOficial = _numeroOficialRepository.AllMatching(spec).SingleOrDefault();
                if (numeroOficial == null)
                    throw new AppException("NumeroOficial não encontrada.");

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<NumeroOficial, NumeroOficialDTO>(numeroOficial);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public List<NumeroOficialListDTO> FindNumerosOficiais<KProperty>(string texto, Expression<Func<NumeroOficial, KProperty>> orderByExpression, bool ascending = true)
        {
            try
            {
                var spec = NumeroOficialSpecification.ConsultaTexto(texto);
                List<NumeroOficial> numerosOficiais = _numeroOficialRepository.AllMatching<KProperty>(spec, orderByExpression, ascending).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<NumeroOficial>, List<NumeroOficialListDTO>>(numerosOficiais);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public List<NumeroOficialListDTO> FindNumerosOficiais<KProperty>(string texto, Expression<Func<NumeroOficial, KProperty>> orderByExpression, bool ascending, int pageIndex, int pageCount)
        {
            try
            {
                var spec = NumeroOficialSpecification.ConsultaTexto(texto);
                List<NumeroOficial> numerosOficiais = _numeroOficialRepository.GetPaged<KProperty>(pageIndex, pageCount, spec, orderByExpression, ascending).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<NumeroOficial>, List<NumeroOficialListDTO>>(numerosOficiais);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public long CountNumerosOficiais(string texto)
        {
            try
            {
                var spec = NumeroOficialSpecification.ConsultaTexto(texto);
                return _numeroOficialRepository.Count(spec);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public NumeroOficialDTO FindNumeroOficialNull(string login)
        {
            try
            {
                if (string.IsNullOrEmpty(login))
                    return null;

                var NumeroOficial = _numeroOficialRepository.GetFiltered(c => c.Requerente == login).SingleOrDefault();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<NumeroOficial, NumeroOficialDTO>(NumeroOficial);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        #endregion Membros de INumeroOficialAppService

        #region Métodos Privados

        private void SalvarNumeroOficial(NumeroOficial numeroOficial)
        {
            var validator = EntityValidatorFactory.CreateValidator();
            if (!validator.IsValid(numeroOficial))
                throw new AppException(validator.GetInvalidMessages<NumeroOficial>(numeroOficial));

            //var specExisteNumeroOficial = NumeroOficialSpecification.ConsultaTexto(numeroOficial.Requerente);
            //if (_numeroOficialRepository.AllMatching(specExisteNumeroOficial).Any())
            //    throw new AppException("Já existe um número oficial cadastrado com este nome.");

            _numeroOficialRepository.Add(numeroOficial);
            _numeroOficialRepository.Commit();
        }

        private void AlterarNumeroOficial(NumeroOficial persistido, NumeroOficial corrente)
        {
            var validator = EntityValidatorFactory.CreateValidator();
            if (!validator.IsValid(corrente))
                throw new AppException(validator.GetInvalidMessages<NumeroOficial>(corrente));

            var specExisteNumeroOficial = NumeroOficialSpecification.ConsultaTexto(corrente.Requerente);
            if (_numeroOficialRepository.AllMatching(specExisteNumeroOficial).Where(c => c.Id != persistido.Id).Any())
                throw new AppException("Já existe um número Oficial cadastrado com este nome.");

            _numeroOficialRepository.Merge(persistido, corrente);
            _numeroOficialRepository.Commit();
        }

        #endregion Métodos Privados

        #region IDisposable

        public void Dispose()
        {
            _numeroOficialRepository.Dispose();
        }

        #endregion IDisposable

        public List<NumeroOficialDTO> FindNumeroOficialPorResponsavel(int responsavelId)
        {
            try
            {
                if (responsavelId <= 0)
                    throw new Exception("Id do número oficial inválido.");

                var numeroOficial = _numeroOficialRepository.GetFiltered(c => c.ResponsavelId == responsavelId).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<NumeroOficial>, List<NumeroOficialDTO>>(numeroOficial);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }
    }
}