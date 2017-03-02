using AliYunModel;
using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YaoService.LeShare {
    public class LeShareService {
        private HttpHelper _httpHelper;
        //用户浏览器标志
        private string UserAgent;

        private LogHelper lh;
        private string LeShareUrl;
        public LeShareService() {
            _httpHelper = new HttpHelper();
            UserAgent = "Mozilla / 5.0(Windows NT 10.0; WOW64; rv: 44.0) Gecko / 20100101 Firefox / 44.0";
            lh = new LogHelper();
            LeShareUrl = ConfigHelper.Get("LeShareUrl");
        }
        /// <summary>
        /// 登陆并返回获取到的cookie
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private string Login(string userName, string password) {
            string _successCookie;
            //访问登陆页面，拿到cookie和页面登陆post请求必须的__RequestVerificationToken值
            HttpItem loginPage = new HttpItem {
                URL = LeShareUrl + "/Account/Login?ReturnUrl=%2f",
                UserAgent = UserAgent,
            };
            //访问登陆页面结果
            HttpResult loginPageResult = _httpHelper.GetHtml(loginPage);

            //取值正则
            Regex pageRequestVerificationTokenRegex = new Regex("\"__RequestVerificationToken\" type=\"hidden\" value=\"(?<res>[^\"]*)",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);
            Regex sessionIdRegex = new Regex(@"(?<res>[^;]*)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            Regex cookieRequestVerificationTokenRegex = new Regex(@"__RequestVerificationToken=(?<res>[^;]*)",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            //页面RequestVerificationToken值
            string pageRequestVerificationToken = pageRequestVerificationTokenRegex.Match(loginPageResult.Html).Groups["res"].Value;
            //得到sessionId
            string sessionId = sessionIdRegex.Match(loginPageResult.Cookie).Groups["res"].Value;
            //cookie里的RequestVerificationToken值
            string cookieRequestVerificationToken = cookieRequestVerificationTokenRegex.Match(loginPageResult.Cookie).Groups["res"].Value;

            //登陆请求的cookie
            string cookie = sessionId + ";__RequestVerificationToken=" + cookieRequestVerificationToken;
            //请求数据
            string postdata = string.Format("__RequestVerificationToken={0}&UserName={1}&Password={2}&RememberMe={3}",
                pageRequestVerificationToken, userName, password, "false");
            //登陆请求
            HttpItem loginrequest = new HttpItem {
                URL = LeShareUrl + "/Account/Login?ReturnUrl=%2f",
                Method = "POST",
                Cookie = cookie,
                Referer = LeShareUrl + "/Account/Login?ReturnUrl=%2f",
                Accept = "text/html, */*; q=0.01",
                UserAgent = this.UserAgent,
                Postdata = postdata
            };
            //登陆结果
            HttpResult loginResult = _httpHelper.GetHtml(loginrequest);

            //获取cookie中ASPXAUTH值
            string ASPXAUTH = sessionIdRegex.Match(loginResult.Cookie).Groups["res"].Value;

            //登陆成功后cookie
            _successCookie = string.Format("{0};{1}", sessionId, ASPXAUTH);
            return _successCookie;
        }

        public bool CheckLogin(string username, string password) {
            string _successCookie = Login(username, password);
            HttpItem checkPage = new HttpItem {
                URL = LeShareUrl + "/Account/Index",
                UserAgent = UserAgent,
                Cookie = _successCookie
            };
            HttpResult checkResult = _httpHelper.GetHtml(checkPage);
            return checkResult.Html.Contains("发现");
        }

        public bool AutoSignIn(SignModel signInModel) {
            try {
                if (string.IsNullOrEmpty(signInModel.SuccessCookie))
                    signInModel.SuccessCookie = Login(signInModel.MansUser.Mobile, signInModel.MansUser.LeSharePwd);
                string postData = string.Format("category={0}&signAtAddress={1}&signAtLng={2}&&signAtLat={3}",
                    (int)signInModel.AutoSignUser.NextSignType, "%E5%8C%97%E4%BA%AC%E5%B8%82%E5%8C%97%E4%BA%AC%E5%B8%82%E6%98%8C%E5%B9%B3%E5%8C%BA%E5%9B%9E%E9%BE%99%E8%A7%82%E4%B8%9C%E5%A4%A7%E8%A1%97199%E5%8F%B7", 116.3476, 40.08022);
                HttpItem autoSignItem = new HttpItem {
                    URL = LeShareUrl + "/requestforleave/SignIn",//签到地址
                    UserAgent = "User-Agent: Mozilla/5.0 (iPhone; CPU iPhone OS 9_2 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Mobile/13C75 MicroMessenger/6.3.13 NetType/WIFI Language/zh_CN",
                    Method = "POST",
                    Postdata = postData,
                    Cookie = signInModel.SuccessCookie
                };
                HttpResult signResult = _httpHelper.GetHtml(autoSignItem);
                bool result = signResult.Html.Contains("成功");
                lh.Write(new Msg {
                    Datetime = DateTime.Now,
                    Type = MsgType.Information,
                    Text = string.Format("用户：{0}，{1}打卡，结果：{2}。",
                    signInModel.MansUser.RealName, signInModel.AutoSignUser.NextSignType.ToString(), result),
                });
                return result;

            } catch (Exception ex) {
                lh.Write(new Msg {
                    Datetime = DateTime.Now,
                    Type = MsgType.Error,
                    Text = string.Format("用户：{0}，{1}打卡异常，异常信息：{2}。",
                    signInModel.MansUser.RealName, signInModel.AutoSignUser.NextSignType.ToString(), ex.Message),
                });
                return false;
            }
        }


        public IList<MansUser> GetMansUserList() {
            string cookie = Login("13269276932", "123456");
            HttpItem item = new HttpItem {
                URL = LeShareUrl + "/User/Contact",
                Cookie = cookie,
            };
            HttpResult result = _httpHelper.GetHtml(item);
            Regex contentRegex = new Regex("<li class(?<content>[\\s\\S]*?)</li>");
            Match content = contentRegex.Match(result.Html);
            IList<MansUser> mansusers = new List<MansUser>();
            while (content.Success) {
                string contentStr = content.Groups["content"].Value;
                string headImg = LeShareUrl + new Regex(@"url(?<img>[^)]*)").Match(contentStr).Groups["img"].Value.Replace("(", "");
                string realName = new Regex(@"<h2>(?<realname>[^<]*)").Match(contentStr).Groups["realname"].Value;
                string mobile = new Regex(@"tel:(?<tel>[^""]*)").Match(contentStr).Groups["tel"].Value;
                string email = new Regex(@"mailto:(?<email>[^""]*)").Match(contentStr).Groups["email"].Value;
                string id = new Regex(@"data-id=""(?<id>[^""]*)").Match(contentStr).Groups["id"].Value;
                mansusers.Add(new MansUser {
                    HeadImg = headImg,
                    Email = email,
                    Mobile = mobile,
                    RealName = realName,
                    Deleted = false,
                    LeShareUserName = id
                });
                content = content.NextMatch();
            }
            return mansusers;
        }
    }
}
