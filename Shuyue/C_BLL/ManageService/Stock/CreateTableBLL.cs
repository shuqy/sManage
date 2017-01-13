using Core.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageService.Stock
{
    public class CreateTableBLL
    {
        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool IsExistTable(string tableName)
        {
            string dbStr = string.Format("select 1 from sysobjects where id = object_id('{0}') and type ='U'", tableName);
            DataSet ds = SqlHelper.ExecuteDataSet(ConfigHelper.GetConnStr("StockConn"), CommandType.Text, dbStr);
            return ds.Tables[0].Rows.Count != 0;
        }

        /// <summary>
        /// 创建股票表
        /// </summary>
        /// <param name="stockCode">股票代码</param>
        /// <returns></returns>
        public static bool CreateStockTableOrNot(string stockCode)
        {
            if (IsExistTable(string.Format("T_TransactionRecord_{0}", stockCode))) return false;
            StringBuilder sqlsb = new StringBuilder();
            //sqlsb.Append("using SQY_Stock ");
            sqlsb.Append(string.Format("create table T_TransactionRecord_{0}(", stockCode));
            sqlsb.Append("Id int identity(1,1) primary key,");
            sqlsb.Append("StockCode varchar(50) not null,");
            sqlsb.Append("TradingDate datetime not null,");
            sqlsb.Append("[Open] decimal(18,4) not null,");
            sqlsb.Append("[High] decimal(18,4) not null,");
            sqlsb.Append("[Low] decimal(18,4) not null,");
            sqlsb.Append("[Close] decimal(18,4) not null,");
            sqlsb.Append("[Rose] decimal(18,4) not null,");
            sqlsb.Append("[Amplitude] decimal(18,4) not null,");
            sqlsb.Append("[Hands] int not null,");
            sqlsb.Append("[Amount] decimal(18,4) not null,");
            sqlsb.Append("[Turnover] decimal(18,4) not null,");
            sqlsb.Append("[VOLAMOUNT] int not null,");
            sqlsb.Append("[CreatedOn] datetime not null default getdate(),");
            sqlsb.Append("[UpdatedOn] datetime null,");
            sqlsb.Append("[Deleted] bit not null default 0,");
            sqlsb.Append(") ");
            sqlsb.Append(string.Format("CREATE INDEX Ix_T_TransactionRecord_{0}_Date ON T_TransactionRecord_{0}(TradingDate)", stockCode));
            return SqlHelper.ExecuteNonQuery(ConfigHelper.GetConnStr("StockConn"), CommandType.Text, sqlsb.ToString()) > 0;
        }
    }
}
