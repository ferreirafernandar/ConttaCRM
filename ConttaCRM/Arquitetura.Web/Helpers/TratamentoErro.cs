using Arquitetura.Aplicacao.Base;
using System;
using System.Web.Mvc;

namespace Arquitetura.Web.Helpers
{
    public static class TratamentoErro
    {
        public static void Tratamento(this Controller controller, Exception ex, bool clearModelState = true)
        {
            if (clearModelState)
            {
                controller.ModelState.Clear();
            }

            var appException = ex as AppException;
            if (appException != null && appException.ValidationErrors != null)
            {
                foreach (var erro in appException.ValidationErrors)
                    controller.ModelState.AddModelError(erro.MemberNames, erro.ErrorMessage);
            }

            controller.ViewBag.AlertError = ex.Message;
        }
    }
}