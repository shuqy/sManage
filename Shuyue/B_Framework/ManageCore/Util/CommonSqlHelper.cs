using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Util
{
    /// <summary>
    /// 公共sql操作方法类
    /// </summary>
    public class CommonSqlHelper
    {
        public string SqlConnStr;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlConnStr">数据库链接字符串</param>
        public CommonSqlHelper(string sqlConnStr)
        {
            this.SqlConnStr = sqlConnStr;
        }
        /// <summary>
        /// 根据ID查询数据
        /// </summary>
        public T GetModelById<T>(int id) where T : new()
        {
            return GetModel<T>(string.Format("id={0}", id));
        }
        /// <summary>
        /// 根据指定字段返回查询的第一条数据
        /// </summary>
        public T GetModelBy<T>(string field, int value) where T : new()
        {
            return GetModel<T>(string.Format("{0}={1}", field, value));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public T GetModel<T>(string where) where T : new()
        {
            T temp = new T();
            string tableName = temp.GetType().Name;
            string sql = string.Format("SELECT TOP 1 * FROM {0} WHERE {1}", tableName, string.IsNullOrEmpty(where) ? "1=1" : where);
            DataSet ds = SqlHelper.ExecuteDataSet(SqlConnStr, CommandType.Text, sql, null);
            if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ModelHelper.FillModel<T>(ds.Tables[0].Rows[0]);
            return default(T);
        }
        /// <summary>
        /// 根据sql查询数据库并返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T GetModelBySql<T>(string sql) where T : new()
        {
            T temp = new T();
            string tableName = temp.GetType().Name;
            DataSet ds = SqlHelper.ExecuteDataSet(SqlConnStr, CommandType.Text, sql);
            if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ModelHelper.FillModel<T>(ds.Tables[0].Rows[0]);
            return default(T);
        }
        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <typeparam name="T">泛型类型，即要更新的表名</typeparam>
        /// <param name="where">查询条件</param>
        /// <returns>List<T></returns>
        public List<T> GetDataList<T>(string where = "1=1 ") where T : new()
        {
            T temp = new T();
            string tableName = temp.GetType().Name;
            string sql = string.Format("SELECT * FROM {0} WHERE {1}", tableName, where);
            DataSet ds = SqlHelper.ExecuteDataSet(SqlConnStr, CommandType.Text, sql);
            if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ModelHelper.FillModelList<T>(ds.Tables[0]).ToList();
            return default(List<T>);
        }
        /// <summary>
        /// 根据sql查询数据库并返回List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> GetDataListBySql<T>(string sql) where T : new()
        {
            T temp = new T();
            string tableName = temp.GetType().Name;
            DataSet ds = SqlHelper.ExecuteDataSet(SqlConnStr, CommandType.Text, sql);
            if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ModelHelper.FillModelList<T>(ds.Tables[0]).ToList();
            return default(List<T>);
        }
        /// <summary>
        /// 主要用来更新数据，要更新的字段自行确定
        /// </summary>
        /// <typeparam name="T">泛型类型，即要更新的表名</typeparam>
        /// <param name="obj">要更新数据实体</param>
        /// <param name="fileds">要更新字段数组</param>
        /// <param name="primaryKeyName">可选更新主键，默认为id</param>
        /// <returns></returns>
        public int AddOrUpdate<T>(T obj, string[] fileds, string primaryKeyName = "id") where T : new()
        {
            string className = obj.GetType().Name;
            PropertyInfo[] propertyInfo = obj.GetType().GetProperties();
            if (!string.Equals(propertyInfo[0].Name.ToLower(), primaryKeyName.ToLower())) throw new Exception("类中不包含主键字段");
            bool isAdd = Convert.ToInt32(propertyInfo[0].GetValue(obj, null)) <= 0;
            return isAdd ? 0 : this.Update<T>(obj, fileds, className, primaryKeyName);
        }
        /// <summary>
        /// 添加
        /// </summary>
        public int Insert<T>(T obj, string[] fields, string tableName) where T : new()
        {
            string sql = InsertSql(obj, fields, tableName);
            DataSet ds = SqlHelper.ExecuteDataSet(SqlConnStr, CommandType.Text, sql, null);
            return ds.Tables[0].Rows[0][0] == DBNull.Value || ds.Tables[0].Rows[0][0] == null ? 1 : Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public int Update<T>(T obj, string[] fields, string tableName, string primaryKeyName) where T : new()
        {
            string sql = UpdateSql(obj, fields, tableName, primaryKeyName);
            DataSet ds = SqlHelper.ExecuteDataSet(SqlConnStr, CommandType.Text, sql, null);
            return ds.Tables[0].Rows[0][0] == DBNull.Value || ds.Tables[0].Rows[0][0] == null ? 1 : Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 生成插入sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fields"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string InsertSql<T>(T obj, string[] fields, string tableName)
        {
            StringBuilder sqlsb = new StringBuilder();
            if (obj == null) return sqlsb.ToString();
            T entity = Activator.CreateInstance<T>();
            PropertyInfo[] pro = entity.GetType().GetProperties();
            string insersql = string.Format("insert into {0} (", tableName);//插入sql，包括表名，出入字段
            string inservalue = " values(";//插入sql对应的值
            for (int i = 0; i < pro.Length; i++)
            {
                if (!fields.Any(a => a.ToLower() == pro[i].Name.ToLower())) continue;
                object value = pro[i].GetValue(obj, null);
                if (value == null || value == DBNull.Value || !pro[i].CanRead) continue;
                insersql += string.Format("[{0}],", pro[i].Name);
                if (value is int)
                {
                    inservalue += string.Format("{0},", value.ToString());
                }
                else if (value is DateTime)
                {
                    inservalue += string.Format("'{0}',", value.ToString());
                }
                else
                {
                    inservalue += string.Format("'{0}',", value.ToString());
                }
            }
            sqlsb.Append(string.Format("{0}){1});", insersql.Substring(0, insersql.Length - 1), inservalue.Substring(0, inservalue.Length - 1)));
            sqlsb.Append("select @@identity;");
            return sqlsb.ToString();
        }

        /// <summary>
        /// 生成更新sql语句
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="obj">要更新的实体对象</param>
        /// <param name="fields">要更新字段数组</param>
        /// <param name="tableName">表名称</param>
        /// <param name="primaryKeyName">主键字段名称</param>
        /// <returns></returns>
        public string UpdateSql<T>(T obj, string[] fields, string tableName, string primaryKeyName)
        {
            StringBuilder sqlsb = new StringBuilder();
            if (obj == null) return sqlsb.ToString();
            T entity = Activator.CreateInstance<T>();
            PropertyInfo[] pro = entity.GetType().GetProperties();
            string updatesql = string.Format("update {0} set ", tableName);
            string whereStr = "";
            for (int i = 0; i < pro.Length; i++)
            {
                if (pro[i].Name.ToLower().Equals(primaryKeyName.ToLower()))//获取更新的主键
                {
                    whereStr = string.Format(" where {0}={1} ", primaryKeyName, pro[i].GetValue(obj, null));
                    continue;
                }
                if (!fields.Contains(pro[i].Name)) continue;//不包含的字段不更新
                object value = pro[i].GetValue(obj, null);
                if (value == null || value == DBNull.Value || !pro[i].CanRead)
                    continue;
                if (value is int)
                    updatesql += string.Format("[{0}]={1},", pro[i].Name, value);
                else if (value is DateTime)
                {
                    if ((DateTime)value != default(DateTime))
                        updatesql += string.Format("[{0}]='{1}',", pro[i].Name, value.ToString());
                }
                else
                    updatesql += string.Format("[{0}]='{1}',", pro[i].Name, value.ToString());
            }
            sqlsb.Append(updatesql.TrimEnd(','));
            sqlsb.Append(whereStr);
            sqlsb.Append(";select @@identity;");
            return sqlsb.ToString();
        }
    }
}
