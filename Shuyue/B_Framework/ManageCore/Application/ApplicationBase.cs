using ManageEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using System.Web;

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

        public SQY_ManageEntities DbContext
        {
            get
            {
                return new SQY_ManageEntities();
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
    }
}
