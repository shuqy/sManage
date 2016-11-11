using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Util
{
    public class StockHelper
    {
        /// <summary>
        /// 获取股票实时数据
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public static SinaStockData GetCurStockData(string stockCode)
        {
            HttpHelper hh = new HttpHelper();
            string url = "http://hq.sinajs.cn";
            string curCode = stockCode == "000000" ? "sh000001" : stockCode == "000001" ? "sz000001" : "sh" + stockCode;
            HttpItem item = new HttpItem
            {
                URL = url + "?list=" + curCode,
                Method = "get",
            };
            HttpResult result = hh.GetHtml(item);
            if (result.Html.Contains("FAILED") || result.Html.Contains("\"\""))
            {
                curCode = "sz" + stockCode;
                item.URL = url + "?list=" + curCode;
                result = hh.GetHtml(item);
                if (result.Html.Contains("FAILED") || result.Html.Contains("\"\"")) return null;
            }
            Regex conreg = new Regex(@"(?<="")([^""]*)");
            string curStr = conreg.Match(result.Html).Value;
            if (curStr == "") return null;
            string[] arr = curStr.Split(',');
            if (!arr.Any()) return null;
            SinaStockData ssd = new SinaStockData();
            ssd.name = arr[0];
            ssd.openningPrice = Convert.ToDecimal(arr[1]).ToString("f2");
            ssd.closingPrice = Convert.ToDecimal(arr[2]).ToString("f2");
            ssd.currentPrice = Convert.ToDecimal(arr[3]).ToString("f2");
            ssd.hPrice = Convert.ToDecimal(arr[4]).ToString("f2");
            ssd.lPrice = Convert.ToDecimal(arr[5]).ToString("f2");
            int tnum = Convert.ToInt32(arr[8]) / 100;
            ssd.increase = ((Convert.ToDecimal(arr[3]) - Convert.ToDecimal(arr[2])) * 100 / Convert.ToDecimal(arr[2])).ToString("f2") + "%";
            ssd.totalNumber = tnum > 10000 ? tnum / 10000 + "万手" : tnum + "手";
            ssd.turnover = (Convert.ToDecimal(arr[9]) / 10000).ToString("f2") + "万元";
            ssd.date = arr[30] + " " + arr[31];
            ssd.chatMin = "http://image.sinajs.cn/newchart/min/n/" + curCode + ".gif";
            return ssd;
        }
    }
}
