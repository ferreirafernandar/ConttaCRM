using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.DTO;
using Arquitetura.Infraestrutura.Util;
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
    public class ConfiguracaoController : Controller
    {
        #region Membros

        readonly IConfiguracaoAppService _configuracaoService;

        public ConfiguracaoController(IConfiguracaoAppService configuracaoService)
        {
            _configuracaoService = configuracaoService;
        }

        #endregion

        #region Actions

        public ActionResult Index()
        {
            try
            {
                var configuracaoServidorEmailDTO = _configuracaoService.GetConfiguracaoServidorEmail();
                configuracaoServidorEmailDTO.Senha = null;

                return View(configuracaoServidorEmailDTO);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost, ValidateAntiForgeryToken()]
        public ActionResult Index(ConfiguracaoServidorEmailDTO configuracaoServidorEmailDTO)
        {
            try
            {
                _configuracaoService.UpdateConfiguracaoServidorEmail(configuracaoServidorEmailDTO);

                return JavaScript(
                    "MensagemSucesso('A configuração foi salva com sucesso.');" +
                    "carregarPaginaAjax('" + Url.Action("Index", "Configuracao") + "');");
            }
            catch (Exception ex)
            {
                TratamentoErro.Tratamento(this, ex);
                return View(configuracaoServidorEmailDTO);
            }
        }

        [HttpPost]
        public ActionResult TestaEnvioEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    throw new Exception("Digite um endereço de e-mail para o destinatário.");

                var configuracao = _configuracaoService.GetConfiguracaoServidorEmail();

                Util.SendEmail(
                    "VisaoWeb - Teste de envio de e-mail",
                    "O servidor de e-mail está configurado corretamente.",
                    configuracao.Conta,
                    configuracao.Senha,
                    configuracao.Smtp,
                    configuracao.Porta ?? 25,
                    configuracao.Ssl,
                    email);

                return Json(new { sucesso = true });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }

        #endregion
    }
}
