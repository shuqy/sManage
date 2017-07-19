using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum;
using MySql.Data.MySqlClient;
using Dapper;

namespace Core.Util
{
    public class DapperMySql : DapperBase
    {
        public DapperMySql(string sqlConnectionStr) : base(sqlConnectionStr)
        {
            _sqlConnectionStr = sqlConnectionStr;
        }

        public DapperMySql(DbConnEnum dbConnEnum) : base(dbConnEnum)
        {
            _sqlConnectionStr = ConfigHelper.GetConn(dbConnEnum.ToString()).ConnectionString;
        }

        public override void Execute(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = new MySqlConnection(_sqlConnectionStr))
            {
                conn.Open();
                var r = conn.Query(sql, param, transaction, buffered, commandTimeout, commandType);
                conn.Close();
            }
        }

        public override int InsertMultiple<T>(string sql, IEnumerable<T> entities, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = new MySqlConnection(_sqlConnectionStr))
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
            using (var conn = new MySqlConnection(_sqlConnectionStr))
            {
                conn.Open();
                var list = conn.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
                conn.Close();
                return list;
            }
        }

        public override T QueryFirstOrDefault<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = new MySqlConnection(_sqlConnectionStr))
            {
                conn.Open();
                var model = conn.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
                conn.Close();
                return model;
            }
        }
    }
}
