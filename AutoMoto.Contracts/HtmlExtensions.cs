using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

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




    }
}
