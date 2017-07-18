using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using System.Web;
using Model;
using Core.Util;
using Core.Enum;

namespace Core.Application
{
    public class ApplicationBase : IApplication
    {

        public BaseUser CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session[SessionKey.CurrentUser] == null)
                {
                    //判断cookie是否存在
                    if (CookieHelper.GetCookie("manageuser", "username") != null && CookieHelper.GetCookie("manageuser", "password") != null)
                    {
                        var u = Encryptor.DecryptDES(CookieHelper.GetCookie("manageuser", "username"));
                        var p = CookieHelper.GetCookie("manageuser", "password");
                        var user = this.ManageDbContext.sys_user.FirstOrDefault(s => s.UserCode == u && s.PassCode == p && !s.Deleted);
                        if (user != null)
                        {
                            BaseUser baseUser = new BaseUser
                            {
                                Id = user.Id,
                                UserCode = user.UserCode,
                                UserName = user.UserName,
                                State = (int)user.State,
                            };
                            HttpContext.Current.Session[SessionKey.CurrentUser] = baseUser;
                            return baseUser;
                        }
                    }
                    //开启调试模式
                    if (Util.ConfigHelper.Get("IsDebug") == "1")
                    {
                        BaseUser user = new BaseUser();
                        user.Id = Convert.ToInt32(Util.ConfigHelper.Get("DebugUserId"));
                        return user;
                    }
                    return null;
                }
                return (BaseUser)HttpContext.Current.Session[SessionKey.CurrentUser];
            }
        }

        /// <summary>
        /// 获取数据库EF上下文
        /// </summary>
        public SQY_ManageEntities ManageDbContext
        {
            get
            {
                return new SQY_ManageEntities();
            }
        }

        /// <summary>
        /// 获取数据库EF上下文
        /// </summary>
        public SQY_StockEntities StockDbContext
        {
            get
            {
                return new SQY_StockEntities();
            }
        }

        public List<sys_user_menu> UserMenu
        {
            get
            {
                if (CurrentUser == null || HttpContext.Current.Session[SessionKey.UserMenu] == null) return null;
                return HttpContext.Current.Session[SessionKey.UserMenu] as List<sys_user_menu>;
            }
            set
            {
                if (value == null)
                {
                    HttpContext.Current.Session.Remove(SessionKey.UserMenu);
                }
                else
                {
                    HttpContext.Current.Session[SessionKey.UserMenu] = value;
                }
            }
        }

        /// <summary>
        /// 获取数据库连接(弃用)
        /// </summary>
        /// <param name="sqlTypeEnum"></param>
        /// <returns></returns>
        public CommonSqlHelper SqlHelper(SqlTypeEnum sqlTypeEnum)
        {
            CommonSqlHelper sqlHelper;
            switch (sqlTypeEnum)
            {
                case SqlTypeEnum.Manage:
                    sqlHelper = new CommonSqlHelper(ConfigHelper.GetConn("ManageConn").ConnectionString);
                    break;
                case SqlTypeEnum.Stock:
                    sqlHelper = new CommonSqlHelper(ConfigHelper.GetConn("StockConn").ConnectionString);
                    break;
                default:
                    sqlHelper = new CommonSqlHelper(ConfigHelper.GetConn("ManageConn").ConnectionString);
                    break;
            }
            return sqlHelper;
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="dbConnEnum"></param>
        /// <returns></returns>
        public CommonSqlUtility ESqlUtil(DbConnEnum dbConnEnum)
        {
            CommonSqlUtility sqlHelper;
            switch (dbConnEnum)
            {
                case DbConnEnum.ZhiHu:
                    sqlHelper = new CommonSqlUtility(ConfigHelper.GetConn("ZhiHuConnStr").ConnectionString);
                    break;
                default:
                    sqlHelper = new CommonSqlUtility(ConfigHelper.GetConn(dbConnEnum.ToString()).ConnectionString);
                    break;
            }
            return sqlHelper;
        }

        /// <summary>
        /// Dapper帮助类
        /// </summary>
        /// <param name="dbConnEnum"></param>
        /// <returns></returns>
        public DapperHelper Dapper(DbConnEnum dbConnEnum)
        {
            return new DapperHelper(dbConnEnum);
        }
    }
}
