using Arquitetura.Aplicacao.Base;
using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.Dominio.ControladorDeSessao;
using Arquitetura.DTO;
using Arquitetura.Web.Helpers;
using Arquitetura.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Arquitetura.Web.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        #region Membros

        private readonly IUsuarioAppService _usuarioService;

        public PerfilController(IUsuarioAppService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        #endregion

        #region Actions

        public ActionResult AlterarSenha()
        {
            try
            {
                var usuarioDTO = _usuarioService.FindUsuario(ControladorDeSessao.GetUsuario().Id);
                return View(usuarioDTO);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost, ValidateAntiForgeryToken()]
        public ActionResult AlterarSenha(string senhaAtual, string novaSenha, string confirmaNovaSenha)
        {
            try
            {
                _usuarioService.AlterarSenha(senhaAtual, novaSenha, confirmaNovaSenha);
                ViewBag.Sucesso = true;

                var usuarioDTO = _usuarioService.FindUsuario(ControladorDeSessao.GetUsuario().Id);
                return View(usuarioDTO);
            }
            catch (Exception ex)
            {
                TratamentoErro.Tratamento(this, ex);
                var usuarioDTO = _usuarioService.FindUsuario(ControladorDeSessao.GetUsuario().Id);
                return View(usuarioDTO);
            }
        }
        
        public ActionResult Editar()
        {
            try
            {
                var usuario = _usuarioService.FindUsuario(ControladorDeSessao.GetUsuario().Id);
                return View(usuario);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost, ValidateAntiForgeryToken()]
        public ActionResult POSTEditar(UsuarioDTO usuarioDTO)
        {
            try
            {
                _usuarioService.UpdatePerfilUsuario(usuarioDTO);

                return JavaScript(
                    "MensagemSucesso('Perfil salvo com sucesso.');" +
                    "carregarPaginaAjax('" + Url.Action("Editar", "Perfil") + "');");
            }
            catch (Exception ex)
            {
                TratamentoErro.Tratamento(this, ex);
                return View("Editar", usuarioDTO);
            }
        }
        

        #endregion
    }
}