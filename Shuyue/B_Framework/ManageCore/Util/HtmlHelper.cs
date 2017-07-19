using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace Core.Util
{
    public class HtmlHelper
    {
        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="s">开始</param>
        /// <param name="e">结束</param>
        /// <returns></returns>
        public static string GetValue(string str, string s, string e)
        {
            string regexStr = "";
            if (s != string.Empty && e != string.Empty)
            {
                regexStr = "(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))";
            }
            else
            {
                regexStr = "(?<=(" + s + "))[.\\s\\S]*";
            }
            Regex rg = new Regex(regexStr, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return rg.Match(str).Value;
        }

        /// <summary>
        /// 获得所有字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<string> GetValueList(string str, string begin, string end)
        {
            string regexStr = "(?<=(" + begin + "))[.\\s\\S]*?(?=(" + end + "))";
            Regex rg = new Regex(regexStr, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Match m = rg.Match(str);
            List<string> matchRes = new List<string>();
            while (m.Success)
            {
                matchRes.Add(m.Value);
                m = m.NextMatch();
            }
            return matchRes;
        }
    }
}
