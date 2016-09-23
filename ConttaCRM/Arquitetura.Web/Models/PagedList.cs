using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Arquitetura.Web.Models
{
    public class PagedList<T>
    {
        public PagedList()
        {
            Page = 1;
            PageSize = 10;
            Entities = Enumerable.Empty<T>().ToList();
            Adicional = "{}";
            GridId = "grid";
            ExecutarScripts = string.Empty;
        }

        public string ExecutarScripts { get; private set; }

        public bool DesabilitarTotalizador { get; set; }

        public string GridId { get; set; }

        public string Sortdir { get; set; }

        public string SearchTerm { get; set; }

        public string Sort { get; set; }

        public int Page { get; set; }

        public bool HasNext { get { return ((Page * PageSize) < TotalRecords); } }

        public bool HasPrevious { get { return Page > 1; } }

        public IList<T> Entities { get; set; }

        public int NextPage { get { return Page + 1; } }

        public int PreviousPage { get { return Page - 1; } }

        public int TotalRecords { get; set; }

        public int PageSize { get; set; }

        public int TotalPage { get { return (TotalRecords % PageSize > 0) ? (int)(TotalRecords / PageSize) + 1 : (int)(TotalRecords / PageSize); } }

        public string Controller { get; set; }

        public string Action { get; set; }

        public bool SortAsc { get { return (!string.IsNullOrWhiteSpace(Sortdir) && Sortdir == "DESC") ? false : true; } }

        public string Adicional { get; set; }

        public void Parametros(Controller controllerContext, IList<T> entities, int totalRecords)
        {
            Entities = entities;
            TotalRecords = totalRecords;
            controllerContext.ViewBag.SearchTerm = SearchTerm;
            controllerContext.ViewBag.PageSize = PageSize;
        }

        public void Parametros(Controller controllerContext, IList<T> entities, int totalRecords, string action, string controller)
        {
            Entities = entities;
            TotalRecords = totalRecords;
            Action = action;
            Controller = controller;
            controllerContext.ViewBag.SearchTerm = SearchTerm;
        }

        public void AdicioneScriptsParaExecutar(string script)
        {
            if (!string.IsNullOrWhiteSpace(script))
            {
                ExecutarScripts += script;
            }
        }
    }
}