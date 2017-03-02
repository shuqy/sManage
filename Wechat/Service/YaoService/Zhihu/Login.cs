using Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YaoService.Zhihu {
    public class Login {
        private HttpHelper _httpHelper;
        private LogHelper _logger;
        private string _userAgent;
        public Login() {
            _httpHelper = new HttpHelper();
            _logger = new LogHelper();
            _userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0";
        }
        public static void EmailLogin(string email, string pwd) {

        }

        //访问登录页获取xsrf值，登陆用
        public string GetXSRF() {
            HttpItem item = new HttpItem {
                URL = "http://www.zhihu.com/",
                UserAgent = _userAgent
            };
            HttpResult result = _httpHelper.GetHtml(item);
            Regex xsrf_regex = new Regex("xsrf\" value=\"(?<xsrf>[^\"]*)");
            string xsrf = xsrf_regex.Match(result.Html).Groups["xsrf"].Value;
            return xsrf;
        }

        public void TryEmailLogin(string email, string pwd) {
            Regex xsrf_regex = new Regex("xsrf\" value = \"(?<res>[^\"]*)");
            string postData = string.Format("_xsrf={0}&password={1}&remember_me=true&email={2}", GetXSRF(), pwd, email);
            HttpItem item = new HttpItem {
                URL = "https://www.zhihu.com/login/email",
                Method = "POST",
                UserAgent = _userAgent,
                Postdata = postData
            };
            HttpResult result = _httpHelper.GetHtml(item);
            var loginRes = JsonConvert.DeserializeObject<ZhihuLoginRes>(result.Html);
            if (loginRes.r == "0") {
                var xsrf = string.Format("_xsrf={0};", LoginSuccess.XSRF);
                var q_c1 = HtmlReg.FindWithBeginAndEnd(result.Cookie, "q_c1", ";");
                var cap_id = HtmlReg.FindWithBeginAndEnd(result.Cookie, "cap_id", ";");
                var n_c = HtmlReg.FindWithBeginAndEnd(result.Cookie, "n_c", ";");
                var z_c0 = HtmlReg.FindWithBeginAndEnd(result.Cookie, "z_c0", ";");
                var unlock_ticket = HtmlReg.FindWithBeginAndEnd(result.Cookie, "unlock_ticket", ";");
                //获取cookie
                var cookie = string.Format("{0}{1}{2}{3}{4}{5}", q_c1, xsrf, cap_id, n_c, z_c0, unlock_ticket);
                LoginSuccess.Cookie = cookie;
            } else {
                //登陆失败
                throw new Exception("loginErr:" + loginRes.msg);
            }
        }
    }
}
