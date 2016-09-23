using Arquitetura.Aplicacao.Base;
using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.Dominio.Aggregates.EntrevistaAgg;
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
    public class EntrevistaAppService : IEntrevistaAppService
    {
        #region Membros

        private readonly IEntrevistaRepository _entrevistaRepository;

        #endregion Membros

        #region Construtor

        public EntrevistaAppService(
            IEntrevistaRepository entrevistaRepository,
            IConfiguracaoServidorEmailRepository configuracaoRepository)
        {
            if (entrevistaRepository == null)
                throw new ArgumentNullException("entrevistaRepository");

            _entrevistaRepository = entrevistaRepository;
        }

        #endregion Construtor

        #region Membros de IEntrevistaAppService

        public EntrevistaDTO AddEntrevista(EntrevistaDTO entrevistaDTO)
        {
            try
            {
                if (entrevistaDTO == null)
                    throw new ArgumentNullException("EntrevistaDTO");
                if (entrevistaDTO.Telefone != null)
                    entrevistaDTO.Telefone.Replace("_", "").Replace("-", "").Trim();

                var Entrevista = EntrevistaFactory.CreateEntrevista(
                    entrevistaDTO.NomeDaEmpresa1,
                    entrevistaDTO.NomeDaEmpresa2,
                    entrevistaDTO.NomeDaEmpresa3,
                    entrevistaDTO.Iptu,
                    entrevistaDTO.NomeFantasia,
                    entrevistaDTO.CapitalSocial,
                    entrevistaDTO.Objetivo,
                    entrevistaDTO.Metragem,
                    entrevistaDTO.PontoDeReferencia,
                    entrevistaDTO.LivroRegistroEmpregados,
                    entrevistaDTO.InspencaoTrabalho,
                    entrevistaDTO.LivroTermoOcorrencia,
                    entrevistaDTO.Telefone,
                    entrevistaDTO.Email,
                    entrevistaDTO.ClienteId,
                    entrevistaDTO.UsuarioId,
                    entrevistaDTO.ResponsalvelId,
                    DateTime.Now,
                    entrevistaDTO.CopiaRg,
                    entrevistaDTO.CopiaCpf,
                    entrevistaDTO.CopiaEndereco,
                    entrevistaDTO.CopiaCnh,
                    entrevistaDTO.CopiaCasamento
                    );

                SalvarEntrevista(Entrevista);

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Entrevista, EntrevistaDTO>(Entrevista);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public EntrevistaDTO UpdateEntrevista(EntrevistaDTO entrevistaDTO)
        {
            try
            {
                if (entrevistaDTO == null)
                    throw new ArgumentNullException("entrevistaFactoryDTO");

                var persistido = _entrevistaRepository.Get(entrevistaDTO.Id);
                if (persistido == null)
                    throw new Exception("Entrevista não encontrada.");

                var corrente = EntrevistaFactory.CreateEntrevista(
                    entrevistaDTO.NomeDaEmpresa1,
                    entrevistaDTO.NomeDaEmpresa2,
                    entrevistaDTO.NomeDaEmpresa3,
                    entrevistaDTO.Iptu,
                    entrevistaDTO.NomeFantasia,
                    entrevistaDTO.CapitalSocial,
                    entrevistaDTO.Objetivo,
                    entrevistaDTO.Metragem,
                    entrevistaDTO.PontoDeReferencia,
                    entrevistaDTO.LivroRegistroEmpregados,
                    entrevistaDTO.InspencaoTrabalho,
                    entrevistaDTO.LivroTermoOcorrencia,
                    entrevistaDTO.Telefone,
                    entrevistaDTO.Email,
                    entrevistaDTO.ClienteId,
                    entrevistaDTO.UsuarioId,
                    persistido.ResponsavelId,
                    persistido.DataCadastro,
                    entrevistaDTO.CopiaRg,
                    entrevistaDTO.CopiaCpf,
                    entrevistaDTO.CopiaEndereco,
                    entrevistaDTO.CopiaCnh,
                    entrevistaDTO.CopiaCasamento);

                corrente.Id = persistido.Id;

                AlterarEntrevista(persistido, corrente);

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Entrevista, EntrevistaDTO>(corrente);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public void RemoveEntrevista(int entrevistaId)
        {
            try
            {
                if (entrevistaId <= 0)
                    throw new ArgumentException("Valor inválido.", "entrevistaId");

                var Entrevista = _entrevistaRepository.Get(entrevistaId);
                if (Entrevista == null)
                    throw new Exception("Entrevista não encontrada.");

                _entrevistaRepository.Remove(Entrevista);
                _entrevistaRepository.Commit();
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public EntrevistaDTO FindEntrevista(int entrevistaId)
        {
            try
            {
                if (entrevistaId <= 0)
                    throw new Exception("Id da entrevista inválido.");

                var Entrevista = _entrevistaRepository.Get(entrevistaId);
                if (Entrevista == null)
                    throw new Exception("Entrevista não encontrado.");

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Entrevista, EntrevistaDTO>(Entrevista);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public EntrevistaDTO FindEntrevista(string nomeDaEmpresa1)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomeDaEmpresa1))
                    throw new AppException("Informe o nome do primeiro sócio.");

                var spec = EntrevistaSpecification.ConsultaTexto(nomeDaEmpresa1);
                var Entrevista = _entrevistaRepository.AllMatching(spec).SingleOrDefault();
                if (Entrevista == null)
                    throw new AppException("Entrevista não encontrada.");

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Entrevista, EntrevistaDTO>(Entrevista);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public List<EntrevistaListDTO> FindEntrevistas<KProperty>(string texto, Expression<Func<Entrevista, KProperty>> orderByExpression, bool ascending = true)
        {
            try
            {
                var spec = EntrevistaSpecification.ConsultaTexto(texto);
                List<Entrevista> Entrevistas = _entrevistaRepository.AllMatching<KProperty>(spec, orderByExpression, ascending).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<Entrevista>, List<EntrevistaListDTO>>(Entrevistas);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public List<EntrevistaListDTO> FindEntrevistas<KProperty>(string texto, Expression<Func<Entrevista, KProperty>> orderByExpression, bool ascending, int pageIndex, int pageCount)
        {
            try
            {
                var spec = EntrevistaSpecification.ConsultaTexto(texto);
                List<Entrevista> Entrevistas = _entrevistaRepository.GetPaged<KProperty>(pageIndex, pageCount, spec, orderByExpression, ascending).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<Entrevista>, List<EntrevistaListDTO>>(Entrevistas);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public long CountEntrevistas(string texto)
        {
            try
            {
                var spec = EntrevistaSpecification.ConsultaTexto(texto);
                return _entrevistaRepository.Count(spec);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public EntrevistaDTO FindEntrevistaNull(string login)
        {
            try
            {
                if (string.IsNullOrEmpty(login))
                    return null;

                var Entrevista = _entrevistaRepository.GetFiltered(c => c.NomeDaEmpresa1 == login).SingleOrDefault();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Entrevista, EntrevistaDTO>(Entrevista);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        #endregion Membros de IEntrevistaAppService

        #region Métodos Privados

        private void SalvarEntrevista(Entrevista entrevista)
        {
            var validator = EntityValidatorFactory.CreateValidator();
            if (!validator.IsValid(entrevista))
                throw new AppException(validator.GetInvalidMessages<Entrevista>(entrevista));

            var specExisteEntrevista = EntrevistaSpecification.ConsultaTexto(entrevista.NomeDaEmpresa1);
            if (_entrevistaRepository.AllMatching(specExisteEntrevista).Any())
                throw new AppException("Já existe uma entrevista cadastrada com este nome.");

            _entrevistaRepository.Add(entrevista);
            _entrevistaRepository.Commit();
        }

        private void AlterarEntrevista(Entrevista persistido, Entrevista corrente)
        {
            var validator = EntityValidatorFactory.CreateValidator();
            if (!validator.IsValid(corrente))
                throw new AppException(validator.GetInvalidMessages<Entrevista>(corrente));

            var specExisteEntrevista = EntrevistaSpecification.ConsultaTexto(corrente.NomeDaEmpresa1);
            if (_entrevistaRepository.AllMatching(specExisteEntrevista).Where(c => c.Id != persistido.Id).Any())
                throw new AppException("Já existe uma entrevista cadastrada com este nome.");

            _entrevistaRepository.Merge(persistido, corrente);
            _entrevistaRepository.Commit();
        }

        #endregion Métodos Privados

        #region IDisposable

        public void Dispose()
        {
            _entrevistaRepository.Dispose();
        }

        #endregion IDisposable

        public EntrevistaDTO UpdatePerfilEntrevista(EntrevistaDTO entrevistaDTO)
        {
            throw new NotImplementedException();
        }

        public List<EntrevistaDTO> FindEntrevistaPorResponsavel(int responsavelId)
        {
            try
            {
                if (responsavelId <= 0)
                    throw new Exception("Id do responsável inválido.");

                var entrevista = _entrevistaRepository.GetFiltered(c => c.ResponsavelId == responsavelId).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<Entrevista>, List<EntrevistaDTO>>(entrevista);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }
    }
}