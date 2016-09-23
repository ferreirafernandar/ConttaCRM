using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.DTO;
using Arquitetura.Web.Helpers;
using Arquitetura.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Arquitetura.Web.Controllers
{
    [Authorize]
    public class SocioController : Controller
    {
        #region Membros

        private readonly ISocioAppService _socioService;

        public SocioController(ISocioAppService socioService)
        {
            _socioService = socioService;
        }

        #endregion Membros

        #region Actions

        public ActionResult Index(PagedList<SocioListDTO> pagedList)
        {
            try
            {
                ConfigureGrid(pagedList);
                return View(pagedList);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public ActionResult IndexGrid(PagedList<SocioListDTO> pagedList)
        {
            try
            {
                ConfigureGrid(pagedList);
                return View("IndexGridViewPartial", pagedList);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult Editar(int? id, int? idEntrevista)
        {
            try
            {
                SocioDTO socioDTO;
                if (!id.HasValue || id == 0 && idEntrevista.HasValue)
                {
                    socioDTO = new SocioDTO { EntrevistaId = idEntrevista.Value };
                }
                else
                {
                    socioDTO = _socioService.FindSocio(id.Value);
                }

                return View(socioDTO);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost, ValidateAntiForgeryToken()]
        public ActionResult POSTEditar(SocioDTO socioDTO)
        {
            try
            {
                if (socioDTO.Id == 0)
                {
                    socioDTO = _socioService.AddSocio(socioDTO);
                }
                else
                {
                    _socioService.UpdateSocio(socioDTO);
                }

                return JavaScript(
                    "MensagemSucesso('O sócio foi salvo com sucesso.');" +
                    "carregarPaginaAjax('" + Url.Action("Index", "Socio") + "');");
            }
            catch (Exception ex)
            {
                TratamentoErro.Tratamento(this, ex);

                return View("Editar", socioDTO);
            }
        }

        public ActionResult Excluir(int id)
        {
            try
            {
                _socioService.RemoveSocio(id);
                return JavaScript(
                    "MensagemSucesso('Sócio foi excluído com sucesso!');" +
                    "carregarPaginaAjax('" + Url.Action("Index", "Socio") + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MensagemErro('" + ex.Message + "');");
            }
        }

        #endregion Actions

        #region Métodos Privados

        private void ConfigureGrid(PagedList<SocioListDTO> pagedList)
        {
            try
            {
                //Definindo a action da GridPartial
                pagedList.Action = "IndexGrid";

                //Obtenha a quantidade total de registros
                var totalRecords = (int)_socioService.CountSocios(pagedList.SearchTerm);

                //Obtenha os registros
                IList<SocioListDTO> entities = null;

                if (pagedList.Sort == "Nome")
                {
                    entities = _socioService.FindSocios(pagedList.SearchTerm, c => c.Nome, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }
                else if (pagedList.Sort == "Cpf")
                {
                    entities = _socioService.FindSocios(pagedList.SearchTerm, c => c.Cpf, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }
                else if (pagedList.Sort == "Telefone")
                {
                    entities = _socioService.FindSocios(pagedList.SearchTerm, c => c.Telefone, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }
                else if (pagedList.Sort == "Celular")
                {
                    entities = _socioService.FindSocios(pagedList.SearchTerm, c => c.Celular, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }
                else if (pagedList.Sort == "Email")
                {
                    entities = _socioService.FindSocios(pagedList.SearchTerm, c => c.Email, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }
                else
                {
                    entities = _socioService.FindSocios(pagedList.SearchTerm, c => c.Sexo, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }

                //Defina os valores
                pagedList.Parametros(this, entities, totalRecords);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AjusteContextoEditar()
        {
        }

        #endregion Métodos Privados
    }
}