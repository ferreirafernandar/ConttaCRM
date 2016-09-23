using Arquitetura.Aplicacao.Services.Interface;
using System.Linq;
using System.Web.Mvc;

namespace Arquitetura.Web.Controllers
{
    [Authorize]
    public class ConstituicaoController : Controller
    {
        private readonly IResponsavelAppService _responsavelService;
        private readonly IEntrevistaAppService _entrevistaService;
        private readonly INumeroOficialAppService _numeroOficialService;
        private readonly IUsoSoloAppService _usoSoloService;

        public ConstituicaoController(IResponsavelAppService responsavelService, IEntrevistaAppService entrevistaService, INumeroOficialAppService numeroOficialService, IUsoSoloAppService usoSoloService)
        {
            _responsavelService = responsavelService;
            _entrevistaService = entrevistaService;
            _numeroOficialService = numeroOficialService;
            _usoSoloService = usoSoloService;
        }

        public ActionResult Index(int? responsavelId)
        {
            //Session["ResponsavelId"] = null;
            if (responsavelId.HasValue)
            {
                Session["ResponsavelId"] = responsavelId.Value;

                var responsavel = _responsavelService.FindResponsavel(responsavelId.Value);

                ViewBag.ResponsavelNome = responsavel.Nome;

                ViewBag.Data = responsavel.DataCadastro.Value.ToString("dd/MM/yyyy H:mm");

                var entrevista = _entrevistaService.FindEntrevistaPorResponsavel(responsavelId.Value);

                if (entrevista.Count > 0)
                {
                    ViewBag.DataEntrevista = entrevista.First().DataCadastro.Value.ToString("dd/MM/yyyy H:mm");
                }

                var numeroOficial = _numeroOficialService.FindNumeroOficialPorResponsavel(responsavelId.Value);

                if (numeroOficial.Count > 0)
                {
                    ViewBag.DataNumeroOficial = numeroOficial.First().DataCadastro.Value.ToString("dd/MM/yyyy H:mm");
                }

                //var usoSolo = _usoSoloService.FindUsoSoloPorResponsavel(responsavelId.Value);

                //if (usoSolo.Count > 0)
                //{
                //    ViewBag.DataUsoSolo = usoSolo.First().DataCadastro.Value.ToString("dd/MM/yyyy H:mm");
                //}
            }

            return View();
        }
    }
}