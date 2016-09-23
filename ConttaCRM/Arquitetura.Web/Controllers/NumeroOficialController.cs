using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.DTO;
using Arquitetura.Web.Helpers;
using System;
using System.Web.Mvc;
using System.Linq;

namespace Arquitetura.Web.Controllers
{
    [Authorize]
    public class NumeroOficialController : Controller
    {
        #region Membros

        private readonly INumeroOficialAppService _numeroOficialService;
        private readonly IResponsavelAppService _responsavelService;
        private readonly IEntrevistaAppService _entrevistaService;

        public NumeroOficialController(INumeroOficialAppService numeroOficialService,
            IResponsavelAppService responsavelService, IEntrevistaAppService entrevistaService)
        {
            _numeroOficialService = numeroOficialService;
            _responsavelService = responsavelService;
            _entrevistaService = entrevistaService;
        }

        #endregion Membros

        #region Actions

        public ActionResult Editar(int? id)
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


                NumeroOficialDTO numeroOficialDTO;

                var existeNumeroOficial = _numeroOficialService.FindNumeroOficialPorResponsavel(responsavelId.Value).ToList();

                if (existeNumeroOficial.Any())
                {
                    numeroOficialDTO = _numeroOficialService.FindNumeroOficial(existeNumeroOficial.First().Id);
                    id = numeroOficialDTO.Id;
                }
                //else
                //{
                //    numeroOficialDTO = new NumeroOficialDTO { ResponsavelId = responsavelId.Value };
                //}

                if (!id.HasValue || id == 0 && responsavelId > 0)
                {
                    var socio = _entrevistaService.FindEntrevistaPorResponsavel(responsavelId.Value).First().ListaDeSocios.ToList().First();
                    var entrevista = _entrevistaService.FindEntrevistaPorResponsavel(responsavelId.Value).First();

                    numeroOficialDTO = new NumeroOficialDTO
                    {
                        Requerente = socio.Nome,
                        Rg = socio.Rg,
                        Telefone = socio.Telefone,
                        Rua = socio.Rua,
                        Iptu = entrevista.Iptu,
                        PossuiIptu = true,
                        ResponsavelId = responsavelId.Value
                    };
                }
                else
                {
                    numeroOficialDTO = _numeroOficialService.FindNumeroOficial(id.Value);
                }

                return View(numeroOficialDTO);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost, ValidateAntiForgeryToken()]
        public ActionResult POSTEditar(NumeroOficialDTO numeroOficialDTO)
        {
            try
            {
                if (numeroOficialDTO.Id == 0)
                {
                    numeroOficialDTO = _numeroOficialService.AddNumeroOficial(numeroOficialDTO);
                }
                else
                {
                    _numeroOficialService.UpdateNumeroOficial(numeroOficialDTO);
                }

                return JavaScript(
                    "MensagemSucesso('O número oficial foi salvo com sucesso.');" +
                    "carregarPaginaAjax('" + Url.Action("Index", "NumeroOficial") + "');");
            }
            catch (Exception ex)
            {
                TratamentoErro.Tratamento(this, ex);

                return View("Editar", numeroOficialDTO);
            }
        }

        public ActionResult Excluir(int id)
        {
            try
            {
                _numeroOficialService.RemoveNumeroOficial(id);
                return JavaScript(
                    "MensagemSucesso('O número oficial excluído com sucesso!');" +
                    "carregarPaginaAjax('" + Url.Action("Index", "NumeroOficial") + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MensagemErro('" + ex.Message + "');");
            }
        }

        #endregion Actions
    }
}