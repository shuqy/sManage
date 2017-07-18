using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Core.Enum;

namespace Core.Util
{
    /// <summary>
    /// Dapper帮助类
    /// </summary>
    public class DapperHelper
    {
        //连接字符串
        private string _sqlConnectionStr;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlConnectionStr">连接字符串</param>
        public DapperHelper(string sqlConnectionStr)
        {
            _sqlConnectionStr = sqlConnectionStr;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnEnum"></param>
        public DapperHelper(DbConnEnum dbConnEnum)
        {
            _sqlConnectionStr = ConfigHelper.GetConn(dbConnEnum.ToString()).ConnectionString;
        }

        #region 执行增删改
        /// <summary>
        /// 执行语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        public void Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var conn = new SqlConnection(_sqlConnectionStr))
            {
                conn.Open();
                var r = conn.Execute(sql, param, transaction, commandTimeout, commandType);
                conn.Close();
            }
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="entities"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public int InsertMultiple<T>(string sql, IEnumerable<T> entities, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            where T : new()
        {
            using (var conn = new SqlConnection(_sqlConnectionStr))
            {
                int records = 0;
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        conn.Execute(sql, entities, transaction, commandTimeout, commandType);
                    }
                    catch (DataException ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                    trans.Commit();
                }
                return records;
            }
        }
        #endregion

        #region 执行查询
        /// <summary>
        /// 执行查询，根据T返回查询类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
            where T : new()
        {
            using (var conn = new SqlConnection(_sqlConnectionStr))
            {
                conn.Open();
                var list = conn.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
                conn.Close();
                return list;
            }
        }

        /// <summary>
        /// 查询一条数据，根据T返回查询类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public T QueryFirstOrDefault<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            where T : new()
        {
            using (var conn = new SqlConnection(_sqlConnectionStr))
            {
                conn.Open();
                var model = conn.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
                conn.Close();
                return model;
            }
        }
        #endregion


    }
}
