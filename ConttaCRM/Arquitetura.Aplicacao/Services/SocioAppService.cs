using Arquitetura.Aplicacao.Base;
using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.Dominio.Aggregates.EntrevistaAgg;
using Arquitetura.Dominio.Aggregates.SocioAgg;
using Arquitetura.DTO;
using Arquitetura.Infraestrutura.Adapter;
using Arquitetura.Infraestrutura.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Arquitetura.Aplicacao.Services
{
    public class SocioAppService : ISocioAppService
    {
        #region Membros

        private readonly ISocioRepository _socioRepository;
        private readonly IEntrevistaRepository _entrevistaRepository;

        #endregion Membros

        #region Construtor

        public SocioAppService(
            ISocioRepository socioRepository,
            IEntrevistaRepository entrevistaRepository)
        {
            if (socioRepository == null)
                throw new ArgumentNullException("socioRepository");

            _socioRepository = socioRepository;
            _entrevistaRepository = entrevistaRepository;
        }

        #endregion Construtor

        #region Membros de ISocioAppService

        public SocioDTO AddSocio(SocioDTO socioDTO)
        {
            try
            {
                if (socioDTO == null)
                    throw new ArgumentNullException("socioDTO");

                if (socioDTO.Cpf != null)
                    socioDTO.Cpf = socioDTO.Cpf.Replace("-", "").Replace(".", "").Replace("_", "").Trim();
                if (socioDTO.Telefone != null)
                    socioDTO.Telefone = socioDTO.Telefone.Replace("_", "").Replace("-", "").Trim();
                if (socioDTO.Celular != null)
                    socioDTO.Celular = socioDTO.Celular.Replace("_", "").Replace("-", "").Trim();

                var Socio = SocioFactory.CreateSocio(
                     socioDTO.Nome,
                     socioDTO.Administrador,
                     socioDTO.DataNascimento,
                     socioDTO.Rg,
                     socioDTO.OrgaoRG,
                     socioDTO.Cpf,
                     socioDTO.NomeMae,
                     socioDTO.NomePai,
                     socioDTO.Cnh,
                     socioDTO.Nacionalidade,
                     socioDTO.Naturalidade,
                     socioDTO.Sexo,
                     socioDTO.EstadoCivil,
                     socioDTO.Telefone,
                     socioDTO.Celular,
                     socioDTO.Email,
                     socioDTO.Participacao,
                     socioDTO.Assina,
                     socioDTO.Rua,
                     socioDTO.Numero,
                     socioDTO.Complemento,
                     socioDTO.Bairro,
                     socioDTO.Cidade,
                     socioDTO.Cep,
                     socioDTO.Referencia,
                     socioDTO.Estado,
                     socioDTO.EntrevistaId

                    );

                SalvarSocio(Socio);

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Socio, SocioDTO>(Socio);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public SocioDTO UpdateSocio(SocioDTO socioDTO)
        {
            try
            {
                if (socioDTO == null)
                    throw new ArgumentNullException("SocioDTO");

                if (socioDTO.Cpf != null)
                    socioDTO.Cpf = socioDTO.Cpf.Replace("-", "").Replace(".", "").Replace("_", "").Trim();
                if (socioDTO.Telefone != null)
                    socioDTO.Telefone = socioDTO.Telefone.Replace("_", "").Replace("-", "").Trim();
                if (socioDTO.Celular != null)
                    socioDTO.Celular = socioDTO.Celular.Replace("_", "").Replace("-", "").Trim();

                var persistido = _socioRepository.Get(socioDTO.Id);
                if (persistido == null)
                    throw new Exception("Sócio não encontrado.");

                var corrente = SocioFactory.CreateSocio(
                     socioDTO.Nome,
                     socioDTO.Administrador,
                     socioDTO.DataNascimento,
                     socioDTO.Rg,
                     socioDTO.OrgaoRG,
                     socioDTO.Cpf,
                     socioDTO.NomeMae,
                     socioDTO.NomePai,
                     socioDTO.Cnh,
                     socioDTO.Nacionalidade,
                     socioDTO.Naturalidade,
                     socioDTO.Sexo,
                     socioDTO.EstadoCivil,
                     socioDTO.Telefone,
                     socioDTO.Celular,
                     socioDTO.Email,
                     socioDTO.Participacao,
                     socioDTO.Assina,
                     socioDTO.Rua,
                     socioDTO.Numero,
                     socioDTO.Complemento,
                     socioDTO.Bairro,
                     socioDTO.Cidade,
                     socioDTO.Cep,
                     socioDTO.Referencia,
                     socioDTO.Estado,
                     persistido.EntrevistaId
                    );

                corrente.Id = persistido.Id;

                AlterarSocio(persistido, corrente);

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Socio, SocioDTO>(corrente);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public void RemoveSocio(int socioId)
        {
            try
            {
                if (socioId <= 0)
                    throw new ArgumentException("Valor inválido.", "socioId");

                var Socio = _socioRepository.Get(socioId);
                if (Socio == null)
                    throw new Exception("Sócio não encontrado.");

                _socioRepository.Remove(Socio);
                _socioRepository.Commit();
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public SocioDTO FindSocio(int socioId)
        {
            try
            {
                if (socioId <= 0)
                    throw new Exception("Id do sócio inválido.");

                var Socio = _socioRepository.Get(socioId);
                if (Socio == null)
                    throw new Exception("Sócio não encontrado.");

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Socio, SocioDTO>(Socio);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public SocioDTO FindSocio(string nomeSocio)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomeSocio))
                    throw new AppException("Informe o nome de sócio.");

                var spec = SocioSpecification.ConsultaTexto(nomeSocio);
                var Socio = _socioRepository.AllMatching(spec).SingleOrDefault();
                if (Socio == null)
                    throw new AppException("Sócio não encontrado.");

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Socio, SocioDTO>(Socio);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public List<SocioListDTO> FindSocios<KProperty>(string texto, Expression<Func<Socio, KProperty>> orderByExpression, bool ascending = true)
        {
            try
            {
                var spec = SocioSpecification.ConsultaTexto(texto);
                List<Socio> Socios = _socioRepository.AllMatching<KProperty>(spec, orderByExpression, ascending).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<Socio>, List<SocioListDTO>>(Socios);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public List<SocioListDTO> FindSocios<KProperty>(string texto, Expression<Func<Socio, KProperty>> orderByExpression, bool ascending, int pageIndex, int pageCount)
        {
            try
            {
                var spec = SocioSpecification.ConsultaTexto(texto);
                List<Socio> Socios = _socioRepository.GetPaged<KProperty>(pageIndex, pageCount, spec, orderByExpression, ascending).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<Socio>, List<SocioListDTO>>(Socios);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public long CountSocios(string texto)
        {
            try
            {
                var spec = SocioSpecification.ConsultaTexto(texto);
                return _socioRepository.Count(spec);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        #endregion Membros de ISocioAppService

        #region Métodos Privados

        private void SalvarSocio(Socio socio)
        {
            var validator = EntityValidatorFactory.CreateValidator();

            if (!validator.IsValid(socio))
                throw new AppException(validator.GetInvalidMessages<Socio>(socio));

            var specExisteSocio = SocioSpecification.ConsultaTexto(socio.Nome);

            if (_socioRepository.AllMatching(specExisteSocio).Any())
                throw new AppException("Já existe um sócio cadastrado com este nome.");

          
            if (JaExisteCpf(socio.Cpf, socio.EntrevistaId, socio.Id))
            {
                throw new AppException("Já existe um sócio cadastrado com este CPF.");
            }

            if (socio.Administrador && socio.Assina)
            {
                bool x = JaExisteAdministrador(socio.EntrevistaId);
                bool y = JaExisteAssinaNaReceita(socio.EntrevistaId);

                if (!x && y)
                {
                    throw new AppException("Já existe um sócio assinando na receita.");
                }
                else if (x && !y)
                {
                    throw new AppException("Já existe um sócio administrador.");
                }
            }

            if (socio.Administrador)
            {
                if (JaExisteAdministrador(socio.EntrevistaId))
                {
                    throw new AppException("Já existe um sócio administrador.");
                }
            }
            if (socio.Assina)
            {
                if (JaExisteAssinaNaReceita(socio.EntrevistaId))
                {
                    throw new AppException("Já existe um sócio assinando na receita.");
                }
            }

            _socioRepository.Add(socio);
            _socioRepository.Commit();
        }

        private void AlterarSocio(Socio persistido, Socio corrente)
        {
            var validator = EntityValidatorFactory.CreateValidator();
            if (!validator.IsValid(corrente))
                throw new AppException(validator.GetInvalidMessages<Socio>(corrente));

            var specExisteSocio = SocioSpecification.ConsultaTexto(corrente.Nome);

            if (_socioRepository.AllMatching(specExisteSocio).Where(c => c.Id != persistido.Id).Any())
                throw new AppException("Já existe um sócio cadastrado com este nome.");

            if (JaExisteCpf(persistido.Cpf, persistido.EntrevistaId, persistido.Id))
            {
                throw new AppException("Já existe um sócio cadastrado com este CPF.");
            }

            _socioRepository.Merge(persistido, corrente);
            _socioRepository.Commit();
        }

        public bool JaExisteAdministrador(int entrevistaId)
        {
            try
            {
                var entrevista = _entrevistaRepository.Get(entrevistaId);
                foreach (var item in entrevista.ListaDeSocios)
                {
                    if (item.Administrador)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public bool JaExisteAssinaNaReceita(int entrevistaId)
        {
            try
            {
                var entrevista = _entrevistaRepository.Get(entrevistaId);
                foreach (var item in entrevista.ListaDeSocios)
                {
                    if (item.Assina)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public bool JaExisteCpf(string cpf, int entrevistaId, int? socioId)
        {
            try
            {
                if (!socioId.HasValue)
                {
                    var listaDeSocios = _entrevistaRepository.Get(entrevistaId).ListaDeSocios;
                    var existeCpf = listaDeSocios.FirstOrDefault(c => c.Cpf.Equals(cpf));

                    if (existeCpf != null)
                    {
                        return true;
                    }

                }
                else
                {
                    var listaDeSocios = _entrevistaRepository.Get(entrevistaId).ListaDeSocios;

                    var existeCpf = listaDeSocios.FirstOrDefault(c => c.Cpf.Equals(cpf) && c.Id != socioId);

                    if (existeCpf != null)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        #endregion Métodos Privados

        #region IDisposable

        public void Dispose()
        {
            _socioRepository.Dispose();
        }

        



        #endregion IDisposable
    }
}