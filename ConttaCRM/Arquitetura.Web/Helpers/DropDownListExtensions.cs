using Arquitetura.Dominio.Aggregates.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

using System.Web.Mvc.Html;

namespace Arquitetura.Web.Helpers
{
    public static class DropDownListExtensions
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

        #region EnumDropDownList

        public static MvcHtmlString EnumDropDownList(this HtmlHelper htmlHelper, string name, Type enumType)
        {
            return EnumDropDownList(htmlHelper, name, enumType, null, null);
        }

        public static MvcHtmlString EnumDropDownList(this HtmlHelper htmlHelper, string name, Type enumType, object htmlAttributes)
        {
            return EnumDropDownList(htmlHelper, name, enumType, null, htmlAttributes);
        }

        public static MvcHtmlString EnumDropDownList(this HtmlHelper htmlHelper, string name, Type enumType, string optionalLabel)
        {
            return EnumDropDownList(htmlHelper, name, enumType, optionalLabel, null);
        }

        public static MvcHtmlString EnumDropDownList(this HtmlHelper htmlHelper, string name, Type enumType, string optionalLabel, object htmlAttributes)
        {
            var dic = new Dictionary<short, string>();
            foreach (var item in Enum.GetValues(enumType))
            {
                int xx = (int)item;
                dic.Add((short)xx, Util.GetEnumDescription(Enum.GetName(enumType, item), enumType));
            }
            var items = new SelectList(dic, "Key", "Value");

            return htmlHelper.DropDownList(name, items, optionalLabel, htmlAttributes);
        }
        
        #endregion

        #region EnumDropDownListFor

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            return EnumDropDownListFor(htmlHelper, expression, null, null);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
        {
            return EnumDropDownListFor(htmlHelper, expression, null, htmlAttributes);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, string optionalLabel)
        {
            return EnumDropDownListFor(htmlHelper, expression, optionalLabel, null);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, string optionalLabel, object htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = GetNonNullableModelType(metadata);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

            IEnumerable<SelectListItem> items = from value in values
                                                select new SelectListItem
                                                {
                                                    Text = Util.GetEnumDescription(value),
                                                    Value = value.ToString(),
                                                    Selected = value.Equals(metadata.Model)
                                                };

            // If the enum is nullable, add an 'empty' item to the collection
            if (optionalLabel != null)
            {
                var optional = new[] { new SelectListItem { Text = optionalLabel, Value = "" } };
                items = optional.Concat(items);
            }
            else if (metadata.IsNullableValueType)
            {
                items = SingleEmptyItem.Concat(items);
            }

            return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
        }

        #endregion
    }
}