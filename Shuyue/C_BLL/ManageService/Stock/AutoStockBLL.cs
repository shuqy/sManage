using Core.Util;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageService.Stock
{
    public class AutoStockBLL
    {
        HttpHelper _httpHelper = new HttpHelper();
        /// <summary>
        /// 获取单个所有历史数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int UpSingleStockHistory(string code, string beginDate, string endDate)
        {
            string dataUrl = string.Format("http://q.stock.sohu.com/hisHq?code=cn_{0}&start={1}&end={2}&stat=1&order=D&period=d&callback=historySearchHandler&rt=jsonp", code, beginDate, endDate);
            HttpItem item = new HttpItem { URL = dataUrl, };
            HttpResult httpResult = _httpHelper.GetHtml(item);
            string html = httpResult.Html.Replace("historySearchHandler([", "").Replace("])", "");
            StockTotalHistory totalHistory = Newtonsoft.Json.JsonConvert.DeserializeObject<StockTotalHistory>(html);
            List<StockData> stockData = new List<StockData>();
            totalHistory.hq = totalHistory.hq.Reverse().ToArray();
            UpSingleStockData(code, totalHistory.hq);
            return totalHistory.hq.Length;
        }

        /// <summary>
        /// 更新所有股票最新日期数据
        /// </summary>
        /// <returns></returns>
        public bool UpAllStockNewData()
        {
            string dataUrl = string.Format("http://vip.stock.finance.sina.com.cn/quotes_service/api/json_v2.php/Market_Center.getHQNodeData?page=1&num=10000&sort=symbol&asc=1&node=hs_a&symbol=&_s_r_a=init");
            HttpItem item = new HttpItem { URL = dataUrl, };
            HttpResult httpResult = _httpHelper.GetHtml(item);
            List<StockData> stockData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StockData>>(httpResult.Html);
            upAllStockData(Convert.ToDateTime("2017-01-21"), stockData);
            return true;
        }

        /// <summary>
        /// 更新历史交易记录
        /// </summary>
        /// <param name="stockCode"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool UpSingleStockData(string stockCode, string[][] data)
        {
            bool isSuccess = true;
            int runCount = 500;
            //检查表是否存在，不存在则创建
            CreateTableBLL.CreateStockTableOrNot(stockCode);
            int begin = 0, end = data.Length >= runCount ? runCount : data.Length;
            for (int i = 0; i < data.Length / runCount + 1; i++)
            {
                string sql = getSignleStockInsertStr(stockCode, data, begin, end);
                begin = end + 1;
                end = data.Length >= end + runCount ? end + runCount : data.Length - 1;
                isSuccess = isSuccess && SqlHelper.ExecuteNonQuery(ConfigHelper.GetConnStr("StockConn"), CommandType.Text, sql.ToString()) > 0;
            }
            return isSuccess;
        }
        /// <summary>
        /// 获取插入sql语句，分段插入，防止用时过长，导致超时
        /// </summary>
        /// <param name="stockCode">代码</param>
        /// <param name="dt">数据源</param>
        /// <param name="begin">开始索引</param>
        /// <param name="end">结束索引</param>
        /// <returns></returns>
        private string getSignleStockInsertStr(string stockCode, string[][] data, int begin, int end)
        {
            StringBuilder sqlsb = new StringBuilder();
            if (begin == 0)
                sqlsb.Append(string.Format("delete T_TransactionRecord_{0};", stockCode));
            sqlsb.Append(string.Format("insert into T_TransactionRecord_{0} (StockCode,TradingDate,[Open],[High],[Low],[Close],[Rose],[Amplitude],[Hands],[Amount],[Turnover],[VOLAMOUNT])", stockCode));
            for (int i = begin; i < end; i++)
            {
                DateTime tradingDate = Convert.ToDateTime(data[i][0]);
                decimal open = 0, high = 0, low = 0, close = 0, rose = 0, amplitude = 0, amount = 0, turnover = 0;
                int hands = 0, VOLAMOUNT = 0;
                decimal.TryParse(data[i][1], out open);
                decimal.TryParse(data[i][2], out close);
                decimal.TryParse(data[i][4].Replace("%", ""), out rose);
                rose = rose / 100;
                decimal.TryParse(data[i][5], out low);
                decimal.TryParse(data[i][6], out high);
                int.TryParse(data[i][7], out hands);
                decimal.TryParse(data[i][8], out amount);
                amount = amount * 10000;
                decimal.TryParse(data[i][9].Replace("%", ""), out turnover);
                sqlsb.Append(string.Format(" select '{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
                    stockCode, tradingDate.ToString("yyyy-MM-dd HH:mm:ss"), open, high, low, close, rose, amplitude, hands, amount, turnover, VOLAMOUNT));
                if (i != end - 1) sqlsb.Append(" union ");
            }
            return sqlsb.ToString();
        }

        /// <summary>
        /// 更新所有股票最新日数据
        /// </summary>
        /// <param name="tradingDate"></param>
        /// <param name="stockData"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="stockList"></param>
        /// <param name="industryList"></param>
        /// <param name="type"></param>
        private void upAllStockData(DateTime tradingDate, List<StockData> stockData)
        {
            var commonSql = Core.AppContext.Current.SqlHelper(Core.Enum.SqlTypeEnum.Stock);
            //已建表的股票
            var createdStock = commonSql.GetDataList<T_Stock>("IsCreatedTable=1");
            StringBuilder upStockHistorySql = new StringBuilder();
            foreach (var item in stockData)
            {
                if (!createdStock.Any(s => s.StockCode == item.code)) continue;
                //拼接历史交易记录sql
                upStockHistorySql.Append(string.Format("insert into T_TransactionRecord_{0} (StockCode,TradingDate,[Open],[High],[Low],[Close],[Rose],[Hands],[Amount],[Turnover],[Amplitude],[VOLAMOUNT]) values ('{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9},0,0);"
                    , item.code, tradingDate, item.open, item.high, item.low, item.trade, item.changepercent, item.volume, item.amount, item.turnoverratio));
            }
            //更新股票数据
            if (!string.IsNullOrEmpty(upStockHistorySql.ToString()))
                SqlHelper.ExecuteNonQuery(ConfigHelper.GetConnStr("StockConn"), CommandType.Text, upStockHistorySql.ToString());
        }
    }
    /// <summary>
    /// 搜狐数据，单个股票所有历史数据
    /// </summary>
    class StockTotalHistory
    {
        public int status { get; set; }
        public string code { get; set; }
        public string[][] hq { get; set; }
    }

    /// <summary>
    /// 新浪数据，所有股票最新交易数据
    /// </summary>
    class StockData
    {
        /// <summary>
        /// 标识符
        /// </summary>
        public string symbol { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 收盘价
        /// </summary>
        public decimal trade { get; set; }
        /// <summary>
        /// 价格变化
        /// </summary>
        public decimal pricechange { get; set; }
        /// <summary>
        /// 涨跌幅
        /// </summary>
        public decimal changepercent { get; set; }
        /// <summary>
        /// 买价
        /// </summary>
        public decimal buy { get; set; }
        /// <summary>
        /// 卖价
        /// </summary>
        public decimal sell { get; set; }
        /// <summary>
        /// 结算
        /// </summary>
        public decimal settlement { get; set; }
        /// <summary>
        /// 开盘价
        /// </summary>
        public decimal open { get; set; }
        /// <summary>
        /// 最高价
        /// </summary>
        public decimal high { get; set; }
        /// <summary>
        /// 最低价
        /// </summary>
        public decimal low { get; set; }
        /// <summary>
        /// 成交量
        /// </summary>
        public decimal volume { get; set; }
        /// <summary>
        /// 成交金额
        /// </summary>
        public decimal amount { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public decimal time { get; set; }
        public decimal per { get; set; }
        public decimal pb { get; set; }
        public decimal mktcap { get; set; }
        public decimal nmc { get; set; }
        /// <summary>
        /// 换手率
        /// </summary>
        public decimal turnoverratio { get; set; }
    }
}
