using Arquitetura.Aplicacao.Base;
using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.Dominio.Aggregates.UsoSoloAgg;
using Arquitetura.DTO;
using Arquitetura.Infraestrutura.Adapter;
using Arquitetura.Infraestrutura.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Arquitetura.Aplicacao.Services
{
    public class UsoSoloAppService : IUsoSoloAppService
    {
        #region Membros

        private readonly IUsoSoloRepository _usoSoloRepository;

        #endregion Membros

        #region Construtor

        public UsoSoloAppService(
            IUsoSoloRepository usoSoloRepository)
        {
            if (usoSoloRepository == null)
                throw new ArgumentNullException("usoSoloRepository");

            _usoSoloRepository = usoSoloRepository;
        }

        #endregion Construtor

        #region Membros de IUsoSoloAppService

        public UsoSoloDTO AddUsoSolo(UsoSoloDTO usoSoloDTO)
        {
            try
            {
                if (usoSoloDTO == null)
                    throw new ArgumentNullException("UsoSoloDTO");

                var usoSolo = UsoSoloFactory.CreateUsoSolo(
                    usoSoloDTO.Iptu,
                    usoSoloDTO.ImovelRual,
                    usoSoloDTO.EnderecoRural,
                    usoSoloDTO.Complemento,
                    usoSoloDTO.Rua,
                    usoSoloDTO.Quadra,
                    usoSoloDTO.Lote,
                    usoSoloDTO.Bairro,
                    usoSoloDTO.Cnae,
                    usoSoloDTO.Escritorio,
                    usoSoloDTO.Observacoes,
                    DateTime.Now,
                    usoSoloDTO.ResponsavelId
                    );

                SalvarUsoSolo(usoSolo);

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<UsoSolo, UsoSoloDTO>(usoSolo);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public UsoSoloDTO UpdateUsoSolo(UsoSoloDTO usoSoloDTO)
        {
            try
            {
                if (usoSoloDTO == null)
                    throw new ArgumentNullException("UsoSoloDTOFactoryDTO");

                var persistido = _usoSoloRepository.Get(usoSoloDTO.Id);
                if (persistido == null)
                    throw new Exception("Uso de solo não encontrada.");

                var corrente = UsoSoloFactory.CreateUsoSolo(
                    usoSoloDTO.Iptu,
                    usoSoloDTO.ImovelRual,
                    usoSoloDTO.EnderecoRural,
                    usoSoloDTO.Complemento,
                    usoSoloDTO.Rua,
                    usoSoloDTO.Quadra,
                    usoSoloDTO.Lote,
                    usoSoloDTO.Bairro,
                    usoSoloDTO.Cnae,
                    usoSoloDTO.Escritorio,
                    usoSoloDTO.Observacoes,
                    persistido.DataCadastro,
                    persistido.ResponsavelId);

                corrente.Id = persistido.Id;

                AlterarUsoSolo(persistido, corrente);

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<UsoSolo, UsoSoloDTO>(corrente);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public void RemoveUsoSolo(int usoSoloId)
        {
            try
            {
                if (usoSoloId <= 0)
                    throw new ArgumentException("Valor inválido.", "usoSoloId");

                var usoSolo = _usoSoloRepository.Get(usoSoloId);
                if (usoSolo == null)
                    throw new Exception("Número oficial não encontrado.");

                _usoSoloRepository.Remove(usoSolo);
                _usoSoloRepository.Commit();
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public UsoSoloDTO FindUsoSolo(int usoSoloId)
        {
            try
            {
                if (usoSoloId <= 0)
                    throw new Exception("Id da número oficial inválido.");

                var usoSolo = _usoSoloRepository.Get(usoSoloId);
                if (usoSolo == null)
                    throw new Exception("Uso de solo não encontrado.");

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<UsoSolo, UsoSoloDTO>(usoSolo);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public UsoSoloDTO FindUsoSolo(string requerente)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(requerente))
                    throw new AppException("Informe o nome do requerente.");

                var spec = UsoSoloSpecification.ConsultaTexto(requerente);
                var usoSolo = _usoSoloRepository.AllMatching(spec).SingleOrDefault();
                if (usoSolo == null)
                    throw new AppException("Uso de solo não encontrada.");

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<UsoSolo, UsoSoloDTO>(usoSolo);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public List<UsoSoloListDTO> FindUsoSolos<KProperty>(string texto, Expression<Func<UsoSolo, KProperty>> orderByExpression, bool ascending = true)
        {
            try
            {
                var spec = UsoSoloSpecification.ConsultaTexto(texto);
                List<UsoSolo> usoSolos = _usoSoloRepository.AllMatching<KProperty>(spec, orderByExpression, ascending).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<UsoSolo>, List<UsoSoloListDTO>>(usoSolos);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public List<UsoSoloListDTO> FindUsoSolos<KProperty>(string texto, Expression<Func<UsoSolo, KProperty>> orderByExpression, bool ascending, int pageIndex, int pageCount)
        {
            try
            {
                var spec = UsoSoloSpecification.ConsultaTexto(texto);
                List<UsoSolo> usoSolos = _usoSoloRepository.GetPaged<KProperty>(pageIndex, pageCount, spec, orderByExpression, ascending).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<UsoSolo>, List<UsoSoloListDTO>>(usoSolos);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public long CountUsoSolos(string texto)
        {
            try
            {
                var spec = UsoSoloSpecification.ConsultaTexto(texto);
                return _usoSoloRepository.Count(spec);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        #endregion Membros de IUsoSoloAppService

        #region Métodos Privados

        private void SalvarUsoSolo(UsoSolo usoSolo)
        {
            var validator = EntityValidatorFactory.CreateValidator();
            if (!validator.IsValid(usoSolo))
                throw new AppException(validator.GetInvalidMessages<UsoSolo>(usoSolo));

            _usoSoloRepository.Add(usoSolo);
            _usoSoloRepository.Commit();
        }

        private void AlterarUsoSolo(UsoSolo persistido, UsoSolo corrente)
        {
            var validator = EntityValidatorFactory.CreateValidator();
            if (!validator.IsValid(corrente))
                throw new AppException(validator.GetInvalidMessages<UsoSolo>(corrente));

            _usoSoloRepository.Merge(persistido, corrente);
            _usoSoloRepository.Commit();
        }

        #endregion Métodos Privados

        #region IDisposable

        public void Dispose()
        {
            _usoSoloRepository.Dispose();
        }

        #endregion IDisposable

        public List<UsoSoloDTO> FindUsoSoloPorResponsavel(int responsavelId)
        {
            try
            {
                if (responsavelId <= 0)
                    throw new Exception("Id do número oficial inválido.");

                var usoSolo = _usoSoloRepository.GetFiltered(c => c.ResponsavelId == responsavelId).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<UsoSolo>, List<UsoSoloDTO>>(usoSolo);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }
    }
}