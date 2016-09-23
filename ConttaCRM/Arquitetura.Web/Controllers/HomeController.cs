using Arquitetura.Aplicacao.Base;
using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.Dominio.Aggregates.Base;
using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Aggregates.UsuarioAgg;
using Arquitetura.Dominio.Base;
using Arquitetura.Dominio.ControladorDeSessao;
using Arquitetura.Dominio.Enums;
using Arquitetura.DTO;
using Arquitetura.Web.Helpers;
using Arquitetura.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Arquitetura.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Membros

        private readonly IUsuarioAppService _usuarioService;
        private readonly IConfiguracaoAppService _configuracaoService;

        public HomeController(
            IUsuarioAppService usuarioService,
            IConfiguracaoAppService configuracaoService)
        {
            _usuarioService = usuarioService;
            _configuracaoService = configuracaoService;
        }

        #endregion

        #region Actions

        public ActionResult Index(string alertSuccess)
        {
            //var usuario = new UsuarioDTO();

            //usuario.NomeUsuario = "fernanda";
            //usuario.Email = "ferreirafernandar@gmail.com";
            //usuario.Senha = "123456";
            //usuario.Nome = "Fernanda Rodrigues Ferreira";
            //usuario.Cpf = "03350980104";
            //usuario.Endereco = "Rua 50";
            //usuario.Complemento = "Edifício Espanha";
            //usuario.Numero = "66";
            //usuario.Bairro = "Castelo Branco";
            //usuario.Cidade = "Goiânia";
            //usuario.Estado = eEstado.GO;
            //usuario.Cep = "74410080";
            //usuario.Telefone = "6236227333";
            //usuario.Celular = "6281300989";
            //usuario.Sexo = eSexo.Feminino;
            //usuario.Ativo = true;
            //usuario.TipoUsuario = eTipoUsuario.Administrador;
            //_usuarioService.AddUsuario(usuario);


            if (ControladorDeSessao.EstaAutenticado())
            {
                return RedirectToAction("Inicio");
            }

            FormsAuthentication.SignOut();
            ViewBag.AlertSuccess = alertSuccess;

            return View();
        }

        [HttpPost]
        public ActionResult Index(string usuario, string senha, string returnUrl, string continuarConectado)
        {
            try
            {
                bool conectado = false;
                if (continuarConectado != null && continuarConectado == "on")
                {
                    conectado = true;
                }

                _usuarioService.AutenticarUsuario(usuario, senha, conectado);

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Inicio");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Usuario = usuario;
                ViewBag.ReturnUrl = returnUrl;

                return View();
            }
        }

        [Authorize]
        public ActionResult Sair()
        {
            ControladorDeSessao.Desautenticar();
            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Sobre()
        {
            return View();
        }

        public ActionResult Contato()
        {
            return View(new ContatoModel());
        }

        [HttpPost, ValidateAntiForgeryToken()]
        public ActionResult Contato(ContatoModel contatoModel)
        {
            try
            {
                if (!Util.IsValidEmail(contatoModel.Email))
                {
                    throw new Exception("O e-mail informado é inválido.");
                }

                var sbMensagem = new StringBuilder();
                sbMensagem.AppendLine("<p>" + contatoModel.Nome + " visitou o site Visão Web e deixou uma mensagem.</p>");
                sbMensagem.AppendLine("<br />");
                sbMensagem.AppendLine("<p>Email: " + contatoModel.Email + "</p>");
                sbMensagem.AppendLine("<p>Telefone: " + contatoModel.Telefone + "</p>");
                sbMensagem.AppendLine("<p>Assunto: " + contatoModel.Assunto + "</p>");
                sbMensagem.AppendLine("<p>Mensagem:</p>");
                sbMensagem.AppendLine("<p>" + contatoModel.Mensagem + "</p>");
                sbMensagem.AppendLine("<br/><hr />");
                sbMensagem.AppendLine("<p>Visão Web - Empresa</p>");
                sbMensagem.AppendLine("<p>Esta é uma mensagem automática. Não responda a esta mensagem.</p>");

                var configuracao = _configuracaoService.GetConfiguracaoServidorEmail();

                if (!configuracao.UtilizarEnvioDeEmail)
                {
                    throw new Exception("Não foi possível completar a operação.");
                }

                Util.SendEmail(
                    "Contato realizado pelo site Gestão Escolar",
                    sbMensagem.ToString(),
                    configuracao.Conta,
                    Encryption.DecryptToString(configuracao.Senha),
                    configuracao.Smtp,
                    configuracao.Porta.Value,
                    configuracao.Ssl,
                    "renanstreit@gmail.com");

                ViewBag.AlertSuccess = "E-mail enviado com sucesso!";
                return View("Contato", new ContatoModel());
            }
            catch (Exception ex)
            {
                TratamentoErro.Tratamento(this, ex);
                return View("Contato", contatoModel);
            }
        }

        public ActionResult Registrar()
        {
            ViewBag.EhRegistrar = true;
            return View(new UsuarioDTO());
        }

        [HttpPost]
        public ActionResult Registrar(UsuarioDTO usuarioDTO, string confirmacaoSenha)
        {
            try
            {
                usuarioDTO.Ativo = true;

                _usuarioService.AutenticarUsuario(usuarioDTO.NomeUsuario, usuarioDTO.Senha, false);

                return View("RegistroSucesso", usuarioDTO);
            }
            catch (Exception ex)
            {
                TratamentoErro.Tratamento(this, ex);
                ViewBag.EhRegistrar = true;

                return View("FormRegistrar", usuarioDTO);
            }
        }

        public ActionResult PageNotFound()
        {
            return Content("A página que você está tentando acessar não existe.");
        }

        public ActionResult Unauthorized()
        {
            return Content("Você não tem permissão para visualizar esta página.");
        }

        [Authorize]
        public ActionResult Inicio()
        {
            return View();
        }

        public ActionResult RecuperarSenha(string usuario)
        {
            ViewBag.Usuario = usuario;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken()]
        public ActionResult RecuperarSenha(string usuario, FormCollection form)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario))
                {
                    throw new Exception("Informe o nome de usuário.");
                }

                var usuarioDTO = _usuarioService.FindUsuarioNull(usuario);
                if (usuarioDTO == null)
                {
                    throw new Exception("Usuário não encontrado.");
                }

                var token = _usuarioService.GerarTokenSenha(usuarioDTO.Id);

                var url = string.Format("http://{0}{1}", Request.Url.Authority, Url.Action("RedefinirSenha", "Home", new { id = token }));

                var sbMensagem = new StringBuilder();
                sbMensagem.AppendLine("<p>Uma solicitação de recuperação de senha foi enviada a partir do sistema Visão Web.</p>");
                sbMensagem.AppendLine("<p>Caso não tenha solicitado nenhuma recuperação de senha favor desconsiderar esta mensagem.</p>");
                sbMensagem.AppendLine("<br />");
                sbMensagem.AppendLine("<p>Para redefinir sua senha clique no link abaixo. Caso o link não funcione copie e cole o link em seu navegador. O link tem validade de 48 horas.</p>");
                sbMensagem.AppendLine("<br />");
                sbMensagem.AppendLine(string.Format("<p><a href=\"{0}\">{0}</a></p>", url));
                sbMensagem.AppendLine("<br /><hr />");
                sbMensagem.AppendLine(string.Format("<p>Visão Web - {0}</p>", DateTime.Now.Year));
                sbMensagem.AppendLine("<p>Esta é uma mensagem automática. Não responda a esta mensagem.</p>");

                var emailDTO = new EmailDTO();
                emailDTO.Assunto = "Recuperar Senha - Visão Web";
                emailDTO.Para = usuarioDTO.Email;
                emailDTO.Mensagem = sbMensagem.ToString();

                var configuracao = _configuracaoService.GetConfiguracaoServidorEmail();
                Util.SendEmail(emailDTO.Assunto, emailDTO.Mensagem, configuracao.Conta, Encryption.DecryptToString(configuracao.Senha), configuracao.Smtp, configuracao.Porta.Value, configuracao.Ssl, emailDTO.Para);

                ViewBag.AlertSuccess = "E-mail enviado com sucesso! Acesse seu e-mail e siga as instruções de recuperação de senha.";

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Usuario = usuario;

                return View();
            }
        }

        public ActionResult Redefinirsenha(string id)
        {
            try
            {
                _usuarioService.ValidarTokenSenha(id);
                ViewBag.Token = id;
                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpPost, ValidateAntiForgeryToken()]
        public ActionResult Redefinirsenha(string senha, string confirma, string token)
        {
            try
            {
                _usuarioService.RedefinirSenhaComToken(token, senha, confirma);

                return RedirectToAction("Index", new { alertSuccess = "Senha redefinida com sucesso! Entre com seu usuário e nova senha para continuar." });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Token = token;

                return View();
            }
        }

        #endregion
    }
}