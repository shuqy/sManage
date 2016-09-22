using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Util
{
    public class StringHelper
    {
        private static readonly RegexOptions options = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase;
        /// <summary>
        /// 过滤HTML标签，返回需要长度字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string LongString(string str, int length = 0)
        {
            string rstr = str;
            rstr = Regex.Replace(rstr, @"<[^>]+>", "");
            rstr = Regex.Replace(rstr, "\n", "");
            rstr = Regex.Replace(rstr, " ", "");
            rstr = Regex.Replace(rstr, "　", "");
            rstr = Regex.Replace(rstr, @"&#\d+;", "");
            rstr = Regex.Replace(rstr, "&nbsp;", "");
            if (rstr.Length > length && length > 0)
            {
                return rstr.Substring(0, length) + "...";
            }
            else
            {
                return rstr.ToString();
            }
        }

        /// <summary>
        /// 过滤只保留中文标签，返回需要长度字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string LongChinaString(string str, int length = 0)
        {
            string rstr = str;
            rstr = Regex.Replace(rstr, "[^\u4e00-\u9fa5]+", "");
            if (rstr.Length > length && length > 0)
            {
                return rstr.Substring(0, length) + "...";
            }
            else
            {
                return rstr.ToString();
            }
        }
        /// <summary>
        /// 返回需要长度字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SubString(string str, int length = 0)
        {
            string rstr = str;
            if (rstr.Length > length && length > 0)
            {
                return rstr.Substring(0, length);
            }
            else
            {
                return str;
            }
        }
        /// <summary>
        /// 过滤HTML标签
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string FilterHtmlString(string str)
        {
            string rstr = str;
            rstr = Regex.Replace(rstr, @"<[^>]+>", "");
            rstr = Regex.Replace(rstr, "\n", "");
            rstr = Regex.Replace(rstr, " ", "");
            rstr = Regex.Replace(rstr, "　", "");
            rstr = Regex.Replace(rstr, @"&#\d+;", "");
            rstr = Regex.Replace(rstr, "&nbsp;", "");
            return rstr;
        }
        /// <summary>        
        /// 取得HTML中所有图片的 URL。        
        /// </summary>        
        /// <param name="sHtmlText">HTML代码</param>        
        /// <returns>图片的URL列表</returns>        
        public static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签            
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", options);// 搜索匹配的字符串            
            MatchCollection matches = regImg.Matches(sHtmlText);
            List<string> arrayList = new List<string>();
            foreach (Match match in matches)
                arrayList.Add(match.Groups["imgUrl"].Value);
            return arrayList.ToArray();
        }
        /// <summary>
        /// 根据网址获得域名部分 
        /// </summary>
        /// <param name="url">http://xxx.xxx.xxx/x/x/x/x或xxx.xxx.xxx/x/x/x/x</param>
        /// <returns>http://xxx.xxx.xxx</returns>
        public static string regexdom(string url)
        {
            return "http://" + url.Replace("http://", "").Split('/')[0];
        }
        /// <summary>
        /// 获得绝对路径的父级路径
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static string parentPath(string Path)
        {
            return Path.Substring(0, Path.Substring(0, Path.Length - 1).LastIndexOf("\\") + 1);
        }
        /// <summary>
        /// 浮点型数据化简
        /// </summary>
        /// <param name="Price"></param>
        /// <returns></returns>
        public static string GetPriceSimplify(string Price)
        {
            return string.Format("{0:N2}", Convert.ToSingle(Price)).Replace(".00", "");
        }
        /// <summary>
        /// 手机号码隐藏中间4位
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public static string GetPhoneEncrypt(string Phone)
        {
            string rePhone = Phone.Trim();
            if (rePhone.Length == 11)
            {
                rePhone = rePhone.Substring(0, 3) + "******" + rePhone.Substring(9);
            }
            return rePhone;
        }
        /// <summary>
        /// 电话号码隐藏最后4位
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public static string GetPhonesEncrypt(string Phone)
        {
            string rePhone = Phone.Trim();
            if (rePhone != "")
            {
                rePhone = rePhone.Replace(rePhone.Substring(rePhone.Length - 4, 4), "****待审核");
            }
            return rePhone;
        }

        /// <summary>
        /// 邮箱号隐藏用户名
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string GetEmailEncrypt(string email)
        {
            string reEmail = email;
            string[] array = reEmail.Split('@');
            if (array.Length > 1)
            {
                if (array[0].Length > 4)
                {
                    reEmail = reEmail.Substring(0, 4) + "******@" + array[1];
                }
                else
                {
                    reEmail = reEmail.Substring(0, 1) + "******@" + array[1];
                }
            }
            return reEmail;
        }
        /// <summary>
        /// Url是否合法
        /// </summary>
        /// <param name="url">url地址</param>
        /// <returns>是否合法</returns>
        public static bool UrlIsExists(string url)
        {
            string pattern = @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";
            if (Regex.IsMatch(url, pattern))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断网址内容是否存在
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static bool CheckUrlResponse(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.Timeout = 500;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// UTF8ToURL
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ChEncodeUrl(string str)
        {
            return System.Web.HttpUtility.UrlEncode(str);

        }

        /// <summary>
        ///URLToUTF8
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ChDecodeUrl(string str)
        {
            return System.Web.HttpUtility.UrlDecode(str);
        }

        /// <summary>
        /// 获取请求格式的所有参数
        /// </summary>
        /// <returns>请求参数字符串</returns>
        public static Dictionary<string, string> GetRequestValue(string Context)
        {
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            //对url第一个字符？过滤
            if (!string.IsNullOrEmpty(Context))
            {
                //根据&符号分隔成数组
                string[] coll = Context.Split('&');
                //定义临时数组
                string[] temp = { };
                //循环各数组
                for (int i = 0; i < coll.Length; i++)
                {
                    //根据=号拆分
                    temp = coll[i].Split('=');
                    //把参数名和值分别添加至SortedDictionary数组
                    sArray.Add(temp[0], temp[1]);
                }
            }
            return sArray;
        }
        /// <summary>
        /// 过滤HTML标签(空格不过滤)，返回需要长度字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string LongStringSpace(string str, int length)
        {
            string rstr = str;
            rstr = Regex.Replace(rstr, @"<[^>]+>", "");
            rstr = Regex.Replace(rstr, "\n", "");
            rstr = Regex.Replace(rstr, @"&#\d+", "");
            rstr = Regex.Replace(rstr, @"&nbsp;", "");
            if (rstr.Length > length)
            {
                return rstr.Substring(0, length) + "...";
            }
            else
            {
                return rstr.ToString();
            }
        }
        /// <summary>
        /// 过滤HTML标签(空格不过滤)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string FilterHtmlStringSpace(string str)
        {
            string rstr = str;
            rstr = Regex.Replace(rstr, @"<[^>]+>", "");
            rstr = Regex.Replace(rstr, "\n", "");
            rstr = Regex.Replace(rstr, @"&#\d+", "");
            rstr = Regex.Replace(rstr, @"&nbsp;", "");
            return rstr;
        }
    }
}
