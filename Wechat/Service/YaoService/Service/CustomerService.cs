using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliYunModel;
namespace YaoService.Service {
    public class CustomerService {

        /// <summary>
        /// 用户绑定手机
        /// 顺便根据手机号，绑定用户与mans用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public bool BindMobile(int userId, string mobile) {
            var dbcontext = AppContext.Current.DbContext;
            var customer = dbcontext.Customer.FirstOrDefault(a => a.Id == userId);
            var mansuser = dbcontext.MansUser.FirstOrDefault(a => a.Mobile == mobile);
            if (customer == null || mansuser == null) return false;
            customer.Mobile = mobile;
            customer.Email = mansuser.Email;
            customer.Username = mansuser.RealName;
            customer.Realname = mansuser.RealName;
            Customer_MansUser_Mapping cmm = new Customer_MansUser_Mapping {
                CustomerId = customer.Id,
                MansUserId = mansuser.Id,
            };
            dbcontext.Customer_MansUser_Mapping.Add(cmm);
            return dbcontext.SaveChanges() > 0;
        }
    }
}
