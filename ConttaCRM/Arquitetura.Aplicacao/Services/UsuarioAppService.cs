using Arquitetura.Aplicacao.Base;
using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.Dominio.Aggregates.Base;
using Arquitetura.Dominio.Aggregates.ConfiguracaoAgg;
using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Aggregates.UsuarioAgg;
using Arquitetura.Dominio.Base;
using Arquitetura.Dominio.ControladorDeSessao;
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
    public class UsuarioAppService : IUsuarioAppService
    {
        #region Membros

        readonly IUsuarioRepository _usuarioRepository;
        readonly IConfiguracaoServidorEmailRepository _configuracaoRepository;
        readonly ITokenSenhaRepository _tokenSenhaRepository;

        #endregion

        #region Construtor

        public UsuarioAppService(
            IUsuarioRepository usuarioRepository,
            IConfiguracaoServidorEmailRepository configuracaoRepository,
            ITokenSenhaRepository tokenSenhaRepository)
        {
            if (usuarioRepository == null)
                throw new ArgumentNullException("usuarioRepository");

            if (configuracaoRepository == null)
                throw new ArgumentNullException("configuracaoRepository");

            if (tokenSenhaRepository == null)
                throw new ArgumentNullException("tokenSenhaRepository");

            _usuarioRepository = usuarioRepository;
            _configuracaoRepository = configuracaoRepository;
            _tokenSenhaRepository = tokenSenhaRepository;
        }

        #endregion

        #region Membros de IUsuarioAppService

        public UsuarioDTO AddUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                if (usuarioDTO == null)
                    throw new ArgumentNullException("usuarioDTO");

                string senha = null;
                if (usuarioDTO.Senha != null)
                {
                    senha = Encryption.Encrypt(usuarioDTO.Senha);
                }

                if (usuarioDTO.Cpf != null)
                    usuarioDTO.Cpf = usuarioDTO.Cpf.Replace(".", "").Replace("-", "").Replace("_", "").Trim();
                if (usuarioDTO.Telefone != null)
                    usuarioDTO.Telefone= usuarioDTO.Telefone.Replace("_", "").Trim();
                if (usuarioDTO.Celular != null)
                    usuarioDTO.Celular=   usuarioDTO.Celular.Replace("_", "").Trim();


                var usuario = UsuarioFactory.CreateUsuario(
                    usuarioDTO.NomeUsuario,
                    usuarioDTO.Email,
                    senha,
                    usuarioDTO.Nome,
                    usuarioDTO.Cpf,
                    usuarioDTO.Endereco,
                    usuarioDTO.Complemento,
                    usuarioDTO.Numero,
                    usuarioDTO.Bairro,
                    usuarioDTO.Cidade,
                    usuarioDTO.Estado,
                    usuarioDTO.Cep,
                    usuarioDTO.Telefone,
                    usuarioDTO.Celular,
                    usuarioDTO.Sexo,
                    usuarioDTO.Ativo,
                    usuarioDTO.TipoUsuario,
                    usuarioDTO.ClienteId);

                SalvarUsuario(usuario);

                //CrieUsuarioWindows(usuario);
                //CriePastaUsuario(usuario);

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Usuario, UsuarioDTO>(usuario);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public UsuarioDTO UpdateUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                if (usuarioDTO == null)
                    throw new ArgumentNullException("usuarioDTO");

                var persistido = _usuarioRepository.Get(usuarioDTO.Id);
                if (persistido == null)
                    throw new Exception("Usuario não encontrado.");

                var corrente = UsuarioFactory.CreateUsuario(
                    persistido.NomeUsuario,
                    usuarioDTO.Email,
                    persistido.Senha,
                    usuarioDTO.Nome,
                    usuarioDTO.Cpf,
                    usuarioDTO.Endereco,
                    usuarioDTO.Complemento,
                    usuarioDTO.Numero,
                    usuarioDTO.Bairro,
                    usuarioDTO.Cidade,
                    usuarioDTO.Estado,
                    usuarioDTO.Cep,
                    usuarioDTO.Telefone,
                    usuarioDTO.Celular,
                    usuarioDTO.Sexo,
                    usuarioDTO.Ativo,
                    usuarioDTO.TipoUsuario,
                    usuarioDTO.ClienteId);

                corrente.Id = persistido.Id;
                bool permissaoAlterada = persistido.Ativo != corrente.Ativo;
                
                AlterarUsuario(persistido, corrente);

                if (permissaoAlterada)
                {
                    AlterarPermissaoPasta(corrente);
                }

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Usuario, UsuarioDTO>(corrente);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public UsuarioDTO UpdatePerfilUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                if (usuarioDTO == null)
                    throw new ArgumentNullException("usuarioDTO");

                var persistido = _usuarioRepository.Get(ControladorDeSessao.GetUsuario().Id);
                if (persistido == null)
                    throw new Exception("Usuario não encontrado.");

                var corrente = UsuarioFactory.CreateUsuario(
                    persistido.NomeUsuario,
                    usuarioDTO.Email,
                    persistido.Senha,
                    usuarioDTO.Nome,
                    persistido.Cpf,
                    usuarioDTO.Endereco,
                    usuarioDTO.Complemento,
                    usuarioDTO.Numero,
                    usuarioDTO.Bairro,
                    usuarioDTO.Cidade,
                    usuarioDTO.Estado,
                    usuarioDTO.Cep,
                    usuarioDTO.Telefone,
                    usuarioDTO.Celular,
                    usuarioDTO.Sexo,
                    persistido.Ativo,
                    persistido.TipoUsuario,
                    persistido.ClienteId);

                corrente.Id = persistido.Id;

                AlterarUsuario(persistido, corrente);

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Usuario, UsuarioDTO>(corrente);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public void RemoveUsuario(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("Valor inválido.", "usuarioId");

                var usuario = _usuarioRepository.Get(usuarioId);
                if (usuario == null)
                    throw new Exception("Usuario não encontrado.");

                _usuarioRepository.Remove(usuario);
                _usuarioRepository.Commit();
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public UsuarioDTO FindUsuario(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new Exception("Id do usuário inválido.");

                var usuario = _usuarioRepository.Get(usuarioId);
                if (usuario == null)
                    throw new Exception("Usuário não encontrado.");

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Usuario, UsuarioDTO>(usuario);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public UsuarioDTO FindUsuario(string nomeUsuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomeUsuario))
                    throw new AppException("Informe o nome de usuário.");

                var spec = UsuarioSpecifications.ConsultaNomeUsuario(nomeUsuario);
                var usuario = _usuarioRepository.AllMatching(spec).SingleOrDefault();
                if (usuario == null)
                    throw new AppException("Usuário não encontrado.");

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Usuario, UsuarioDTO>(usuario);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public List<UsuarioListDTO> FindUsuarios<KProperty>(string texto, Expression<Func<Usuario, KProperty>> orderByExpression, bool ascending = true)
        {
            try
            {
                var spec = UsuarioSpecifications.ConsultaTexto(texto);
                List<Usuario> usuarios = _usuarioRepository.AllMatching<KProperty>(spec, orderByExpression, ascending).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<Usuario>, List<UsuarioListDTO>>(usuarios);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public List<UsuarioListDTO> FindUsuarios<KProperty>(string texto, Expression<Func<Usuario, KProperty>> orderByExpression, bool ascending, int pageIndex, int pageCount)
        {
            try
            {
                var spec = UsuarioSpecifications.ConsultaTexto(texto);
                List<Usuario> usuarios = _usuarioRepository.GetPaged<KProperty>(pageIndex, pageCount, spec, orderByExpression, ascending).ToList();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<List<Usuario>, List<UsuarioListDTO>>(usuarios);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public long CountUsuarios(string texto)
        {
            try
            {
                var spec = UsuarioSpecifications.ConsultaTexto(texto);
                return _usuarioRepository.Count(spec);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public void AutenticarUsuario(string nomeUsuario, string senha, bool continuarConectado)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomeUsuario))
                {
                    throw new AppException("Digite o nome de usuário.");
                }

                if (string.IsNullOrWhiteSpace(senha))
                {
                    throw new AppException("Digite a senha.");
                }

                var spec = UsuarioSpecifications.ConsultaNomeUsuario(nomeUsuario);
                var usuario = _usuarioRepository.AllMatching(spec).SingleOrDefault();
                if (usuario == null)
                {
                    throw new AppException("Usuário não encontrado.");
                }

                if (usuario.Senha != Encryption.Encrypt(senha))
                {
                    throw new AppException("Senha inválida.");
                }

                if (!usuario.Ativo)
                {
                    throw new AppException("Não foi possível autenticar o usuário. Por favor contate o suporte.");
                }
                
                //TODO: verificar usuario inadimplente

                ControladorDeSessao.Autenticar(usuario, continuarConectado);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public void AlterarSenha(string senhaAtual, string novaSenha, string confirmaNovaSenha)
        {
            try
            {
                var usuario = _usuarioRepository.Get(ControladorDeSessao.GetUsuario().Id);
                if (usuario == null)
                {
                    throw new Exception("Usuario da sessao nao encontrado.");
                }

                if (string.IsNullOrWhiteSpace(senhaAtual))
                {
                    throw new AppException("Informe a senha atual.");
                }


                if (string.IsNullOrWhiteSpace(novaSenha))
                {
                    throw new AppException("Informe a nova senha.");
                }

                if (novaSenha != confirmaNovaSenha)
                {
                    throw new AppException("A nova senha e a confirmação da nova senha não conferem.");
                }

                if (usuario.Senha != Encryption.Encrypt(senhaAtual))
                {
                    throw new AppException("A senha atual está incorreta.");
                }

                if (usuario.Senha == Encryption.Encrypt(novaSenha))
                {
                    throw new AppException("A nova senha não pode ser igual a senha atual.");
                }

                usuario.Senha = Encryption.Encrypt(novaSenha);
                _usuarioRepository.Commit();
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public UsuarioDTO FindUsuarioNull(string login)
        {
            try
            {
                if (string.IsNullOrEmpty(login))
                    return null;

                var usuario = _usuarioRepository.GetFiltered(c => c.NomeUsuario == login).SingleOrDefault();

                var adapter = TypeAdapterFactory.CreateAdapter();
                return adapter.Adapt<Usuario, UsuarioDTO>(usuario);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public string GerarTokenSenha(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("Valor inválido.", "usuarioId");

                var usuario = _usuarioRepository.Get(usuarioId);
                if (usuario == null)
                    throw new Exception("Usuario não encontrada.");

                var spec = UsuarioSpecifications.ConsulteTokenSenhaUsuario(usuarioId);
                var tokensSenhas = _tokenSenhaRepository.AllMatching(spec).ToList();

                foreach (var item in tokensSenhas.ToList())
                {
                    _tokenSenhaRepository.Remove(item);
                }

                var tokenSenha = UsuarioFactory.CreateTokenSenha(usuarioId);
                var validator = EntityValidatorFactory.CreateValidator();
                if (!validator.IsValid(tokenSenha))
                    throw new AppException(validator.GetInvalidMessages(tokenSenha));

                _tokenSenhaRepository.Add(tokenSenha);
                _tokenSenhaRepository.Commit();

                return tokenSenha.Token;
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public void ValidarTokenSenha(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                    throw new ArgumentException("Valor não pode ser nulo ou vazio.", "token");

                ValideToken(token);
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        public void RedefinirSenhaComToken(string token, string senha, string confirmaSenha)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                    throw new ArgumentException("Valor não pode ser nulo ou vazio.", "token");

                ValideToken(token);

                if (string.IsNullOrWhiteSpace(senha))
                {
                    throw new AppException("Nova senha não informada.");
                }

                if (string.IsNullOrWhiteSpace(confirmaSenha))
                {
                    throw new AppException("Confirmação da nova senha não informada.");
                }

                if (senha != confirmaSenha)
                {
                    throw new AppException("Nova senha não confere com a confirmação da nova senha.");
                }

                var spec = UsuarioSpecifications.ConsulteTokenSenha(token);
                var tokenSenha = _tokenSenhaRepository.AllMatching(spec).Single();

                var usuario = _usuarioRepository.Get(tokenSenha.UsuarioId);

                usuario.Senha = Encryption.Encrypt(senha);
                _tokenSenhaRepository.Remove(tokenSenha);

                _tokenSenhaRepository.Commit();
            }
            catch (Exception ex)
            {
                throw ManipuladorDeExcecao.TrateExcecao(ex);
            }
        }

        #endregion

        #region Métodos Privados

        void SalvarUsuario(Usuario usuario)
        {
            var validator = EntityValidatorFactory.CreateValidator();
            if (!validator.IsValid(usuario))
                throw new AppException(validator.GetInvalidMessages<Usuario>(usuario));
            
            var specExisteUsuario = UsuarioSpecifications.ConsultaNomeUsuario(usuario.NomeUsuario);
            if (_usuarioRepository.AllMatching(specExisteUsuario).Any())
                throw new AppException("Já existe um usuário cadastrado com este nome de usuário.");

            _usuarioRepository.Add(usuario);
            _usuarioRepository.Commit();
        }

        void AlterarUsuario(Usuario persistido, Usuario corrente)
        {
            var validator = EntityValidatorFactory.CreateValidator();
            if (!validator.IsValid(corrente))
                throw new AppException(validator.GetInvalidMessages<Usuario>(corrente));

            var specExisteUsuario = UsuarioSpecifications.ConsultaNomeUsuario(corrente.NomeUsuario);
            if (_usuarioRepository.AllMatching(specExisteUsuario).Where(c => c.Id != persistido.Id).Any())
                throw new AppException("Já existe um usuário cadastrado com este nome de usuário.");

            _usuarioRepository.Merge(persistido, corrente);
            _usuarioRepository.Commit();
        }

        private void CrieUsuarioWindows(Usuario usuario)
        {
            if (usuario.IsTransient())
                throw new Exception("Usuario nao deve ser transiente.");

            try
            {
                Util.CrieNovoUsuario(usuario.NomeUsuario, Encryption.DecryptToString(usuario.Senha), usuario.Nome);
            }
            catch (Exception e)
            {
                _usuarioRepository.Remove(usuario);
                _usuarioRepository.Commit();

                throw new Exception("Não foi possível criar o usuário do Windows. " + e.ToString());
            }
        }

        private void CriePastaUsuario(Usuario usuario)
        {
            try
            {
                if (usuario.IsTransient())
                    throw new Exception("Usuario nao deve ser transiente.");

                var configuracao = _configuracaoRepository.GetAll().Single();
                if (string.IsNullOrWhiteSpace(configuracao.PastaRaiz))
                    throw new Exception("Pasta raiz nao configurada.");

                if (!Directory.Exists(configuracao.PastaRaiz))
                    throw new Exception("A pasta raiz nao existe.");

                string path = Path.Combine(configuracao.PastaRaiz, usuario.NomeUsuario);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    ConfigurePermissaoDePastaUsuario(path, usuario.NomeUsuario, true);
                }
            }
            catch
            {
                //nao foi possivel criar a pasta do usuario
            }
        }

        private void AlterarPermissaoPasta(Usuario usuario)
        {
            try
            {
                if (usuario.IsTransient())
                    throw new Exception("Usuario nao deve ser transiente.");

                var configuracao = _configuracaoRepository.GetAll().Single();
                if (string.IsNullOrWhiteSpace(configuracao.PastaRaiz))
                    throw new Exception("Pasta raiz nao configurada.");

                if (!Directory.Exists(configuracao.PastaRaiz))
                    throw new Exception("A pasta raiz nao existe.");

                string path = Path.Combine(configuracao.PastaRaiz, usuario.NomeUsuario);

                if (Directory.Exists(path))
                {
                    ConfigurePermissaoDePastaUsuario(path, usuario.NomeUsuario, usuario.Ativo);
                }

                var subdiretorios = Directory.GetDirectories(path);
                foreach (var item in subdiretorios)
                {
                    ConfigurePermissaoDePastaProduto(item, usuario.NomeUsuario, usuario.Ativo);
                }
            }
            catch
            {
                //nao foi possivel alterar a permissao
            }
        }

        private void ConfigurePermissaoDePastaUsuario(string path, string nomeUsuario, bool permitir)
        {
            if (permitir)
            {
                Util.AddDirectorySecurity(path, nomeUsuario, FileSystemRights.ListDirectory, AccessControlType.Allow);
            }
            else
            {
                Util.AddDirectorySecurity(path, nomeUsuario, FileSystemRights.ListDirectory, AccessControlType.Deny);
            }
        }

        private void ConfigurePermissaoDePastaProduto(string path, string nomeUsuario, bool permitir)
        {
            if (permitir)
            {
                Util.AddDirectorySecurity(path, nomeUsuario, FileSystemRights.FullControl, AccessControlType.Allow);
            }
            else
            {
                Util.AddDirectorySecurity(path, nomeUsuario, FileSystemRights.FullControl, AccessControlType.Deny);
            }
        }

        private void ValideToken(string token)
        {
            var spec = UsuarioSpecifications.ConsulteTokenSenha(token);
            var tokenSenha = _tokenSenhaRepository.AllMatching(spec).SingleOrDefault();

            if (tokenSenha == null)
                throw new AppException("Link inválido ou já utilizado.");

            if (tokenSenha.Data > DateTime.Now.AddDays(2))
                throw new AppException("Este link expirou.");
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _usuarioRepository.Dispose();
            _configuracaoRepository.Dispose();
            _tokenSenhaRepository.Dispose();
        }

        #endregion
    }
}
