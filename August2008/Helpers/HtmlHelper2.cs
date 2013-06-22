using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace August2008.Helpers
{
    public static class HtmlHelper2 
    {
        public static IHtmlString LabelFor2<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return LabelHelper(html, 
                ModelMetadata.FromLambdaExpression(expression, html.ViewData), 
                ExpressionHelper.GetExpressionText(expression), "");
        }
        private static IHtmlString LabelHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName, string labelText)
        {
            if (string.IsNullOrEmpty(labelText))
            {
                labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            }
            if (string.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }
            var isRequired = false;
            if (metadata.ContainerType != null && metadata.PropertyName != null)
            {
                isRequired = metadata.ContainerType.GetProperty(metadata.PropertyName) 
                    .GetCustomAttributes(typeof (RequiredAttribute), false)
                    .Length == 1;
            }
            var tag = new TagBuilder("label");
            tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            if (isRequired)
            {
                tag.Attributes.Add("class", "label-required");
            }
            tag.SetInnerText(labelText);
            var output = tag.ToString(TagRenderMode.Normal);
            if (isRequired)
            {
                var asterisk = new TagBuilder("span");
                asterisk.Attributes.Add("class", "required");
                asterisk.SetInnerText("*");
                output += asterisk.ToString(TagRenderMode.Normal);
            }
            return MvcHtmlString.Create(output);
        }
    }
}