using AliYunModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaoService.LeShare {
    public class AutoSignDbService {
        private AliDbEntities dbcontext;
        public AutoSignDbService() {
            dbcontext = new AliDbEntities();

        }
        public bool AddNewAutoSignUser(SignModel signModel) {
            //var res = from s in dbcontext.MansUser
            //          join a in dbcontext.AutoSignUser on s.Id equals a.MansUserId
            //          where s.Mobile.Equals(signModel.)
            return false;
        }
        /// <summary>
        /// 根据登录名获取用户
        /// </summary>
        /// <param name="loginUserName"></param>
        /// <returns></returns>
        public MansUser GetMansUserByLoginUserName(string loginUserName) {
            return dbcontext.MansUser.FirstOrDefault(m => m.Mobile.Equals(loginUserName)
            || m.RealName.Equals(loginUserName)
            || m.Email.Equals(loginUserName));
        }
        /// <summary>
        /// 检查是否能登陆
        /// </summary>
        /// <param name="loginUserName"></param>
        /// <returns></returns>
        public bool CheckIsAutoSign(string loginUserName) {
            MansUser mansUser = GetMansUserByLoginUserName(loginUserName);
            if (mansUser == null) return false;
            if (mansUser.IsAutoSign == true) return false;
            return true;
        }
        /// <summary>
        /// 更新用户登录密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public MansUser UpdateMansUser(string username, string pwd) {
            MansUser mansUser = GetMansUserByLoginUserName(username);
            if (mansUser.LeSharePwd == pwd && mansUser.IsAutoSign.Equals(true)) return mansUser;
            mansUser.LeSharePwd = pwd;
            mansUser.IsAutoSign = true;
            if (dbcontext.SaveChanges() > 0) return mansUser;
            return null;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="mansUserList"></param>
        /// <returns></returns>
        public bool AddMansUser(IList<MansUser> mansUserList) {
            dbcontext.MansUser.AddRange(mansUserList);
            return dbcontext.SaveChanges() > 0;
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="mansUser"></param>
        /// <returns></returns>
        public bool AddMansUser(MansUser mansUser) {
            dbcontext.MansUser.Add(mansUser);
            return dbcontext.SaveChanges() > 0;
        }
        /// <summary>
        /// 添加或更新自动签到信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="signDatetime"></param>
        /// <param name="signType"></param>
        /// <returns></returns>
        public AutoSignUser AddOrUpdateAutoSignUser(int userId, DateTime signDatetime, SignType signType) {
            AutoSignUser autoSignUser = dbcontext.AutoSignUser.FirstOrDefault(a => a.MansUserId.Equals(userId));
            if (autoSignUser == null) {
                autoSignUser = new AutoSignUser {
                    MansUserId = userId,
                    NextSignDateTime = signDatetime,
                    NextSignType = (int)signType,
                    SuccessTimes = 0,
                    CreatedDate = DateTime.Now
                };
                dbcontext.AutoSignUser.Add(autoSignUser);
            } else {
                autoSignUser.NextSignDateTime = signDatetime;
                autoSignUser.NextSignType = (int)signType;
                autoSignUser.SuccessTimes = autoSignUser.SuccessTimes + 1;
            }
            dbcontext.SaveChanges();
            return autoSignUser;
        }
        /// <summary>
        /// 添加自动签到历史纪录
        /// </summary>
        /// <param name="autoSignHistory"></param>
        /// <returns></returns>
        public bool AddAutoSignHistory(AutoSignHistory autoSignHistory) {
            dbcontext.AutoSignHistory.Add(autoSignHistory);
            return dbcontext.SaveChanges() > 0;
        }
        /// <summary>
        /// 获取自动签到列表
        /// </summary>
        /// <returns></returns>
        public IList<SignModel> GetAutoSignList() {
            var res = from u in dbcontext.MansUser
                      join a in dbcontext.AutoSignUser on u.Id equals a.MansUserId
                      where u.IsAutoSign == true
                      select new SignModel {
                          MansUser = u,
                          AutoSignUser = a,
                      };
            return res.ToList();
        }

        public string GetCodeByMobile(string mobile) {
            var res = from a in dbcontext.Customer
                      join e in dbcontext.Employee on a.Id equals e.CustomerId
                      where a.Mobile == mobile
                      select e.Code;
            return res.Any() ? res.FirstOrDefault() : "";
        }

        public IList<SignHistory> GetSignHistory() {
            var res = from h in dbcontext.AutoSignHistory
                      join m in dbcontext.MansUser on h.MansUserId equals m.Id
                      select new SignHistory {
                          MansUser = m,
                          AutoSignHistory = h,
                          SignType = h.SignType == 1 ? "上班" : "下班"
                      };
            return res.OrderByDescending(a => a.AutoSignHistory.Id).ToList();
        }

        public bool ChangeNextSignDateTime(int userId, DateTime signDateTime) {
            var result = dbcontext.AutoSignUser.FirstOrDefault(a => a.MansUserId == userId);
            if (result == null) return false;
            result.NextSignDateTime = signDateTime;
            return dbcontext.SaveChanges() > 0;
        }

        public bool CloseAutoSign(int userId) {
            var msuser = dbcontext.MansUser.FirstOrDefault(m => m.Id == userId);
            if (msuser == null) return false;
            msuser.IsAutoSign = false;
            return dbcontext.SaveChanges() > 0;
        }
    }
}
