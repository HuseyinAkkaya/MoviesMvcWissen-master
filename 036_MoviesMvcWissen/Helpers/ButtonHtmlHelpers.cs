using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData.ModelProviders;
using System.Web.Mvc;

namespace _036_MoviesMvcWissen.Helpers
{
    public static class ButtonHtmlHelpers
    {/// <summary>
     /// Buton oluşturur.
     /// </summary>
     /// <param name="htmlHelper"></param>
     /// <param name="text">Buton üzerinde görünecek yazı </param>
     /// <param name="attributes">exc. new {@class= "[class name]",type="Submit" || "Reset" ,id="[id]" }</param>
     /// <returns>MvcHtmlString tipinde veri</returns>
        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string text, object attributes = null)
        {
            TagBuilder tagBuilder = new TagBuilder("button");
            tagBuilder.InnerHtml = text;
            tagBuilder.MergeAttributes((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
            return MvcHtmlString.Create(tagBuilder.ToString());

        }

       

    }
}