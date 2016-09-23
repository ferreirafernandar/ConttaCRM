using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.Dominio.Aggregates.Base;
using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.DTO;
using Arquitetura.Web.Helpers;
using Arquitetura.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Arquitetura.Web.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        #region Membros

        readonly IUsuarioAppService _usuarioService;
        private readonly IClienteAppService _clienteService;

        public UsuarioController(IUsuarioAppService usuarioService, IClienteAppService clienteService)
        {
            _usuarioService = usuarioService;
            _clienteService = clienteService;
        }

        #endregion

        #region Actions

        public ActionResult Index(PagedList<UsuarioListDTO> pagedList)
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

        public ActionResult IndexGrid(PagedList<UsuarioListDTO> pagedList)
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
                UsuarioDTO usuarioDTO;
                if (!id.HasValue || id == 0)
                {
                    usuarioDTO = new UsuarioDTO { Ativo = true };
                }
                else
                {
                    usuarioDTO = _usuarioService.FindUsuario(id.Value);
                    GetNomeEmpresa(id.Value);
                }

                AjusteContextoEditar();

                return View(usuarioDTO);
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
                if (usuarioDTO.Id == 0)
                {
                    usuarioDTO = _usuarioService.AddUsuario(usuarioDTO);
                }
                else
                {
                    _usuarioService.UpdateUsuario(usuarioDTO);
                }

                return JavaScript(
                    "MensagemSucesso('O usuário foi salvo com sucesso.');" +
                    "carregarPaginaAjax('" + Url.Action("Index", "Usuario") + "');");
            }
            catch (Exception ex)
            {
                TratamentoErro.Tratamento(this, ex);
                AjusteContextoEditar();

                return View("Editar", usuarioDTO);
            }
        }

        public ActionResult Excluir(int id)
        {
            try
            {
                _usuarioService.RemoveUsuario(id);
                return JavaScript(
                    "MensagemSucesso('Usuário excluído com sucesso!');" +
                    "carregarPaginaAjax('" + Url.Action("Index", "Usuario") + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MensagemErro('" + ex.Message + "');");
            }
        }

        private SelectList GetEmpresas()
        {
            var clientes = _clienteService.FindClientes(null, c => c.RazaoSocial);
            return new SelectList(clientes, "Id", "RazaoSocial");
        }
        #endregion

        #region Métodos Privados

        private void ConfigureGrid(PagedList<UsuarioListDTO> pagedList)
        {
            try
            {
                //var deserializado = JsonConvert.DeserializeObject<dynamic>(pagedList.Adicional, new[] { new StringToNIntConverter() });
                //int? parametro = deserializado.EscolaId != null && !string.IsNullOrEmpty(deserializado.Parametro.Value as string) ? deserializado.Parametro : null;
                //pagedList.Adicional = JsonConvert.SerializeObject(deserializado);


                //Definindo a action da GridPartial
                pagedList.Action = "IndexGrid";

                //Obtenha a quantidade total de registros
                var totalRecords = (int)_usuarioService.CountUsuarios(pagedList.SearchTerm);

                //Obtenha os registros
                IList<UsuarioListDTO> entities = null;

                if (pagedList.Sort == "Nome")
                {
                    entities = _usuarioService.FindUsuarios(pagedList.SearchTerm, c => c.Nome, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }
                else if (pagedList.Sort == "NomeUsuario")
                {
                    entities = _usuarioService.FindUsuarios(pagedList.SearchTerm, c => c.NomeUsuario, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }
                else if (pagedList.Sort == "Email")
                {
                    entities = _usuarioService.FindUsuarios(pagedList.SearchTerm, c => c.Email, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
                }
                else
                {
                    entities = _usuarioService.FindUsuarios(pagedList.SearchTerm, c => c.Nome, pagedList.SortAsc, pagedList.Page, pagedList.PageSize);
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
            ViewBag.Empresas = GetEmpresas();
        }

        private void GetNomeEmpresa(int? empresaId)
        {

            if (empresaId.HasValue)
            {
                ViewBag.NomeEmpresa = _clienteService.FindCliente(empresaId.Value).RazaoSocial; 
            }
        }

        #endregion
    }
}
