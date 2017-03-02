using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliYunModel;
using Core.Entities;

namespace Core.Authorization {
    public class WexinQyUserManagement : IWexinUserManagement {
        public bool Add(WeixinCustomer entity) {
            throw new NotImplementedException();
        }

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

        private Employee GetEmployee(string userKey) {
            var weixinUser = AppContext.Current.DbContext.Employee.FirstOrDefault(item => item.Code.Equals(userKey));
            return weixinUser;
        }

        public BaseUser Login(string userKey) {
            Customer user = GetUserByCode(userKey);
            if (user == null) {
                return null;
            }
            return new BaseUser {
                Customer = user,
                Employee = GetEmployee(userKey),
            };
        }

        private Customer GetUserByCode(string userKey) {
            var dbcontext = AppContext.Current.DbContext;
            var customer = from weixinCustomer in dbcontext.Employee
                           join cust in dbcontext.Customer on weixinCustomer.CustomerId equals cust.Id
                           where weixinCustomer.Code.Equals(userKey)
                           select cust;
            return customer.Any() ? customer.FirstOrDefault() : null;
        }

        public bool Relevance(string code, int userId) {
            var dbcontext = AppContext.Current.DbContext;
            var weixinUser = dbcontext.Employee.FirstOrDefault(item => item.Code.Equals(code));
            if (weixinUser == null) {
                weixinUser = new Employee {
                    Code = code,
                    Name = "未知",
                    CreatedOn = DateTime.Now,
                    CreatedBy = 0,
                    Status = 0,
                    Deleted = false,
                };
                dbcontext.Employee.Add(weixinUser);
            }
            weixinUser.CustomerId = userId;
            return dbcontext.SaveChanges() > 0;
        }

        public bool UnRelevance(string code) {
            throw new NotImplementedException();
        }
    }
}
