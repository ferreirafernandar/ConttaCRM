using Arquitetura.Aplicacao.Services.Interface;
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
    public class DashboardController : Controller
    {
        #region Membros

        readonly INumeroOficialAppService _usuarioService;

        public DashboardController(INumeroOficialAppService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        #endregion

        #region Actions

        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        #endregion
    }
}
