using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Util
{
    public class CommonSqlUtility
    {

        public string SqlConnStr;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlConnStr">数据库链接字符串</param>
        public CommonSqlUtility(string sqlConnStr)
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
        /// 查询列表数据
        /// </summary>
        /// <typeparam name="T">泛型类型，即要更新的表名</typeparam>
        /// <param name="where">查询条件</param>
        /// <returns>List<T></returns>
        public List<T> GetDataList<T>(string where = "1=1") where T : new()
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
            try
            {
                T temp = new T();
                DataSet ds = SqlHelper.ExecuteDataSet(SqlConnStr, CommandType.Text, sql);
                if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    return ModelHelper.FillModelList<T>(ds.Tables[0]).ToList();
                return default(List<T>);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据sql查询数据库并返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T GetDataBySql<T>(string sql) where T : new()
        {
            T temp = new T();
            string tableName = temp.GetType().Name;
            DataSet ds = SqlHelper.ExecuteDataSet(SqlConnStr, CommandType.Text, sql);
            if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ModelHelper.FillModel<T>(ds.Tables[0].Rows[0]);
            return default(T);
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int RunSql(string sql) 
        {
            return SqlHelper.ExecuteNonQuery(SqlConnStr, CommandType.Text, sql);
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
            return isAdd ? this.Insert<T>(obj, fileds, className) : this.Update<T>(obj, fileds, className, primaryKeyName);
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

        /// <summary>
        /// 假删除
        /// </summary>
        public bool DeleteModel<T>(Expression<Func<T, bool>> predicate, string updateField, int fieldVal) where T : new()
        {
            string where = GetWhereStr(predicate);
            return DeleteModel<T>(updateField, fieldVal, where);
        }

        /// <summary>
        /// 假删除
        /// </summary>
        /// <typeparam name="T">表</typeparam>
        /// <param name="updateField">要更新字段</param>
        /// <param name="fieldVal">字段值</param>
        /// <param name="tagStr">符合条件条件字段</param>
        /// <param name="tagVal">符合条件条件值</param>
        /// <returns></returns>
        public bool DeleteModel<T>(string updateField, int fieldVal, string tagStr, int tagVal) where T : new()
        {
            string className = (new T()).GetType().Name;
            string sql = string.Format("UPDATE {0} SET {1}={2} where {3}={4}", className, updateField, fieldVal, tagStr, tagVal);
            return SqlHelper.ExecuteNonQuery(SqlConnStr, CommandType.Text, sql) > 0;
        }
        /// <summary>
        /// 假删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateField"></param>
        /// <param name="fieldVal"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        private bool DeleteModel<T>(string updateField, int fieldVal, string where = "1=1") where T : new()
        {
            string className = (new T()).GetType().Name;
            string sql = string.Format("UPDATE {0} SET {1}={2} where {3}", className, updateField, fieldVal, where);
            return SqlHelper.ExecuteNonQuery(SqlConnStr, CommandType.Text, sql) > 0;
        }

        /// <summary>
        /// 真删除
        /// </summary>
        public bool HardDeleteModel<T>(Expression<Func<T, bool>> predicate) where T : new()
        {
            string where = GetWhereStr(predicate);
            return HardDeleteModel<T>(where);
        }
        /// <summary>
        /// 真删除
        /// </summary>
        public bool HardDeleteModel<T>(string field, int value) where T : new()
        {
            return HardDeleteModel<T>(string.Format("{1}={2}", field, value));
        }
        /// <summary>
        /// 真删除
        /// </summary>
        private bool HardDeleteModel<T>(string whereStr = "1=1") where T : new()
        {
            string className = (new T()).GetType().Name;
            string sql = string.Format("DELETE {0} WHERE {1}", className, whereStr);
            return SqlHelper.ExecuteNonQuery(SqlConnStr, CommandType.Text, sql) > 0;
        }

        /// <summary>
        /// 添加或更新数据
        /// </summary>
        /// <typeparam name="T">泛型类型，即要更新的表名</typeparam>
        /// <param name="obj">要更新数据实体</param>
        /// <param name="primaryKeyName">可选更新主键，默认为id</param>
        /// <returns></returns>
        public int AddOrUpdate<T>(T obj, string primaryKeyName = "id") where T : new()
        {
            string className = obj.GetType().Name;
            PropertyInfo[] propertyInfo = obj.GetType().GetProperties();
            if (!string.Equals(propertyInfo[0].Name.ToLower(), primaryKeyName.ToLower())) throw new Exception("类中不包含主键字段");
            bool isAdd = Convert.ToInt32(propertyInfo[0].GetValue(obj, null)) <= 0;
            string[] fileds = SqlFieldsUtil.GetFields(className, isAdd);
            if (fileds == null) throw new Exception("未设定添加或更新字段");
            return isAdd ? this.Insert<T>(obj, fileds, className) : this.Update<T>(obj, fileds, className, primaryKeyName);
        }

        /// <summary>
        /// 筛选查询
        /// </summary>
        /// <typeparam name="T">表</typeparam>
        /// <returns></returns>
        public List<T> Get<T>() where T : new()
        {
            return Get<T>(t => true);
        }

        /// <summary>
        /// 返回序列中的第一个元素；如果序列中不包含任何元素，则返回默认值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FirstOrDefault<T>(Expression<Func<T, bool>> predicate = null) where T : new()
        {
            List<T> list = Get(predicate);
            return list == null ? default(T) : list.FirstOrDefault();
        }


        /// <summary>
        /// 筛选查询 
        /// lambda表达式中不能出现!a.Deleted的写法，替换为a.Deleted==True 
        /// 不能引用父类中的变量，如需要请先定义变量再赋值
        /// </summary>
        /// <typeparam name="T">表</typeparam>
        /// <param name="predicate">用于测试每个元素是否满足条件的函数</param>
        /// <returns></returns>
        public List<T> Get<T>(Expression<Func<T, bool>> predicate = null) where T : new()
        {
            try
            {
                //解析表达式树
                var result = new ExpressionAnalysis(predicate).ResultData;
                //拼接SQL
                StringBuilder sqlsb = new StringBuilder();
                sqlsb.Append("SELECT * FROM ");
                var table = result.TableList.FirstOrDefault();
                sqlsb.Append(string.Format("{0} AS {1} ", table.Value.Name, table.Key));
                string whereStr = "";
                if (result.StackList.Any())
                {
                    whereStr += "WHERE ";
                    foreach (var item in result.StackList)
                    {
                        if (!item.StartsWith("@")) { whereStr += string.Format("{0} ", item); continue; }
                        if (!result.ParamList.ContainsKey(item)) throw new Exception("参数不存在");
                        whereStr += string.Format("{0} ", result.ParamList[item]);
                    }
                    sqlsb.Append(whereStr);
                }
                //返回执行结果
                return GetDataListBySql<T>(sqlsb.ToString());
            }
            catch (Exception ex)
            {
                //Log(ex.StackTrace);
                throw new Exception("表达式中包含未能解析的函数");
            }
        }

        private string GetWhereStr<T>(Expression<Func<T, bool>> predicate = null)
        {
            //解析表达式树
            var result = new ExpressionAnalysis(predicate).ResultData;
            string whereStr = "";
            if (result.StackList.Any())
            {
                foreach (var item in result.StackList)
                {
                    if (!item.StartsWith("@")) { whereStr += string.Format("{0} ", item.Replace("[TAB].", "")); continue; }
                    if (!result.ParamList.ContainsKey(item)) throw new Exception("参数不存在");
                    whereStr += string.Format("{0} ", result.ParamList[item]);
                }
            }
            return whereStr;
        }
    }

    public class ExpressionAnalysis
    {
        /// <summary>
        /// 解析结果
        /// </summary>
        public AnalysisData ResultData { get; set; }

        /// <summary>
        /// 表达式所有参数集合
        /// </summary>
        private Dictionary<string, object> _params;

        /// <summary>
        /// 命名参数别名
        /// </summary>
        private const string _argName = "TAB";

        public ExpressionAnalysis()
        {
            ResultData = new AnalysisData();
            _params = new Dictionary<string, object>();
        }
        public ExpressionAnalysis(LambdaExpression exp)
             : this()
        {
            if (exp != null)
            {
                AnalysisParams(GetChildValue(exp.Body), _params);
                foreach (var item in exp.Parameters)
                {
                    AnalysisTables(item);
                }
                AnalysisExpression(exp.Body, true);
            }
        }

        /// <summary>
        /// 解析表达式
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="isLeftChild"></param>
        private void AnalysisExpression(Expression exp, bool isLeftChild = true)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.AndAlso:
                    ResultData.StackList.Add("(");
                    AnalysisExpression(GetChildExpression(exp));
                    ResultData.StackList.Add(")");
                    ResultData.StackList.Add("AND");
                    ResultData.StackList.Add("(");
                    AnalysisExpression(GetChildExpression(exp, false), false);
                    ResultData.StackList.Add(")");
                    break;
                case ExpressionType.OrElse:
                    ResultData.StackList.Add("(");
                    AnalysisExpression(GetChildExpression(exp));
                    ResultData.StackList.Add(")");
                    ResultData.StackList.Add("OR");
                    ResultData.StackList.Add("(");
                    AnalysisExpression(GetChildExpression(exp, false), false);
                    ResultData.StackList.Add(")");
                    break;
                case ExpressionType.Equal:
                    AnalysisExpression(GetChildExpression(exp));
                    ResultData.StackList.Add("=");
                    AnalysisExpression(GetChildExpression(exp, false), false);
                    break;
                case ExpressionType.NotEqual:
                    AnalysisExpression(GetChildExpression(exp));
                    ResultData.StackList.Add("!=");
                    AnalysisExpression(GetChildExpression(exp, false), false);
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    AnalysisExpression(GetChildExpression(exp));
                    ResultData.StackList.Add(">=");
                    AnalysisExpression(GetChildExpression(exp, false), false);
                    break;
                case ExpressionType.GreaterThan:
                    AnalysisExpression(GetChildExpression(exp));
                    ResultData.StackList.Add(">");
                    AnalysisExpression(GetChildExpression(exp, false), false);
                    break;
                case ExpressionType.LessThan:
                    AnalysisExpression(GetChildExpression(exp));
                    ResultData.StackList.Add("<");
                    AnalysisExpression(GetChildExpression(exp, false), false);
                    break;
                case ExpressionType.LessThanOrEqual:
                    AnalysisExpression(GetChildExpression(exp));
                    ResultData.StackList.Add("<=");
                    AnalysisExpression(GetChildExpression(exp, false), false);
                    break;
                case ExpressionType.Call:
                    var imExp = exp as MethodCallExpression;
                    AnalysisExpression(imExp.Object, true);
                    ResultData.StackList.Add("LIKE");
                    if (imExp.Arguments.Count > 0)
                    {
                        var arg0 = imExp.Arguments[0] as MemberExpression;
                        ResultData.StackList.Add("'%'+");
                        AnalysisExpression(imExp.Arguments[0], false);
                        ResultData.StackList.Add("+'%'");
                    }
                    break;
                case ExpressionType.MemberAccess:
                    if (isLeftChild)
                    {
                        AnalysisTables(exp);
                        var mberExp = exp as MemberExpression;
                        var parentName = GetExpressionName(mberExp.Expression);
                        if (!string.IsNullOrEmpty(parentName))
                        {
                            ResultData.StackList.Add(string.Format("[{0}].{1}", parentName, GetExpressionName(exp)));
                            break;
                        }
                        ResultData.StackList.Add(GetExpressionName(exp));
                    }
                    else
                    {
                        var paramName = GetParamName(exp);
                        ResultData.ParamList.Add(paramName, _params[paramName]);
                        ResultData.StackList.Add(paramName);
                    }
                    break;
                case ExpressionType.Constant:
                    var constent = exp as ConstantExpression;
                    if (constent.Value == null)
                    {
                        var op = ResultData.StackList.ElementAt(ResultData.StackList.Count - 1);
                        ResultData.StackList.RemoveAt(ResultData.StackList.Count - 1);
                        if (string.Equals(op, "="))
                        {
                            ResultData.StackList.Add("IS NULL");
                        }
                        else
                        {
                            ResultData.StackList.Add("IS NOT NULL");
                        }
                        break;
                    }
                    if (constent.Value.GetType() == typeof(String))
                    {
                        ResultData.StackList.Add(string.Format("'{0}'", constent.Value));
                        break;
                    }
                    if (constent.Value.GetType() == typeof(bool))
                    {
                        if (ResultData.StackList.Count > 0)
                        {
                            var value = Convert.ToBoolean(constent.Value);
                            ResultData.StackList.Add(string.Format("{0}", value ? "1" : "0"));
                        }

                        break;
                    }
                    ResultData.StackList.Add(string.Format("{0}", constent.Value));
                    break;
                case ExpressionType.Convert:
                    var uExp = exp as UnaryExpression;
                    AnalysisExpression(uExp.Operand, isLeftChild);
                    break;
                case ExpressionType.New:
                    var newExp = exp as NewExpression;
                    //解析查询字段
                    for (int i = 0; i < newExp.Arguments.Count; i++)
                    {
                        AnalysisExpression(newExp.Arguments[i]);
                        ResultData.StackList.Add("AS");
                        ResultData.StackList.Add(string.Format("'{0}'", newExp.Members[i].Name));
                    }
                    break;
                case ExpressionType.Parameter:
                    AnalysisExpression(Expression.New(exp.Type)); break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 获取孩子节点
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="getLeft"></param>
        /// <returns></returns>
        private Expression GetChildExpression(Expression exp, bool getLeft = true)
        {
            var className = exp.GetType().Name;
            switch (className)
            {
                case "BinaryExpression":
                case "LogicalBinaryExpression":
                    var bExp = exp as BinaryExpression;
                    return getLeft ? bExp.Left : bExp.Right;
                case "PropertyExpression":
                case "FieldExpression":
                    var mberExp = exp as MemberExpression;
                    return mberExp;
                case "MethodBinaryExpression":
                    var mbExp = exp as BinaryExpression;
                    return getLeft ? mbExp.Left : mbExp.Right;
                case "UnaryExpression":
                    var unaryExp = exp as UnaryExpression;
                    return unaryExp;
                case "ConstantExpression":
                    var cExp = exp as ConstantExpression;
                    return cExp;
                case "InstanceMethodCallExpressionN":
                    var imExp = exp as MethodCallExpression;
                    return imExp;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取参数名
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="isLeftChild"></param>
        /// <returns></returns>
        private string GetParamName(Expression exp)
        {
            var className = exp.GetType().Name;
            switch (className)
            {
                case "PropertyExpression":
                case "FieldExpression":
                    var mberExp = exp as MemberExpression;
                    return string.Format("@{0}", mberExp.Member.Name);
                case "TypedParameterExpression":
                    var texp = exp as ParameterExpression;
                    return string.Format("@{0}", texp.Name);
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 获取变量名
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="isLeftChild"></param>
        /// <returns></returns>
        private string GetExpressionName(Expression exp)
        {
            var className = exp.GetType().Name;
            switch (className)
            {
                case "PropertyExpression":
                case "FieldExpression":
                    var mberExp = exp as MemberExpression;
                    return string.Format("{0}", mberExp.Member.Name);
                case "TypedParameterExpression":
                    return _argName;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 解析参数
        /// </summary>
        /// <param name="paramObj"></param>
        /// <param name="_params"></param>
        private void AnalysisParams(object paramObj, Dictionary<string, object> _params)
        {
            if (IsNullOrDefaultType(paramObj)) { return; }
            if (_params == null) _params = new Dictionary<string, object>();
            foreach (var item in paramObj.GetType().GetProperties())
            {
                if (IsDefaultType(item.PropertyType))
                {
                    var value = item.GetValue(paramObj, null);
                    if (value != null) _params.Add(string.Format("@{0}", item.Name), value);
                    continue;
                }
                AnalysisParams(item.GetValue(paramObj, null), _params);
            }
            foreach (var item in paramObj.GetType().GetFields())
            {
                if (IsDefaultType(item.FieldType))
                {
                    var value = item.GetValue(paramObj);
                    if (value != null) _params.Add(string.Format("@{0}", item.Name), value);
                    continue;
                }
                AnalysisParams(item.GetValue(paramObj), _params);
            }
        }

        /// <summary>
        /// 解析获取表达式的值
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private object GetChildValue(Expression exp)
        {
            var className = exp.GetType().Name;
            switch (className)
            {
                case "BinaryExpression":
                case "LogicalBinaryExpression":
                    var lExp = exp as BinaryExpression;
                    var ret = GetChildValue(lExp.Left);
                    if (IsNullOrDefaultType(ret))
                    {
                        ret = GetChildValue(lExp.Right);
                    }
                    return ret;
                case "MethodBinaryExpression":
                    var mbExp = exp as BinaryExpression;
                    var ret1 = GetChildValue(mbExp.Left);
                    if (IsNullOrDefaultType(ret1))
                    {
                        ret1 = GetChildValue(mbExp.Right);
                    }
                    return ret1;

                case "PropertyExpression":
                case "FieldExpression":
                    var mberExp = exp as MemberExpression;
                    return GetChildValue(mberExp.Expression);
                case "ConstantExpression":
                    var cExp = exp as ConstantExpression;
                    return cExp.Value;
                case "UnaryExpression":
                    var unaryExp = exp as UnaryExpression;
                    return GetChildValue(unaryExp.Operand);
                case "InstanceMethodCallExpressionN":
                    var imExp = exp as MethodCallExpression;
                    if (imExp.Arguments.Count > 0)
                    {
                        return GetChildValue(imExp.Arguments[0]);
                    }
                    return null;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 解析表信息
        /// </summary>
        /// <param name="exp"></param>
        private void AnalysisTables(Expression exp)
        {
            var className = exp.GetType().Name;
            switch (className)
            {
                case "PropertyExpression":
                case "FieldExpression":
                    var mberExp = exp as MemberExpression;
                    if (!IsDefaultType(mberExp.Type))
                    {
                        if (!ResultData.TableList.ContainsKey(mberExp.Member.Name))
                        {
                            ResultData.TableList.Add(mberExp.Member.Name, new AnalysisTable()
                            {
                                Name = mberExp.Type.Name,
                                TableType = mberExp.Type,
                                IsMainTable = false
                            });
                        }
                    }
                    AnalysisTables(mberExp.Expression);
                    break;
                case "TypedParameterExpression":
                    //命名参数表达式
                    var texp = exp as ParameterExpression;
                    if (!IsDefaultType(texp.Type))
                    {
                        if (!ResultData.TableList.ContainsKey(_argName))
                        {
                            ResultData.TableList.Add(_argName, new AnalysisTable()
                            {
                                Name = texp.Type.Name,
                                TableType = texp.Type,
                                IsMainTable = true
                            });
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 判断是否系统基础类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool IsNullOrDefaultType(object obj)
        {
            if (null == obj) return true;
            return IsDefaultType(obj.GetType());
        }

        private bool IsDefaultType(Type type)
        {
            string defaultType = @"String|Boolean|Double|Int32|Int64|Int16|Single|DateTime|Decimal|Char|Object|Guid";
            Regex r = new Regex(defaultType, RegexOptions.IgnoreCase);
            if (type.Name.ToLower().Contains("nullable"))
            {
                if (type.GetGenericArguments().Count() > 0)
                {
                    return r.IsMatch(type.GetGenericArguments()[0].Name);
                }
            }
            return r.IsMatch(type.Name);
        }
    }

    /// <summary>
    /// 解析结果
    /// </summary>
    public class AnalysisData
    {
        public AnalysisData()
        {
            StackList = new List<string>();
            ParamList = new Dictionary<string, object>();
            TableList = new Dictionary<string, AnalysisTable>();
        }
        public List<string> StackList { get; set; }
        public Dictionary<string, object> ParamList { get; set; }
        public Dictionary<string, AnalysisTable> TableList { get; set; }
    }

    /// <summary>
    /// 解析表结果
    /// </summary>
    public class AnalysisTable
    {
        public string Name { get; set; }
        public Type TableType { get; set; }
        public bool IsMainTable { get; set; }
    }
}
