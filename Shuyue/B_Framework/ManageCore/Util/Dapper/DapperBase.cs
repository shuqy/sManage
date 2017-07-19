using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Core.Enum;

namespace Core.Util
{
    /// <summary>
    /// Dapper帮助类
    /// </summary>
    public abstract class DapperBase
    {

        //连接字符串
        protected string _sqlConnectionStr;

        public DapperBase(string sqlConnectionStr) { }

        public DapperBase(DbConnEnum dbConnEnum) { }

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
        public abstract void Execute(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

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
        public abstract int InsertMultiple<T>(string sql, IEnumerable<T> entities, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : new();
        #endregion

        #region 执行查询
        /// <summary>
        /// 执行查询，根据T返回查询类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public abstract IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) where T : new();

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
        public abstract T QueryFirstOrDefault<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : new();
        #endregion

    }
}
