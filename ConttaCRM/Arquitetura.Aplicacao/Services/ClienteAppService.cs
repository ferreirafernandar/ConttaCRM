using Arquitetura.Aplicacao.Base;
using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.Dominio.Aggregates.Base;
using Arquitetura.Dominio.Aggregates.ClienteAgg;
using Arquitetura.Dominio.Base;
using Arquitetura.Dominio.Enums;
using Arquitetura.Dominio.Repositories.Interfaces;
using Arquitetura.DTO;
using Arquitetura.Infraestrutura.Adapter;
using Arquitetura.Infraestrutura.Validator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.AccessControl;

namespace Arquitetura.Aplicacao.Services
{
    public class ClienteAppService : IClienteAppService
    {
        #region Membros

        readonly IClienteRepository _clienteRepository;

        #endregion

        #region Construtor

        public ClienteAppService(
            IClienteRepository clienteRepository,
            IConfiguracaoServidorEmailRepository configuracaoRepository)
        {
            if (clienteRepository == null)
                throw new ArgumentNullException("ClienteRepository");

            _clienteRepository = clienteRepository;
        }

        #endregion

        #region Membros de IClienteAppService

        public ClienteDTO AddCliente(ClienteDTO clienteDTO)
        {
            try
            {
                if (clienteDTO == null)
                    throw new ArgumentNullException("ClienteDTO");

                if (clienteDTO.Cnpj != null)
                    clienteDTO.Cnpj = clienteDTO.Cnpj.Replace("-", "").Replace("/", "").Replace(".", "").Replace("_", "").Trim();
                if (clienteDTO.Telefone != null)
                   clienteDTO.Telefone = clienteDTO.Telefone.Replace("_", "").Replace("-","").Trim();
                if (clienteDTO.Celular != null)
                    clienteDTO.Celular = clienteDTO.Celular.Replace("_", "").Replace("-", "").Trim();

                var Cliente = ClienteFactory.CreateCliente(
                    clienteDTO.NomeFantasia,
                    clienteDTO.RazaoSocial,
                    clienteDTO.Cnpj,
                    clienteDTO.InscricaoEstadual,
                    clienteDTO.Email,
                    clienteDTO.Telefone,
                    clienteDTO.Celular,
                    clienteDTO.Skype,
                    clienteDTO.NomeResponsavel,
                    clienteDTO.Rua,
                    clienteDTO.Numero,
                    clienteDTO.Complemento,
                    clienteDTO.Bairro,
                    clienteDTO.Cidade,
                    clienteDTO.Estado,
                    clienteDTO.Cep,
                    clienteDTO.TipoEmpresa,
                    clienteDTO.MatrizId
                    );

                SalvarCliente(Cliente);

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Cliente, ClienteDTO>(Cliente);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public ClienteDTO UpdateCliente(ClienteDTO clienteDTO)
        {
            try
            {
                if (clienteDTO == null)
                    throw new ArgumentNullException("clienteDTO");

                var persistido = _clienteRepository.Get(clienteDTO.Id);
                if (persistido == null)
                    throw new Exception("Cliente não encontrado.");

                var corrente = ClienteFactory.CreateCliente(
                    clienteDTO.NomeFantasia,
                    clienteDTO.RazaoSocial,
                    persistido.Cnpj,
                    persistido.InscricaoEstadual,
                    clienteDTO.Email,
                    clienteDTO.Telefone,
                    clienteDTO.Celular,
                    clienteDTO.Skype,
                    clienteDTO.NomeResponsavel,
                    clienteDTO.Rua,
                    clienteDTO.Numero,
                    clienteDTO.Complemento,
                    clienteDTO.Bairro,
                    clienteDTO.Cidade,
                    clienteDTO.Estado,
                    clienteDTO.Cep,
                    persistido.TipoEmpresa,
                    persistido.MatrizId);

                corrente.Id = persistido.Id;
                
                AlterarCliente(persistido, corrente);

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Cliente, ClienteDTO>(corrente);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public ClienteDTO UpdatePerfilCliente(ClienteDTO clienteDTO)
        {
            try
            {
                if (clienteDTO == null)
                    throw new ArgumentNullException("clienteDTO");

                var persistido = _clienteRepository.Get(clienteDTO.Id);
                if (persistido == null)
                    throw new Exception("Cliente não encontrado.");

                var corrente = ClienteFactory.CreateCliente(
                    clienteDTO.NomeFantasia,
                    clienteDTO.RazaoSocial,
                    persistido.Cnpj,
                    persistido.InscricaoEstadual,
                    clienteDTO.Email,
                    clienteDTO.Telefone,
                    clienteDTO.Celular,
                    clienteDTO.Skype,
                    clienteDTO.NomeResponsavel,
                    clienteDTO.Rua,
                    clienteDTO.Numero,
                    clienteDTO.Complemento,
                    clienteDTO.Bairro,
                    clienteDTO.Cidade,
                    clienteDTO.Estado,
                    clienteDTO.Cep,
                    persistido.TipoEmpresa,
                    persistido.MatrizId);

                corrente.Id = persistido.Id;

                AlterarCliente(persistido, corrente);

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Cliente, ClienteDTO>(corrente);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public void RemoveCliente(int clienteId)
        {
            try
            {
                if (clienteId <= 0)
                    throw new ArgumentException("Valor inválido.", "clienteId");

                var Cliente = _clienteRepository.Get(clienteId);
                if (Cliente == null)
                    throw new Exception("Cliente não encontrado.");

                _clienteRepository.Remove(Cliente);
                _clienteRepository.Commit();
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public ClienteDTO FindCliente(int clienteId)
        {
            try
            {
                if (clienteId <= 0)
                    throw new Exception("Id do cliente inválido.");

                var Cliente = _clienteRepository.Get(clienteId);
                if (Cliente == null)
                    throw new Exception("Cliente não encontrado.");

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Cliente, ClienteDTO>(Cliente);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public ClienteDTO FindCliente(string nomeCliente)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomeCliente))
                    throw new AppException("Informe o nome de cliente.");

                var spec = ClienteSpecification.ConsultaTexto(nomeCliente);
                var Cliente = _clienteRepository.AllMatching(spec).SingleOrDefault();
                if (Cliente == null)
                    throw new AppException("Cliente não encontrado.");

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Cliente, ClienteDTO>(Cliente);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public List<ClienteListDTO> FindClientes<KProperty>(string texto, Expression<Func<Cliente, KProperty>> orderByExpression, bool ascending = true)
        {
            try
            {
                var spec = ClienteSpecification.ConsultaTexto(texto);
                List<Cliente> Clientes = _clienteRepository.AllMatching<KProperty>(spec, orderByExpression, ascending).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<Cliente>, List<ClienteListDTO>>(Clientes);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public List<ClienteListDTO> FindClientes<KProperty>(string texto, Expression<Func<Cliente, KProperty>> orderByExpression, bool ascending, int pageIndex, int pageCount)
        {
            try
            {
                var spec = ClienteSpecification.ConsultaTexto(texto);
                List<Cliente> Clientes = _clienteRepository.GetPaged<KProperty>(pageIndex, pageCount, spec, orderByExpression, ascending).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<Cliente>, List<ClienteListDTO>>(Clientes);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public long CountClientes(string texto)
        {
            try
            {
                var spec = ClienteSpecification.ConsultaTexto(texto);
                return _clienteRepository.Count(spec);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

       

        public ClienteDTO FindClienteNull(string login)
        {
            try
            {
                if (string.IsNullOrEmpty(login))
                    return null;

                var Cliente = _clienteRepository.GetFiltered(c => c.NomeFantasia == login).SingleOrDefault();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Cliente, ClienteDTO>(Cliente);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }


        #endregion

        #region Métodos Privados

        void SalvarCliente(Cliente Cliente)
        {
            var validator = EntityValidatorFactory.CreateValidator();
            if (!validator.IsValid(Cliente))
                throw new AppException(validator.GetInvalidMessages<Cliente>(Cliente));
            
            var specExisteCliente = ClienteSpecification.ConsultaTexto(Cliente.NomeFantasia);
            if (_clienteRepository.AllMatching(specExisteCliente).Any())
                throw new AppException("Já existe um cliente cadastrado com este nome.");

            _clienteRepository.Add(Cliente);
            _clienteRepository.Commit();
        }

        void AlterarCliente(Cliente persistido, Cliente corrente)
        {
            var validator = EntityValidatorFactory.CreateValidator();
            if (!validator.IsValid(corrente))
                throw new AppException(validator.GetInvalidMessages<Cliente>(corrente));

            var specExisteCliente = ClienteSpecification.ConsultaTexto(corrente.NomeFantasia);
            if (_clienteRepository.AllMatching(specExisteCliente).Where(c => c.Id != persistido.Id).Any())
                throw new AppException("Já existe um cliente cadastrado com este nome.");

            _clienteRepository.Merge(persistido, corrente);
            _clienteRepository.Commit();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _clienteRepository.Dispose();
        }

        #endregion


        public List<ClienteListDTO> GetMatriz()
        {
            try
            {
                List<Cliente> Clientes = _clienteRepository.GetFiltered(c => c.TipoEmpresa == eTipoEmpresa.Matriz).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<Cliente>, List<ClienteListDTO>>(Clientes);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public List<ClienteListDTO> GetFilial(int MatrizId)
        {
            try
            {
                List<Cliente> Clientes = _clienteRepository.GetFiltered(c => c.TipoEmpresa == eTipoEmpresa.Filial && c.MatrizId.Value == MatrizId).ToList();
                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<Cliente>, List<ClienteListDTO>>(Clientes);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }
    }
}
