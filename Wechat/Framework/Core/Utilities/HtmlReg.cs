using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Utilities {
    public class HtmlReg {
        public HtmlReg() {
        }
        /// <summary>
        /// 根据开始内容和结束标记（单字母）获取内容
        /// 返回结果带开始标记和结束标记
        /// </summary>
        /// <param name="startWith"></param>
        /// <param name="endWith"></param>
        /// <returns></returns>
        public static string FindWithBeginAndEnd(string htmlContent, string startWith, string endWith) {
            Regex creg = new Regex(string.Format("{0}(?<content>[^{1}]*)", startWith, endWith));
            string res = creg.Match(htmlContent).Groups["content"].Value;
            return string.Format("{0}{1}{2}", startWith, res, endWith);
        }

        /// <summary>
        /// 根据开始内容和结束标记（单字母）获取内容
        /// 返回结果不带开始标记和结束标记
        /// </summary>
        /// <param name="startWith"></param>
        /// <param name="endWith"></param>
        /// <returns></returns>
        public static string Find(string htmlContent, string startWith, string endWith) {
            Regex creg = new Regex(string.Format("{0}(?<content>[^{1}]*)", startWith, endWith));
            string res = creg.Match(htmlContent).Groups["content"].Value;
            return string.IsNullOrEmpty(res) ? "0" : res;
        }

        /// <summary>
        /// 根据开始内容和结束内容获取内容
        /// 返回结果不带开始标记和结束标记
        /// </summary>
        /// <param name="startWith"></param>
        /// <param name="endWith"></param>
        /// <returns></returns>
        public static string FindContent(string html, string startWith, string endWith) {
            Regex creg = new Regex(string.Format("{0}(?<content>[\\s\\S]*?){1}", startWith, endWith));
            string res = creg.Match(html).Groups["content"].Value;
            return res;
        }

        public static List<string> FindList(string htmlContent, string startWith, string endWith) {
            Regex creg = new Regex(string.Format("{0}(?<content>[\\s\\S]*?){1}", startWith, endWith));
            var res = creg.Match(htmlContent);
            List<string> clist = new List<string>();
            while (res.Success) {
                clist.Add(res.Groups["content"].Value);
                res = res.NextMatch();
            }
            return clist;
        }

        public static List<string> FindListT(string htmlContent, string startWith, string endWith) {
            Regex creg = new Regex(string.Format("{0}(?<content>[^{1}]*)", startWith, endWith));
            var res = creg.Match(htmlContent);
            List<string> clist = new List<string>();
            while (res.Success) {
                clist.Add(res.Groups["content"].Value);
                res = res.NextMatch();
            }
            return clist;
        }

        public static List<ImgData> FindImgList(string htmlContent) {
            Regex imglinkreg = new Regex("<img.*?(?<content>[^>]*)");
            Regex imgurlreg = new Regex("src=\"(?<src>[^\"]*)");
            var res = imglinkreg.Match(htmlContent);
            List<ImgData> clist = new List<ImgData>();
            while (res.Success) {
                string imgcontent = "<img" + res.Groups["content"].Value + ">";
                string src = imgurlreg.Match(imgcontent).Groups["src"].ToString();
                string realPath = WebUtility.UrlDecode(src);
                if (realPath.StartsWith("//")) realPath = realPath.Replace("//", "http://www.");
                ImgData data = new ImgData {
                    oldimg = imgcontent,
                    src = src,
                    name = RandomOperate.RansdomName(10),
                };
                data.realPath = realPath;
                data.url = "/Img/" + data.name + ".jpg";
                data.path = ConfigHelper.Get("ImgPath") + data.name + ".jpg";
                clist.Add(data);
                res = res.NextMatch();
            }
            return clist;
        }


        public static string FindLinkTitle(string link) {
            Regex treg = new Regex(string.Format("<a.*?>(?<title>[\\s\\S]*?)</a>"));
            var t = treg.Match(link);
            if (t.Success)
                return t.Groups["title"].Value;
            return "";

        }

        /// <summary>
        /// 去掉简介中链接和图片和换行
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveLink(string content) {
            //Regex reg = new Regex(@"(?is)</?a\b[^>]*>(?:(?!</?a).)*</a>");
            //Regex imgreg = new Regex(@"<img[^>]*>");
            //string result = reg.Replace(content, "");
            //result = imgreg.Replace(result, "");>");
            Regex tagreg = new Regex(@"<[^>]*>");
            string result = tagreg.Replace(content, "");
            result = result.Replace("\n", "");
            result = result.Replace("-----", "-");
            return result;
        }

        public static string ChangeWidth(string content) {
            Regex widthreg = new Regex("width=\"[^\"]*\"");
            string result = widthreg.Replace(content, "width=\"100%\"");
            result = result.Replace("----------------", "");
            result = result.Replace("================", "");
            result = result.Replace("~~~~~~~~~~~~~~~~", "");
            return result;
        }

        public static string RemoveEditDateLink(string content) {
            Regex reg = new Regex("<a class=\"answer-date-link.*?</a>");
            Regex datareg = new Regex("<a class=\"answer-date-link.*?>(?<data>[^<]*)");
            var updatematch = datareg.Match(content);
            if (updatematch.Success) {
                var data = updatematch.Groups["data"].Value;
                content = reg.Replace(content, data);
            }
            return content;
        }
    }
}
