using AliYunModel;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Authorization {
    public class WexinUserManagement : IWexinUserManagement {

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public BaseUser Login(string userKey) {
            Customer user = GetUserByOpenId(userKey);
            if (user == null) {
                return null;
            }
            return new BaseUser {
                Customer = user,
                WeixinCustomer = GetWeixinCustomer(userKey),
            };
        }

        /// <summary>
        /// 创建用户并关联
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public BaseUser CreateAndRelevance(string code) {
            Customer customer = new Customer {
                Username = string.Format("user_{0}", Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10)),
                CreateOn = DateTime.Now,
                Deleted = false,
            };
            var dbcontext = AppContext.Current.DbContext;
            dbcontext.Customer.Add(customer);
            dbcontext.SaveChanges();
            return Relevance(code, customer.Id) ? Login(code) : null;
        }

        /// <summary>
        /// 获取微信用户实体
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        private WeixinCustomer GetWeixinCustomer(string userKey) {
            var weixinUser = AppContext.Current.DbContext.WeixinCustomer.FirstOrDefault(item => item.OpenId.Equals(userKey));
            return weixinUser;
        }


        /// <summary>
        /// 添加微信用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Add(WeixinCustomer entity) {
            var dbContext = AppContext.Current.DbContext;
            dbContext.WeixinCustomer.Add(entity);
            return dbContext.SaveChanges() > 0;
        }


        /// <summary>
        /// 用户实体和第三方用户实体关联
        /// </summary>
        /// <param name="code">第三方身份标识</param>
        /// <param name="userId">用户标识</param>
        /// <returns></returns>
        public bool Relevance(string code, int userId) {
            var dbcontext = AppContext.Current.DbContext;
            var weixinUser = dbcontext.WeixinCustomer.FirstOrDefault(item => item.OpenId.Equals(code));
            if (weixinUser == null) {
                weixinUser = new WeixinCustomer {
                    OpenId = code,
                    Nickname = "非关注用户",
                    CreateOn = DateTime.Now,
                    Deleted = false,
                };
                dbcontext.WeixinCustomer.Add(weixinUser);
            }
            weixinUser.CustomerId = userId;
            return dbcontext.SaveChanges() > 0;
        }


        /// <summary>
        /// 取消用户实体和第三方用户实体关联
        /// </summary>
        /// <param name="code">第三方身份标识</param>
        /// <returns></returns>
        public bool UnRelevance(string code) {
            var dbcontext = AppContext.Current.DbContext;
            var weixinUser = dbcontext.WeixinCustomer.FirstOrDefault(item => item.OpenId.Equals(code));
            if (weixinUser == null) return false;
            weixinUser.CustomerId = null;
            return dbcontext.SaveChanges() > 0;
        }


        private Customer GetUserByOpenId(string userKey) {
            var dbcontext = AppContext.Current.DbContext;
            var customer = from weixinCustomer in dbcontext.WeixinCustomer
                           join cust in dbcontext.Customer on weixinCustomer.CustomerId equals cust.Id
                           where weixinCustomer.OpenId.Equals(userKey)
                           select cust;
            return customer.Any() ? customer.FirstOrDefault() : null;
        }
    }
}
