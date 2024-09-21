using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace PM.WEB.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IEnumerable<SelectListItem> EnumToSelectList<TEnum, TModel>(this IHtmlHelper<TModel> htmlHelper) where TEnum : struct, Enum
        {
            var model = htmlHelper.ViewData.Model;

            // Use reflection to get the Status property
            var selectedValue = model?.GetType().GetProperty("Status")?.GetValue(model)?.ToString();

            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString().Replace("_", " "),
                    Selected = e.ToString() == selectedValue
                });
        }


        private static string GetString(this TagBuilder tagBuilder)
        {
            using (var writer = new System.IO.StringWriter())
            {
                tagBuilder.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}