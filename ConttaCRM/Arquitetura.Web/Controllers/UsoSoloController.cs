using Arquitetura.Aplicacao.Services.Interface;
using Arquitetura.DTO;
using Arquitetura.Web.Helpers;
using System;
using System.Web.Mvc;
using System.Linq;

namespace Arquitetura.Web.Controllers
{
    [Authorize]
    public class UsoSoloController : Controller
    {
        #region Membros

        private readonly IUsoSoloAppService _usoSoloService;
        private readonly IResponsavelAppService _responsavelService;
        private readonly IEntrevistaAppService _entrevistaService;

        public object usoSoloDTODTO { get; private set; }

        public UsoSoloController(IUsoSoloAppService usoSoloService,
            IResponsavelAppService responsavelService, IEntrevistaAppService entrevistaService)
        {
            _usoSoloService = usoSoloService;
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


               UsoSoloDTO usoSoloDTO;

                var existeUsoSolo = _usoSoloService.FindUsoSoloPorResponsavel(responsavelId.Value).ToList();

                if (existeUsoSolo.Any())
                {
                    usoSoloDTO = _usoSoloService.FindUsoSolo(existeUsoSolo.First().Id);
                    id = usoSoloDTO.Id;
                }
                //else
                //{
                //    numeroOficialDTO = new NumeroOficialDTO { ResponsavelId = responsavelId.Value };
                //}

                if (!id.HasValue || id == 0 && responsavelId > 0)
                {
                    var socio = _entrevistaService.FindEntrevistaPorResponsavel(responsavelId.Value).First().ListaDeSocios.ToList().First();
                    var entrevista = _entrevistaService.FindEntrevistaPorResponsavel(responsavelId.Value).First();

                    usoSoloDTO = new UsoSoloDTO
                    {
                        Rua = socio.Rua,
                        Iptu = entrevista.Iptu,
                        ResponsavelId = responsavelId.Value
                    };
                }
                else
                {
                    usoSoloDTO = _usoSoloService.FindUsoSolo(id.Value);
                }

                return View(usoSoloDTO);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost, ValidateAntiForgeryToken()]
        public ActionResult POSTEditar(UsoSoloDTO usoSoloDTO)
        {
            try
            {
                if (usoSoloDTO.Id == 0)
                {
                    usoSoloDTO = _usoSoloService.AddUsoSolo(usoSoloDTO);
                }
                else
                {
                    _usoSoloService.UpdateUsoSolo(usoSoloDTO);
                }

                return JavaScript(
                    "MensagemSucesso('O uso de solo foi salvo com sucesso.');" +
                    "carregarPaginaAjax('" + Url.Action("Index", "UsoSolo") + "');");
            }
            catch (Exception ex)
            {
                TratamentoErro.Tratamento(this, ex);

                return View("Editar", usoSoloDTO);
            }
        }

        public ActionResult Excluir(int id)
        {
            try
            {
                _usoSoloService.RemoveUsoSolo(id);
                return JavaScript(
                    "MensagemSucesso('O uso de solo foi excluído com sucesso!');" +
                    "carregarPaginaAjax('" + Url.Action("Index", "UsoSolo") + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MensagemErro('" + ex.Message + "');");
            }
        }

        #endregion Actions
    }
}