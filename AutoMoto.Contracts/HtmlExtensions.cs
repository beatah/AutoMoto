using System;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace AutoMoto.Contracts
{
    public static class HtmlExtensions
    {
        public static string GetDisplayForStatus(this bool status)
        {
            return status ? "Aktywny" : "Nieaktywny";
        }

        public static string UrlReplace(this HttpRequestBase request, string key, string value)
        {
            NameValueCollection nv = new NameValueCollection(request.QueryString);
            nv.Set(key, value);

            UriBuilder u = new UriBuilder(request.Url);
            u.Query = ToQueryString(nv);

            return u.Uri.ToString();
        }
        public static string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
                .ToArray();
            return string.Join("&", array);
        }
        /// <summary>
        /// Renders checkbox as ont input (normal Html.CheckBoxFor renders two inputs: checkbox and hidden)
        /// </summary>
        public static MvcHtmlString BasicCheckBoxFor<T>(this HtmlHelper<T> html, Expression<Func<T, bool>> expression, object htmlAttributes = null)
        {
            var tag = new TagBuilder("input");

            tag.Attributes["type"] = "checkbox";
            tag.Attributes["id"] = html.IdFor(expression).ToString();
            tag.Attributes["name"] = html.NameFor(expression).ToString();
            tag.Attributes["value"] = "true";

            // set the "checked" attribute if true
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            if (metadata.Model != null)
            {
                bool modelChecked;
                if (Boolean.TryParse(metadata.Model.ToString(), out modelChecked))
                {
                    if (modelChecked)
                    {
                        tag.Attributes["checked"] = "checked";
                    }
                }
            }

            // merge custom attributes
            tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            var tagString = tag.ToString(TagRenderMode.SelfClosing);
            return MvcHtmlString.Create(tagString);
        }



    }
}
