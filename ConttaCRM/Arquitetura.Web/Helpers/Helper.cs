using Arquitetura.Aplicacao.Base;
using Arquitetura.Infraestrutura.Util;
using Arquitetura.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Arquitetura.Web.Helpers
{
    public static class Helper
    {
        //public static void AdicionarMensagem(string mensagem, eTipoMensagem tipo, Controller controller)
        //{
        //    List<Mensagem> listaMensagens = new List<Mensagem>();

        //    if (controller.ViewBag.ListaMensgens != null)
        //    {
        //        listaMensagens.AddRange((List<Mensagem>)controller.ViewBag.ListaMensgens);
        //    }
        //    else if (controller.TempData["ListaMensgens"] != null)
        //    {
        //        listaMensagens.AddRange((List<Mensagem>)controller.TempData["ListaMensgens"]);
        //    }

        //    listaMensagens.Add(new Mensagem { Texto = mensagem, TipoMensagem = tipo });
        //    controller.ViewBag.ListaMensgens = listaMensagens;
        //    controller.TempData["ListaMensgens"] = listaMensagens;
        //}

        //public static List<Mensagem> ObtenhaMensagens(Controller controller)
        //{
        //    List<Mensagem> listaMensagens = null;

        //    if (controller.ViewBag.ListaMensgens != null)
        //    {
        //        listaMensagens = (List<Mensagem>)controller.ViewBag.ListaMensgens;
        //    }
        //    else if (controller.TempData["ListaMensgens"] != null)
        //    {
        //        listaMensagens = (List<Mensagem>)controller.TempData["ListaMensgens"];
        //    }

        //    controller.ViewBag.ListaMensgens = null;
        //    controller.TempData["ListaMensgens"] = null;

        //    return listaMensagens;
        //}

        //public static void TratarExcecao(Exception ex, Controller controller)
        //{
        //    controller.ModelState.Clear();
        //    AppException avex = ex as AppException;
        //    if (avex != null && avex.ValidationErrors != null)
        //    {
        //        foreach (var item in avex.ValidationErrors)
        //            controller.ModelState.AddModelError(item.MemberNames, item.ErrorMessage);
        //    }

        //    AdicionarMensagem(ex.Message, eTipoMensagem.Error, controller);
        //}

        /*
        public static void TratarMensagem(Controller controller)
        {
            controller.ViewBag.Error = controller.Request["error"];
            controller.ViewBag.Danger = controller.Request["danger"];
            controller.ViewBag.Info = controller.Request["info"];
            controller.ViewBag.Success = controller.Request["success"];
        }
        */

        public static void SendEmail(
            string subject,
            string message,
            string contaEmail,
            string contaSenha,
            string contaSmtp,
            int contaPorta,
            bool ssl,
            string emailDestinatario,
            List<HttpPostedFileBase> files = null)
        {
            List<string> destinatarios = new List<string>();
            if (!string.IsNullOrWhiteSpace(emailDestinatario))
                destinatarios.Add(emailDestinatario);

            SendEmail(subject, 
                message, 
                contaEmail,
                contaSenha,
                contaSmtp,
                contaPorta,
                ssl,
                destinatarios, 
                files);
        }

        public static void SendEmail(
            string subject, 
            string message, 
            string contaEmail,
            string contaSenha,
            string contaSmtp,
            int contaPorta,
            bool ssl,
            List<string> destinatarios, 
            List<HttpPostedFileBase> files = null)
        {
            var loginInfo = new NetworkCredential(contaEmail, contaSenha);
            var msg = new System.Net.Mail.MailMessage();
            var smtpClient = new SmtpClient(contaSmtp, contaPorta);

            msg.From = new MailAddress(contaEmail);
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            if (destinatarios == null || destinatarios.Count == 0)
            {
                throw new Exception("Nenhum destinatário informado.");
            }
            else
            {
                foreach (var destinatario in destinatarios)
                {
                    if (Util.IsValidEmail(destinatario))
                        msg.To.Add(new MailAddress(destinatario));
                }
            }

            if (files != null)
            {
                foreach (var file in files)
                {
                    Attachment anexo = new Attachment(file.InputStream, file.FileName);
                    msg.Attachments.Add(anexo);
                }
            }

            smtpClient.EnableSsl = ssl;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }
    }

    public static class HtmlDropDownExtensions
    {
        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            Type realModelType = modelMetadata.ModelType;

            Type underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }
            return realModelType;
        }

        private static readonly SelectListItem[] SingleEmptyItem = new[] { new SelectListItem { Text = "", Value = "" } };

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, ViewDataDictionary viewData)
        {
            return EnumDropDownListFor(htmlHelper, expression, null, viewData);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes, ViewDataDictionary viewData)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = GetNonNullableModelType(metadata);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

            IEnumerable<SelectListItem> items = from value in values
                                                select new SelectListItem
                                                {
                                                    Text = Util.GetEnumDescription(value),
                                                    Value = ((int)Enum.Parse(GetNonNullableModelType(metadata), value.ToString())).ToString(),
                                                    Selected = value.Equals(metadata.Model)
                                                };

            if (metadata.IsNullableValueType)
                items = SingleEmptyItem.Concat(items);

            viewData[metadata.PropertyName] = items; 

            return htmlHelper.DropDownListFor(expression, null, htmlAttributes);
        }
    }
}