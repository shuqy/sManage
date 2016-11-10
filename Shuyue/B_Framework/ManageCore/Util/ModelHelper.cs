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
    /// 实体帮助类
    /// </summary>
    public class ModelHelper
    {
        /// <summary>
        /// 数据拷贝 (将第一个实体值赋值给第二个实体;忽略大小写) 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="M"></typeparam>
        /// <param name="t"></param>
        /// <param name="m"></param>
        public static void ModelCopyTo<T, M>(T t, M m)
        {
            Type t1 = t.GetType();
            Type t2 = m.GetType();
            PropertyInfo[] pros = t1.GetProperties();
            foreach (var propertyinfo in pros)
            {
                PropertyInfo p = t2.GetProperty(propertyinfo.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                if (p == null)//第二个实体没有这个字段则不赋值
                {
                    continue;
                }
                object obj = propertyinfo.GetValue(t, null);//t的属性值                
                object obj_m = p.GetValue(m, null);//m的属性值
                if (p.PropertyType == typeof(string))//字符串类型
                {
                    if (obj == null && obj_m == null)
                    {
                        obj = string.Empty;
                    }
                    else if (obj == null)//第二个实体类字段有值
                    {
                        obj = obj_m;
                    }
                }
                if (p.PropertyType == typeof(DateTime))//时间类型
                {
                    if (Convert.ToDateTime(obj).Ticks == 0 && Convert.ToDateTime(obj_m).Ticks == 0)
                    {
                        obj = DateTime.Now;
                    }
                    else if (Convert.ToDateTime(obj).Ticks == 0)//第二个实体类时间有值
                    {
                        obj = obj_m;
                    }
                }
                p.SetValue(m, obj, null);
            }
        }

        /// <summary>
        /// 使用DataTable填充List实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> FillModelList<T>(DataTable dt) where T : new()
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            IList<T> listT = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                t = FillModel<T>(dr);
                listT.Add(t);
            }
            return listT;
        }
        /// <summary>  
        /// 填充对象：用DataRow填充实体类 
        /// </summary>  
        public static T FillModel<T>(DataRow dr) where T : new()
        {
            if (dr == null)
            {
                return default(T);
            }

            T model = new T();
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                if (dr.Table.Columns.Contains(propertyInfo.Name))
                {
                    if (dr[propertyInfo.Name] != DBNull.Value)
                    {
                        model.GetType().GetProperty(propertyInfo.Name).SetValue(model, dr[propertyInfo.Name], null);
                    }
                }
            }
            return model;
        }
    }
}
