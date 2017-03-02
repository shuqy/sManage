using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeixinService.Common;

namespace YaoService.LeShare {
    public static class AutoSign {
        //签到列表
        public static IList<SignModel> SignList { get; set; }
        static AutoSign() {
            SignList = new List<SignModel>();
        }

        public static bool GetMansUser() {
            var mansUserList = new LeShareService().GetMansUserList();
            return new AutoSignDbService().AddMansUser(mansUserList);
        }

        public static bool CheckIsAutoSign(string loginusername) {
            AutoSignDbService autoSignDbService = new AutoSignDbService();
            if (!SignList.Any()) SignList = autoSignDbService.GetAutoSignList();
            return SignList.Any(s => s.MansUser.Mobile.Equals(loginusername));
        }

        /// <summary>
        /// 获取首次加入系统用户签到模型，次日生效
        /// </summary>
        /// <returns></returns>
        public static SignModel GetCurrentSignModel(string username, string password) {
            AutoSignDbService autoSignDbService = new AutoSignDbService();
            bool loginResult = new LeShareService().CheckLogin(username, password);
            if (!loginResult) return null;
            SignModel signModel = new SignModel();
            signModel.MansUser = autoSignDbService.UpdateMansUser(username, password);
            if (signModel.MansUser == null) return null;
            signModel.AutoSignUser = autoSignDbService.AddOrUpdateAutoSignUser(signModel.MansUser.Id, GetSignInTime(), SignType.上班);
            if (signModel.AutoSignUser == null) return null;
            return signModel;
        }

        public static SignModel GetAutoSignUser(string username) {
            return AutoSign.SignList.FirstOrDefault(u => u.MansUser.Mobile.Equals(username));
        }

        /// <summary>
        /// 自动签到
        /// </summary>
        public static void AutoSignInHandle() {
            LogHelper lh = new LogHelper();
            AutoSignDbService autoSignDbService = new AutoSignDbService();
            LeShareService leShareService = new LeShareService();
            int[] signinTimeZone = { 8, 9 };
            int[] signoutTimeZone = { 18, 19, 20, 21 };
            DateTime dt = DateTime.Now;
            if (!SignList.Any()) {
                SignList = autoSignDbService.GetAutoSignList();
                if (!SignList.Any())
                    return;
            }
            if (!signinTimeZone.Contains(dt.Hour) && !signoutTimeZone.Contains(dt.Hour)) return;
            foreach (SignModel signinModel in SignList) {
                bool signResult = false;
                if (!((DateTime.Now - signinModel.AutoSignUser.NextSignDateTime).TotalSeconds > 0)) continue;
                signResult = leShareService.AutoSignIn(signinModel);
                if (!signResult) {
                    var code = autoSignDbService.GetCodeByMobile(signinModel.MansUser.Mobile);
                    if (!string.IsNullOrEmpty(code))
                        WeixinQyMsgHelper.SendMsg(code, ConfigHelper.Get("YaoAgentId"), "打卡失败，打卡失败，打卡失败，请检查！");
                    lh.Write(new Msg {
                        Datetime = DateTime.Now,
                        Type = MsgType.Information,
                        Text = string.Format("用户：{0}，{1}打卡异常，请检查日志。",
                        signinModel.MansUser.RealName, signinModel.AutoSignUser.NextSignType.ToString()),
                    });
                    continue;
                }
                autoSignDbService.AddAutoSignHistory(new AliYunModel.AutoSignHistory {
                    MansUserId = signinModel.MansUser.Id,
                    SignDateTime = signinModel.AutoSignUser.NextSignDateTime,
                    SignType = signinModel.AutoSignUser.NextSignType
                });
                if (signinModel.AutoSignUser.NextSignType.Equals((int)SignType.上班)) {
                    signinModel.AutoSignUser = autoSignDbService.AddOrUpdateAutoSignUser(signinModel.MansUser.Id, GetSignOutTime(), SignType.下班);
                } else {
                    signinModel.AutoSignUser = autoSignDbService.AddOrUpdateAutoSignUser(signinModel.MansUser.Id, GetSignInTime(), SignType.上班);
                }
            }
        }


        /// <summary>
        /// 获取下次上班签到时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetSignInTime() {
            DateTime dt = DateTime.Now;
            Random rd = new Random();
            int addDay = 1;
            //如果是星期五
            if (dt.DayOfWeek.Equals(DayOfWeek.Friday)) addDay = 3;
            //如果是星期六
            else if (dt.DayOfWeek.Equals(DayOfWeek.Saturday)) addDay = 2;
            dt = dt.AddDays(addDay);
            return new DateTime(dt.Year, dt.Month, dt.Day, 9, rd.Next(0, 29), rd.Next(0, 59));
        }

        /// <summary>
        /// 获取当天下班签到时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetSignOutTime() {
            DateTime dt = DateTime.Now;
            Random rd = new Random();
            return new DateTime(dt.Year, dt.Month, dt.Day, 19, rd.Next(0, 29), rd.Next(0, 59));
        }

        public static IList<SignHistory> GetSignHistory() {
            AutoSignDbService autoSignDbService = new AutoSignDbService();
            return autoSignDbService.GetSignHistory();
        }

        public static bool ChangeNextSignDateTime(int userId, DateTime signDateTime) {
            AutoSignDbService autoSignDbService = new AutoSignDbService();
            if (autoSignDbService.ChangeNextSignDateTime(userId, signDateTime)) {
                SignList = autoSignDbService.GetAutoSignList();
                return true;
            }
            return false;
        }

        public static bool CloseAutoSign(int userId) {
            AutoSignDbService autoSignDbService = new AutoSignDbService();
            if (autoSignDbService.CloseAutoSign(userId)) {
                SignList = autoSignDbService.GetAutoSignList();
                return true;
            }
            return false;
        }
    }

}
