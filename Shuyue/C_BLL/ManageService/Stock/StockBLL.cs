using Core.Util;
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
        /// <summary>
        /// 更新股票交易记录
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
        /// <param name="stockCode"></param>
        /// <param name="dt"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
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
