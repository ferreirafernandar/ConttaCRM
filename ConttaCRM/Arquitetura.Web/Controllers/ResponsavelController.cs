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

namespace Arquitetura.Web.Controllers
{
    [Authorize]
    public class ResponsavelController : Controller
    {
        #region Membros

        readonly IResponsavelAppService _responsavelService;

        public ResponsavelController(IResponsavelAppService responsavelService)
        {
            _responsavelService = responsavelService;
        }

        #endregion

        #region Actions

        public ActionResult Index(PagedList<ResponsavelListDTO> pagedList)
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

        public ActionResult IndexGrid(PagedList<ResponsavelListDTO> pagedList)
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

        public ActionResult Editar(int? id)
        {
            try
            {
                ResponsavelDTO responsavelDTO;
                if (!id.HasValue || id == 0)
                {
                    responsavelDTO = new ResponsavelDTO();
                }
                else
                {
                    responsavelDTO = _responsavelService.FindResponsavel(id.Value);
                }

                return View(responsavelDTO);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost, ValidateAntiForgeryToken()]
        public ActionResult POSTEditar(ResponsavelDTO responsavelDTO)
        {
            try
            {
                var tipo = Request["abertura"];

                if (!string.IsNullOrEmpty(tipo))
                {
                    if (tipo.Equals("Empresário Individual"))
                    {
                        responsavelDTO.TipoAbertura = eTipoAbertura.EmpreendedorIndividual;
                    }
                    else if (tipo.Equals("Empresário Individual"))
                    {
                        responsavelDTO.TipoAbertura = eTipoAbertura.EmpresarioIndividual;
                    }
                    else if (tipo.Equals("Sociedade Limitada"))
                    {
                        responsavelDTO.TipoAbertura = eTipoAbertura.SociedadeLimitada;
                    }
                    else if (tipo.Equals("EIRELI"))
                    {
                        responsavelDTO.TipoAbertura = eTipoAbertura.Eireli;
                    }
                    else if (tipo.Equals("Consórcio"))
                    {
                        responsavelDTO.TipoAbertura = eTipoAbertura.Consorcio;
                    }
                    else if (tipo.Equals("Cooperativa"))
                    {
                        responsavelDTO.TipoAbertura = eTipoAbertura.Cooperativa;
                    }
                    else if (tipo.Equals("Sociedade Anônima"))
                    {
                        responsavelDTO.TipoAbertura = eTipoAbertura.SociedadeAnonima;
                    }
                    else
                    {
                        responsavelDTO.TipoAbertura = eTipoAbertura.OutrosTiposJuridicos;
                    }
                }
                
                if (responsavelDTO.Id == 0)
                {
                    responsavelDTO = _responsavelService.AddResponsavel(responsavelDTO);
                }
                else
                {
                    _responsavelService.UpdateResponsavel(responsavelDTO);
                }

                return JavaScript(
                    "MensagemSucesso('O responsável foi salvo com sucesso.');" +
                    "carregarPaginaAjax('" + Url.Action("Index", "Responsavel") + "');");
            }
            catch (Exception ex)
            {
                TratamentoErro.Tratamento(this, ex);

                return View("Editar", responsavelDTO);
            }
        }

        public ActionResult Excluir(int id)
        {
            try
            {
                _responsavelService.RemoveResponsavel(id);
                return JavaScript(
                    "MensagemSucesso('Responsável excluído com sucesso!');" +
                    "carregarPaginaAjax('" + Url.Action("Index", "Responsavel") + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MensagemErro('" + ex.Message + "');");
            }
        }

        #endregion

        #region Métodos Privados

        private void ConfigureGrid(PagedList<ResponsavelListDTO> pagedList)
        {
            try
            {
                //var deserializado = JsonConvert.DeserializeObject<dynamic>(pagedList.Adicional, new[] { new StringToNIntConverter() });
                //int? parametro = deserializado.EscolaId != null && !string.IsNullOrEmpty(deserializado.Parametro.Value as string) ? deserializado.Parametro : null;
                //pagedList.Adicional = JsonConvert.SerializeObject(deserializado);


                //Definindo a action da GridPartial
                pagedList.Action = "IndexGrid";

                //Obtenha a quantidade total de registros
                var totalRecords = (int)_responsavelService.CountResponsaveis(pagedList.SearchTerm);

                //Obtenha os registros
                IList<ResponsavelListDTO> entities = null;

                if (pagedList.Sort == "Nome")
                {
                    entities = _responsavelService.FindResponsaveis(pagedList.SearchTerm, c => c.Nome, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }
                else if (pagedList.Sort == "Cpf")
                {
                    entities = _responsavelService.FindResponsaveis(pagedList.SearchTerm, c => c.Cpf, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }
                else if (pagedList.Sort == "Telefone")
                {
                    entities = _responsavelService.FindResponsaveis(pagedList.SearchTerm, c => c.Telefone, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }
                else if (pagedList.Sort == "Celular")
                {
                    entities = _responsavelService.FindResponsaveis(pagedList.SearchTerm, c => c.Celular, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }
                else if (pagedList.Sort == "Email")
                {
                    entities = _responsavelService.FindResponsaveis(pagedList.SearchTerm, c => c.Email, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }
                else
                {
                    entities = _responsavelService.FindResponsaveis(pagedList.SearchTerm, c => c.Sexo, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }

                //Defina os valores
                pagedList.Parametros(this, entities, totalRecords);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AjusteContextoEditar(int? matrizId)
        {
          
        }

        #endregion
    }
}
