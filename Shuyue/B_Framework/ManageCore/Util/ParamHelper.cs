using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Util
{
    public static class ParamHelper
    {
        public static T? GetValueOrNull<T>(this string valueAsString) where T : struct
        {
            if (string.IsNullOrEmpty(valueAsString) || valueAsString == "undefined")
                return null;
            return (T)Convert.ChangeType(valueAsString, typeof(T));
        }
        public static T[] GetArrayNoNull<T>(this string valueAsString) where T : struct
        {
            List<T> result = new List<T>();
            if (!string.IsNullOrEmpty(valueAsString))
            {
                string[] array = valueAsString.Split(',');


                foreach (var item in array)
                {
                    var v = item.GetValueOrNull<T>();
                    if (v.HasValue)
                    {
                        result.Add(v.Value);
                    }
                }
            }

            return result.ToArray();
        }


        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName, string Default = "")
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return Default;
            }
            return HttpContext.Current.Request.Form[strName];
        }
        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName, string Default = "")
        {
            if ("".Equals(GetQueryString(strName)))
            {
                return GetFormString(strName, Default);
            }
            else
            {
                return GetQueryString(strName, Default);
            }
        }
        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName, string Default = "")
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return Default;
            }
            return HttpContext.Current.Request.QueryString[strName];
        }
    }
}
