using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Util
{
    public class CookieHelper
    {
        public static bool SetCookie(string strName, string strValue, int strDay = 30)
        {
            try
            {
                HttpCookie Cookie = new HttpCookie(strName);
                // Cookie.Domain = HttpContext.Current.Request.Url.Host;
                //Cookie.Expires = DateTime.Now.AddDays(strDay);
                Cookie.Value = strValue;
                System.Web.HttpContext.Current.Response.Cookies.Add(Cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SetCookie(string strName, string strValue)
        {
            try
            {
                HttpCookie Cookie = new HttpCookie(strName);
                //Cookie.Domain = HttpContext.Current.Request.Url.Host;
                Cookie.Value = strValue;
                System.Web.HttpContext.Current.Response.Cookies.Add(Cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Cookie写入
        /// </summary>
        /// <param name="strName">Cookie名称</param>
        /// <param name="KeyName">参数名称</param>
        /// <param name="strValue">参数值</param>
        /// <param name="strDay">存放时间(不传默认30天)</param>
        /// <returns></returns>
        public static bool SetCookie(string strName, string KeyName, string strValue, int strDay = 30)
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.Cookies[strName] != null)
                {
                    HttpCookie Cookie = System.Web.HttpContext.Current.Request.Cookies[strName];
                    //Cookie.Expires = DateTime.Now.AddDays(strDay);
                    //Cookie.Domain = HttpContext.Current.Request.Url.Host;
                    Cookie.Values.Set(KeyName, strValue);
                    System.Web.HttpContext.Current.Response.AppendCookie(Cookie);
                }
                else
                {
                    HttpCookie Cookie = new HttpCookie(strName);
                    //Cookie.Expires = DateTime.Now.AddDays(strDay);
                    //Cookie.Domain = HttpContext.Current.Request.Url.Host;
                    Cookie.Values.Add(KeyName, strValue);
                    System.Web.HttpContext.Current.Response.AppendCookie(Cookie);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GetCookie(string strName)
        {
            HttpCookie Cookie = System.Web.HttpContext.Current.Request.Cookies[strName];
            if (Cookie != null)
            {
                return StringHelper.ChDecodeUrl(Cookie.Value.ToString());
            }
            else
            {
                return "";
            }
        }

        public static string GetCookie(string strName, string KeyName)
        {
            HttpCookie Cookie = System.Web.HttpContext.Current.Request.Cookies[strName];
            if (Cookie != null)
            {
                return StringHelper.ChDecodeUrl(Cookie[KeyName]);
            }
            else
            {
                return "";
            }
        }

        public static bool DelCookie(string strName)
        {
            try
            {
                HttpCookie Cookie = new HttpCookie(strName);
                //Cookie.Domain = HttpContext.Current.Request.Url.Host;
                Cookie.Expires = DateTime.Now.AddDays(-1);
                System.Web.HttpContext.Current.Response.Cookies.Add(Cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DelCookieNoHost(string strName)
        {
            try
            {
                HttpCookie Cookie = new HttpCookie(strName);
                Cookie.Expires = DateTime.Now.AddDays(-1);
                System.Web.HttpContext.Current.Response.Cookies.Add(Cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 写入cookie,过期时间用秒
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        /// <param name="strSec"></param>
        /// <returns></returns>
        public static bool SetCookieBySec(string strName, string strValue, int strSec)
        {
            try
            {
                HttpCookie Cookie = new HttpCookie(strName);
                Cookie.Expires = DateTime.Now.AddSeconds(strSec);
                Cookie.Value = strValue;
                System.Web.HttpContext.Current.Response.Cookies.Add(Cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
