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
    public class ClienteController : Controller
    {
        #region Membros

        readonly IClienteAppService _clienteService;

        public ClienteController(IClienteAppService clienteService)
        {
            _clienteService = clienteService;
        }

        #endregion

        #region Actions

        public ActionResult Index(PagedList<ClienteListDTO> pagedList)
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

        public ActionResult IndexGrid(PagedList<ClienteListDTO> pagedList)
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
                ClienteDTO clienteDTO;
                if (!id.HasValue || id == 0)
                {
                    clienteDTO = new ClienteDTO();
                    ViewBag.Filial = new List<ClienteListDTO>();
                }
                else
                {
                    clienteDTO = _clienteService.FindCliente(id.Value);
                    if (clienteDTO.TipoEmpresa == eTipoEmpresa.Matriz)
                    {
                        GetFilial(clienteDTO.Id);
                    }
                    else
                    {
                        ViewBag.Filial = new List<ClienteListDTO>();
                    }
                }

                AjusteContextoEditar(clienteDTO.MatrizId);

                return View(clienteDTO);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost, ValidateAntiForgeryToken()]
        public ActionResult POSTEditar(ClienteDTO clienteDTO)
        {
            try
            {
                if (clienteDTO.Id == 0)
                {
                    clienteDTO = _clienteService.AddCliente( clienteDTO);
                }
                else
                {
                    _clienteService.UpdateCliente(clienteDTO);
                }

                return JavaScript(
                    "MensagemSucesso('O cliente foi salvo com sucesso.');" +
                    "carregarPaginaAjax('" + Url.Action("Index", "Cliente") + "');");
            }
            catch (Exception ex)
            {
                TratamentoErro.Tratamento(this, ex);
                AjusteContextoEditar(clienteDTO.MatrizId);

                return View("Editar", clienteDTO);
            }
        }

        public ActionResult Excluir(int id)
        {
            try
            {
                _clienteService.RemoveCliente(id);
                return JavaScript(
                    "MensagemSucesso('Cliente excluído com sucesso!');" +
                    "carregarPaginaAjax('" + Url.Action("Index", "Cliente") + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MensagemErro('" + ex.Message + "');");
            }
        }

        #endregion

        #region Métodos Privados

        private void ConfigureGrid(PagedList<ClienteListDTO> pagedList)
        {
            try
            {
                //var deserializado = JsonConvert.DeserializeObject<dynamic>(pagedList.Adicional, new[] { new StringToNIntConverter() });
                //int? parametro = deserializado.EscolaId != null && !string.IsNullOrEmpty(deserializado.Parametro.Value as string) ? deserializado.Parametro : null;
                //pagedList.Adicional = JsonConvert.SerializeObject(deserializado);


                //Definindo a action da GridPartial
                pagedList.Action = "IndexGrid";

                //Obtenha a quantidade total de registros
                var totalRecords = (int)_clienteService.CountClientes(pagedList.SearchTerm);

                //Obtenha os registros
                IList<ClienteListDTO> entities = null;

                if (pagedList.Sort == "RazaoSocial")
                {
                    entities = _clienteService.FindClientes(pagedList.SearchTerm, c => c.RazaoSocial, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }
                else if (pagedList.Sort == "Cnpj")
                {
                    entities = _clienteService.FindClientes(pagedList.SearchTerm, c => c.Cnpj, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }
                else if (pagedList.Sort == "TipoEmpresa")
                {
                    entities = _clienteService.FindClientes(pagedList.SearchTerm, c => c.TipoEmpresa, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }
                else
                {
                    entities = _clienteService.FindClientes(pagedList.SearchTerm, c => c.NomeFantasia, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
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
            ViewBag.Matriz = GetMatriz();
            ViewBag.Empresa = GetNomeMatriz(matrizId);
            
        }

        private SelectList GetMatriz()
        {
            var clientes = _clienteService.GetMatriz();
            return new SelectList(clientes, "Id", "RazaoSocial");
        }
        private string GetNomeMatriz(int? matrizId)
        {
            if (matrizId.HasValue)
            {
                var empresaNome = _clienteService.FindCliente(matrizId.Value).RazaoSocial;
                return empresaNome;
            }

            return "Nenhuma empresa localizada.";
        }

        private void GetFilial(int matrizId)
        {
            ViewBag.Filial = _clienteService.GetFilial(matrizId);
        }
        #endregion
    }
}
