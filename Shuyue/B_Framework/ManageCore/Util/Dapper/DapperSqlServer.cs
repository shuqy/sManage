using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using Core.Enum;

namespace Core.Util
{
    public class DapperSqlServer : DapperBase
    {
        public DapperSqlServer(string sqlConnectionStr) : base(sqlConnectionStr)
        {
            _sqlConnectionStr = sqlConnectionStr;
        }

        public DapperSqlServer(DbConnEnum dbConnEnum) : base(dbConnEnum)
        {
            _sqlConnectionStr = ConfigHelper.GetConn(dbConnEnum.ToString()).ConnectionString;
        }

        public override void Execute(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = new SqlConnection(_sqlConnectionStr))
            {
                conn.Open();
                var r = conn.Execute(sql, param, transaction, commandTimeout, commandType);
                conn.Close();
            }
        }

        public override int InsertMultiple<T>(string sql, IEnumerable<T> entities, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
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

        public override IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = new SqlConnection(_sqlConnectionStr))
            {
                conn.Open();
                var list = conn.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
                conn.Close();
                return list;
            }
        }

        public override T QueryFirstOrDefault<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = new SqlConnection(_sqlConnectionStr))
            {
                conn.Open();
                var model = conn.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
                conn.Close();
                return model;
            }
        }
    }
}
