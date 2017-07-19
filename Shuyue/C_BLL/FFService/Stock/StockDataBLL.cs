using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Util;
using Core.Enum;
using Model.ViewModel.ff;
using System.Text.RegularExpressions;

namespace FFService.Stock
{
    public class StockDataBLL
    {
        /// <summary>
        /// 更新所有股票名称代码
        /// </summary>
        public int UpdateStocks()
        {
            string stocksListURL = "http://quote.eastmoney.com/stocklist.html";
            HttpHelper httpHelper = new HttpHelper();
            HttpItem item = new HttpItem
            {
                URL = stocksListURL,
                Method = "get",
            };
            HttpResult result = httpHelper.GetHtml(item);
            string html = "";
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                html = result.Html;
            }
            Regex regex = new Regex("<a target=\"_blank\".*?html\">(?<name>[^<]*)");
            Match match = regex.Match(html);
            List<stocks> stocks = new List<stocks>();
            while (match.Success)
            {
                string s = match.Groups["name"].Value;
                string stockName = s.Substring(0, s.IndexOf('('));
                string stockCode = s.Substring(s.IndexOf('(') + 1, 6);
                match = match.NextMatch();
                if (!stockCode.StartsWith("0") && !stockCode.StartsWith("3") && !stockCode.StartsWith("6")) continue;
                stocks.Add(new stocks
                {
                    name = stockName,
                    code = stockCode,
                    fullPy = PyHelper.Get(stockName),
                    pyAbbre = PyHelper.GetFirst(stockName)
                });
            }
            DapperMySql mysql = new DapperMySql(DbConnEnum.ff);
            var stockList = mysql.Query<stocks>("select * from stocks;");
            stocks = stocks.Where(s => !stockList.Any(r => r.code == s.code)).ToList();
            int count = 0;
            if (stocks.Any())
            {
                count = mysql.InsertMultiple("insert stocks (name,code,fullPy,pyAbbre) values (@name,@code,@fullPy,@pyAbbre);", stocks);
            }
            return count;
        }
    }
}
