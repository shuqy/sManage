using Core.Util;
using Core.Utilities;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageService.Stock
{
    public class StockBLL
    {
        public int UpAllStockData(DataTable dt, DateTime tradingDate)
        {
            //每次执行条数
            int runCount = 500;
            var commonSql = Core.AppContext.Current.SqlHelper(Core.Enum.SqlTypeEnum.Stock);
            //更新行业数据
            UpdateIndustry(dt);
            List<T_Industry> industryList = commonSql.GetDataList<T_Industry>();
            List<T_Stock> stockList = commonSql.GetDataList<T_Stock>();
            int begin = 0, end = dt.Rows.Count >= runCount ? runCount : dt.Rows.Count;
            for (int i = 0; i < dt.Rows.Count / runCount + 1; i++)
            {
                begin = end + 1;
                upAllStockData(tradingDate, dt, begin, end, stockList, industryList);
                end = dt.Rows.Count >= end + runCount ? end + runCount : dt.Rows.Count - 1;
            }
            return 0;
        }

        private void upAllStockData(DateTime tradingDate, DataTable dt, int begin, int end, List<T_Stock> stockList, List<T_Industry> industryList)
        {
            var commonSql = Core.AppContext.Current.SqlHelper(Core.Enum.SqlTypeEnum.Stock);
            //股票信息添加列表、股票信息更新列表
            List<T_Stock> addStockList = new List<T_Stock>(), upStockList = new List<T_Stock>();
            //存储sql字符串
            StringBuilder sqlsb = new StringBuilder(), addStockSql = new StringBuilder(), upStockSql = new StringBuilder(), upStockHistorySql = new StringBuilder();
            for (int i = begin; i <= end; i++)
            {
                //open 开，high 最高，low 最低，close 收，rose 涨幅，amplitude 振幅，amount 金额，turnover 换手率，mainCount 主力净量，earning 市盈，peRatio 市盈率
                decimal open = 0, high = 0, low = 0, close = 0, rose = 0, amplitude = 0, amount = 0, turnover = 0, mainCount = 0, earning = 0, peRatio = 0;
                //hands 手数，VOLAMOUNT 成交次数
                int hands = 0, VOLAMOUNT = 0;
                //totalAmount 总金额，totalMarketValue 总市值，circulationMarketValue 流通市值
                decimal totalAmount = 0, totalMarketValue = 0, circulationMarketValue = 0;
                //赋值
                string fullCode = dt.Rows[i][0].ToString();
                string stockCode = fullCode.Substring(2);
                string stockName = dt.Rows[i][1].ToString();
                decimal.TryParse(dt.Rows[i][2].ToString(), out rose);
                decimal.TryParse(dt.Rows[i][3].ToString(), out close);
                decimal.TryParse(dt.Rows[i][5].ToString(), out turnover);
                decimal.TryParse(dt.Rows[i][6].ToString(), out amplitude);
                decimal.TryParse(dt.Rows[i][8].ToString(), out totalAmount);
                decimal.TryParse(dt.Rows[i][9].ToString(), out totalMarketValue);
                decimal.TryParse(dt.Rows[i][10].ToString(), out circulationMarketValue);
                int.TryParse(dt.Rows[i][11].ToString(), out hands);
                decimal.TryParse(dt.Rows[i][14].ToString(), out mainCount);
                decimal.TryParse(dt.Rows[i][15].ToString(), out earning);
                decimal.TryParse(dt.Rows[i][16].ToString(), out peRatio);
                decimal.TryParse(dt.Rows[i][18].ToString(), out open);
                decimal.TryParse(dt.Rows[i][19].ToString(), out close);
                decimal.TryParse(dt.Rows[i][20].ToString(), out high);
                decimal.TryParse(dt.Rows[i][21].ToString(), out low);
                string industry = dt.Rows[i][32].ToString();//行业
                //判断股票是否新增或有数据更改
                var currentStock = stockList.FirstOrDefault(s => s.StockCode == stockCode);
                if (currentStock == null || currentStock.StockName != stockName || currentStock.TotalAmount != totalAmount || currentStock.TotalMarketValue != totalMarketValue
                    || currentStock.CirculationMarketValue != circulationMarketValue || currentStock.IndustryName != industry || currentStock.MainCount != mainCount
                    || currentStock.Earning != earning || currentStock.PERatio != peRatio)
                {
                    T_Stock stock = new T_Stock
                    {
                        FullCode = fullCode,
                        StockName = stockName,
                        TotalAmount = totalAmount,
                        TotalMarketValue = totalMarketValue,
                        CirculationMarketValue = circulationMarketValue,
                        CreatedOn = DateTime.Now,
                        Deleted = false,
                        StockCode = stockCode,
                        PyAbbre = PyHelper.GetFirst(stockName),
                        PyFullName = PyHelper.Get(stockName),
                        IndustryName = industry,
                        Industry = industryList.FirstOrDefault(s => s.Name == industry).Id,
                        MainCount = mainCount,
                        Earning = earning,
                        PERatio = peRatio,
                    };
                    if (currentStock == null) addStockList.Add(stock);
                    else upStockList.Add(stock);
                }
                //拼接历史交易记录sql
                upStockHistorySql.Append(string.Format("insert into T_TransactionRecord_{0} (StockCode,TradingDate,[Open],[High],[Low],[Close],[Rose],[Amplitude],[Hands],[Amount],[Turnover],[VOLAMOUNT]) values ('{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}) go "
                    , stockCode, tradingDate, open, high, low, close, rose, amplitude, hands, amount, turnover, VOLAMOUNT));
            }
            //拼接更新或插入股票信息sql
            addStockSql.Append("insert into T_Stock (StockCode,FullCode,StockName,TotalAmount,TotalMarketValue,CirculationMarketValue,MainCount,Earning,PERatio,Industry,IndustryName,PyAbbre,PyFullName) ");
            int addStockIndex = 0;
            foreach (var item in addStockList)
            {
                addStockSql.Append(string.Format("select '{0}','{1}','{2}',{3},{4},{5},{6},{7},{8},{9},'{10}','{11}','{12}'",
                    item.StockCode, item.FullCode, item.StockName, item.TotalAmount, item.TotalMarketValue, item.CirculationMarketValue, item.MainCount
                    , item.Earning, item.PERatio, item.Industry, item.IndustryName, item.PyAbbre, item.PyFullName));
                if (++addStockIndex < addStockList.Count) sqlsb.Append(" union ");
            }
            foreach (var item in upStockList)
            {
                upStockSql.Append(string.Format("update T_Stock set StockName='{0}',TotalAmount={1},TotalMarketValue={2},CirculationMarketValue={3},MainCount={4},Earning={5},PERatio={6},Industry={7},IndustryName={8},PyAbbre={9},PyFullName={10} where StockCode={11} go",
                    item.StockName, item.TotalAmount, item.TotalMarketValue, item.CirculationMarketValue, item.MainCount
                    , item.Earning, item.PERatio, item.Industry, item.IndustryName, item.PyAbbre, item.PyFullName, item.StockCode));
            }
            //执行sql
            SqlHelper.ExecuteNonQuery(ConfigHelper.GetConnStr("StockConn"), CommandType.Text, addStockSql.ToString());
            SqlHelper.ExecuteNonQuery(ConfigHelper.GetConnStr("StockConn"), CommandType.Text, upStockSql.ToString());
        }

        /// <summary>
        /// 更新行业信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool UpdateIndustry(DataTable dt)
        {
            var commonSql = Core.AppContext.Current.SqlHelper(Core.Enum.SqlTypeEnum.Stock);
            //现有行业列表、新增行业列表
            List<T_Industry> industryList = commonSql.GetDataList<T_Industry>(), newIndustryList = new List<T_Industry>();
            StringBuilder sqlsb = new StringBuilder();
            //判断是否是新增行业
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string industry = dt.Rows[i][32].ToString();
                if (!string.IsNullOrEmpty(industry) && !industryList.Any(item => item.Name.Equals(industry)) && !newIndustryList.Any(item => item.Name.Equals(industry)))
                    newIndustryList.Add(new T_Industry { Name = industry });
            }
            //拼接sql语句
            sqlsb.Append("insert into T_Industry (Name) ");
            int index = 0;
            foreach (var item in newIndustryList)
            {
                sqlsb.Append(string.Format("select {0}", item));
                if (++index < newIndustryList.Count) sqlsb.Append(" union ");
            }
            //执行sql
            return SqlHelper.ExecuteNonQuery(ConfigHelper.GetConnStr("StockConn"), CommandType.Text, sqlsb.ToString()) > 0;
        }

        /// <summary>
        /// 更新历史交易记录
        /// </summary>
        /// <param name="stockCode"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int UpSingleStockData(string stockCode, DataTable dt)
        {
            int count = 0, runCount = 500;
            //检查表是否存在，不存在则创建
            CreateTableBLL.CreateStockTableOrNot(stockCode);
            int begin = 0, end = dt.Rows.Count >= runCount ? runCount : dt.Rows.Count;
            for (int i = 0; i < dt.Rows.Count / runCount + 1; i++)
            {
                string sql = getSignleStockInsertStr(stockCode, dt, begin, end);
                begin = end + 1;
                end = dt.Rows.Count >= end + runCount ? end + runCount : dt.Rows.Count - 1;
                count += SqlHelper.ExecuteNonQuery(ConfigHelper.GetConnStr("StockConn"), CommandType.Text, sql.ToString());
            }
            return count;
        }

        /// <summary>
        /// 获取插入sql语句，分段插入，防止用时过长，导致超时
        /// </summary>
        /// <param name="stockCode">代码</param>
        /// <param name="dt">数据源</param>
        /// <param name="begin">开始索引</param>
        /// <param name="end">结束索引</param>
        /// <returns></returns>
        private string getSignleStockInsertStr(string stockCode, DataTable dt, int begin, int end)
        {
            StringBuilder sqlsb = new StringBuilder();
            if (begin == 0)
                sqlsb.Append(string.Format("delete T_TransactionRecord_{0};", stockCode));
            sqlsb.Append(string.Format("insert into T_TransactionRecord_{0} (StockCode,TradingDate,[Open],[High],[Low],[Close],[Rose],[Amplitude],[Hands],[Amount],[Turnover],[VOLAMOUNT])", stockCode));
            for (int i = begin; i <= end; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString())) continue;
                DateTime tradingDate = Convert.ToDateTime(dt.Rows[i][0].ToString().Split(',')[0]);
                decimal open = 0, high = 0, low = 0, close = 0, rose = 0, amplitude = 0, amount = 0, turnover = 0;
                int hands = 0, VOLAMOUNT = 0;
                decimal.TryParse(dt.Rows[i][1].ToString(), out open);
                decimal.TryParse(dt.Rows[i][2].ToString(), out high);
                decimal.TryParse(dt.Rows[i][3].ToString(), out low);
                decimal.TryParse(dt.Rows[i][4].ToString(), out close);
                decimal.TryParse(dt.Rows[i][5].ToString(), out rose);
                decimal.TryParse(dt.Rows[i][6].ToString(), out amplitude);
                int.TryParse(dt.Rows[i][7].ToString(), out hands);
                decimal.TryParse(dt.Rows[i][8].ToString(), out amount);
                decimal.TryParse(dt.Rows[i][9].ToString(), out turnover);
                int.TryParse(dt.Rows[i][10].ToString(), out VOLAMOUNT);
                sqlsb.Append(string.Format(" select '{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
                    stockCode, tradingDate, open, high, low, close, rose, amplitude, hands, amount, turnover, VOLAMOUNT));
                if (i != end) sqlsb.Append(" union ");
            }
            return sqlsb.ToString();
        }
    }
}
