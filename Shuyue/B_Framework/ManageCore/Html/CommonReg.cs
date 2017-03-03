using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Html
{
    public class CommonReg
    {
        /// <summary>
        /// meta标签
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static string CommonTagRegStr(string tagName)
        {
            return string.Format(@"<{0}(?<=<{1})(?<{2}>[\s\S]*)(?=</{3}>)</{4}>",
                tagName, tagName, tagName, tagName, tagName);
        }
        /// <summary>
        /// html
        /// </summary>
        public static string AllHtml { get { return CommonTagRegStr("html"); } }
        /// <summary>
        /// head
        /// </summary>
        public static string AllHead { get { return CommonTagRegStr("head"); } }
        /// <summary>
        /// body
        /// </summary>
        public static string AllBody { get { return CommonTagRegStr("body"); } }
        public static string Title { get { return @"(?<=<title*?>)([\s\S]*)(?=</title>)"; } }
        /// <summary>
        /// meta
        /// </summary>
        public static string Meta { get { return @"(?=<meta).*?name.*?""(?<name>[^""]*).*?content.*?""(?<content>[^""]*).*?(?=/>)"; } }
        /// <summary>
        /// styles
        /// </summary>
        public static string Styles { get { return CommonTagRegStr("styels"); } }
        /// <summary>
        /// js
        /// </summary>
        public static string Scripts { get { return CommonTagRegStr("script"); } }
        /// <summary>
        /// 链接
        /// </summary>
        public static string Link { get { return @"(?=<a).*?href=""(?<href>[^""]*).*?>(?<name>[^<]*).*?(?=/a>)/a>"; } }
        /// <summary>
        /// 获取类似json类型数据
        /// </summary>
        public static string LikeJsonParam { get { return @"""(?<name>[^""]*).*?:(?<value>[^]|^,|^}]*)"; } }
        /// <summary>
        /// 微博登录成功获取cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string CookieRegStr(string cookieName)
        {
            return "(?<=" + cookieName + "=)(?<val>[^;]*)";
        }
        /// <summary>
        /// 微博用户信息正则
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public static string WeiboUserRegStr(string entityName)
        {
            return "(?<=" + entityName + "=)(?<val>[^&]*)";
        }

        /// <summary>
        /// 微博内容正则
        /// </summary>
        public static string WeiboBody
        {
            get { return @"{""pid"":""pl_weibo_direct""(?<={""pid"":""pl_weibo_direct"").*?(?<content>[^}]*)}"; }
        }
        /// <summary>
        /// 单条微博内容抓取
        /// </summary>
        public static string WeiboDetials
        {
            get { return @"<!--feed_detail-->(?<content>[\s\S]*?)<!--/feed_detail-->"; }
        }

        public static string Img
        {
            get { return @"(?<=<img.*?src="")(?<src>[^""]*)"; }
        }
        /// <summary>
        /// 微博内容头像部分
        /// </summary>
        public static string WeiboFace { get { return @"<div class=""face"">(?<val>[\s\S]*?)</div>"; } }
        /// <summary>
        /// 微博内容正文部分
        /// </summary>
        public static string WeiboContentHtml { get { return @"<div class=""feed_content wbcon"">(?<val>[\s\S]*?)</div>"; } }
        /// <summary>
        /// 微博内容下边部分
        /// </summary>
        public static string WeiboFooterHtml { get { return @"<div class=""feed_from W_textb"">(?<val>[\s\S]*?)</div>"; } }
        /// <summary>
        /// 微博发布者昵称
        /// </summary>
        public static string WeiboNickName { get { return @"(?<=nick-name="")(?<val>[^""]*)"; } }
        /// <summary>
        /// 微博发布者主页
        /// </summary>
        public static string WeiboAuthorUrl { get { return @"(?<=href="")(?<href>[^""]*)"; } }
        /// <summary>
        /// 微博时间
        /// </summary>
        public static string WeiboPubData { get { return @"(?<=title="")(?<val>[^""]*)"; } }
        /// <summary>
        /// 微博发布自
        /// </summary>
        public static string DeviceType { get { return @"(?<=rel=""nofollow"">)(?<val>[^<]*)"; } }

        /// <summary>
        /// 移除所有尖括号内的内容
        /// </summary>
        public static string RemoveTag { get { return @"<(?<d>[^>]*)>"; } }
        /// <summary>
        /// 移除所有a标签
        /// </summary>
        public static string RemoveLink { get { return @"<a.*?(?<d>[\s\S]*?)</a>"; } }
        /// <summary>
        /// 获取页数todo
        /// </summary>
        public static string PageCount { get { return @"<a.*?>第(?<p>[\d]*?)页</a>"; } }
    }
}
