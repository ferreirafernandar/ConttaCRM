using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.Dominio.Aggregates.Base;
using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Enums;
using Arquitetura.DTO;
using Arquitetura.Web.Helpers;
using Arquitetura.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
namespace Arquitetura.Web.Controllers
{
    [Authorize]
    public class EntrevistaController : Controller
    {
        #region Membros

        readonly IEntrevistaAppService _entrevistaService;
        private readonly IClienteAppService _clienteService; //ijeção de dependência
        private readonly IUsuarioAppService _usuarioService;


        public EntrevistaController(IEntrevistaAppService entrevistaService, IClienteAppService clienteService, IUsuarioAppService usuarioService)
        {
            _entrevistaService = entrevistaService;
            _clienteService = clienteService;
            _usuarioService = usuarioService;
        }

        #endregion

        #region Actions

        public ActionResult Editar()
        {
            try
            {
                if (Session["ResponsavelId"] == null)
                {
                    throw new Exception("Responsável ID inválido.");
                }

                int? responsavelId = (int)Session["ResponsavelId"];

                if (!responsavelId.HasValue)
                    throw new Exception("Responsável ID inválido.");

                var existeEntrevista = _entrevistaService.FindEntrevistaPorResponsavel(responsavelId.Value).ToList();

                EntrevistaDTO entrevistaDTO;
                        
                if (existeEntrevista.Any())
                {
                    entrevistaDTO = _entrevistaService.FindEntrevista(existeEntrevista.First().Id);
                }
                else
                {
                    entrevistaDTO = new EntrevistaDTO { ResponsalvelId = responsavelId };
                }
              
                AjusteContextoEditar();

                return View(entrevistaDTO);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost, ValidateAntiForgeryToken()]
        public ActionResult POSTEditar(EntrevistaDTO  entrevistaDTO)
        {
            try
            {
                if (entrevistaDTO.Id == 0)
                {
                     entrevistaDTO = _entrevistaService.AddEntrevista(entrevistaDTO);
                }
                else
                {
                    _entrevistaService.UpdateEntrevista(entrevistaDTO);
                }

                return JavaScript(
                    "MensagemSucesso('A entrevista foi salva com sucesso.');" +
                    "carregarPaginaAjax('" + Url.Action("Constituicao", "Index") + "');");
            }
            catch (Exception ex)
            {
                TratamentoErro.Tratamento(this, ex);
                AjusteContextoEditar();

                return View("Editar", entrevistaDTO);
            }
        }

        public ActionResult Excluir(int id)
        {
            try
            {
                _entrevistaService.RemoveEntrevista(id);
                return JavaScript(
                    "MensagemSucesso('Entrevista excluída com sucesso!');" +
                    "carregarPaginaAjax('" + Url.Action("Index", "Entrevista") + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MensagemErro('" + ex.Message + "');");
            }
        }
        #endregion

        #region Métodos Privados

        public void AjusteContextoEditar()
        {
           
        }

        #endregion
    }

}
