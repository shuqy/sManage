using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Util
{
    public class SqlFieldsUtil
    {
        private static Dictionary<string, string> FieldsDic;
        private static Func<string, string> AddStr = s => string.Format("Add_{0}", s);
        private static Func<string, string> UpStr = s => string.Format("Update_{0}", s);
        public static void InitFields()
        {
            if (FieldsDic != default(Dictionary<string, string>)) return;
            FieldsDic = new Dictionary<string, string>
            {
                //通知
                {AddStr("BugNotice"),"Siteid,UserId,Title,Content,Summary,UpdateUserID,UpdateTime,CreateTime,IsDel" },
                {UpStr("BugNotice"),"Title,Content,Summary,UpdateUserID,UpdateTime" },
                //企业信息编辑
                {UpStr("dept3e21_company"),"dept_name,name,tell" },
                {AddStr("dept3e21_department_position"),"dept_name,parent_dept,beizhu,state,isdeleted,addtime,addusername" },
                {UpStr("dept3e21_department_position"),"dept_name,beizhu" },
                //
                {AddStr("user3e21erp"),"username,password,pic,name,sex,born,degree,tel,oicq,state,companyno,marry,roleid" },
                {UpStr("user3e21erp"),"username,password,pic,name,sex,born,degree,tel,oicq,state,marry,roleid" },
                //项目
                {AddStr("BugProject"),"CreateTime,CreateBy,UpdateTime,UpdateBy,Siteid,UserId,Title,Explain,proType,proState,IsDel" },
                {UpStr("BugProject"),"UpdateBy,UpdateTime,Title,Explain,proType" },
                {UpStr("BugProjectState"),"proState" },
                //更改状态
                {UpStr("UpdateState"),"State" },
            };
        }
        public static string[] GetFields(string tableName, bool isAdd)
        {
            InitFields();
            string s = isAdd ? AddStr(tableName) : UpStr(tableName);
            return FieldsDic.ContainsKey(s) ? FieldsDic[s].Split(',') : null;
        }
    }
}
