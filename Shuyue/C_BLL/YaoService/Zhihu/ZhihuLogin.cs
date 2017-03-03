using Core.Html;
using Core.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YaoService.Zhihu
{
    public class ZhihuLogin
    {
        private string _cookie;
        private HttpHelper _httpHelper;
        private LogHelper _logger;
        private string _userAgent;
        private string _logincode;
        private string _pwd;
        public ZhihuLogin()
        {
            _logincode = "617086902@qq.com";//ConfigHelper.Get("ZhihuUser")
            _pwd = "2131402780";//ConfigHelper.Get("ZhihuPwd")
            _httpHelper = new HttpHelper();
            _logger = new LogHelper();
            _userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0";
        }

        public ZhihuLogin(string logincode, string pwd) : this()
        {
            _logincode = logincode;
            _pwd = pwd;
        }

        public string Cookie
        {
            get
            {
                if (string.IsNullOrEmpty(_cookie))
                {
                    TryEmailLogin(_logincode, _pwd);
                }
                return _cookie;
            }
            set
            {
                _cookie = value;
            }
        }

        /// <summary>
        /// 访问登录页获取xsrf值，登陆用
        /// </summary>
        /// <returns></returns>
        public string GetXSRF()
        {
            HttpItem item = new HttpItem
            {
                URL = "http://www.zhihu.com/",
                UserAgent = _userAgent
            };
            HttpResult result = _httpHelper.GetHtml(item);
            Regex xsrf_regex = new Regex("_xsrf\" value=\"(?<xsrf>[^\"]*)");
            string xsrf = xsrf_regex.Match(result.Html).Groups["xsrf"].Value;
            return xsrf;
        }

        /// <summary>
        /// 邮箱登录
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pwd"></param>
        public void TryEmailLogin(string email, string pwd)
        {
            var _xsrf = GetXSRF();
            string postData = string.Format("_xsrf={0}&password={1}&remember_me=true&email={2}", _xsrf, pwd, email);
            HttpItem item = new HttpItem
            {
                URL = "https://www.zhihu.com/login/email",
                Method = "POST",
                UserAgent = _userAgent,
                Postdata = postData
            };
            HttpResult result = _httpHelper.GetHtml(item);
            var loginRes = JsonConvert.DeserializeObject<ZhihuLoginRes>(result.Html);
            if (loginRes.r == "0")
            {
                var xsrf = string.Format("_xsrf={0};", _xsrf);
                var q_c1 = MelonReg.FindWithBeginAndEnd(result.Cookie, "q_c1", ";");
                var cap_id = MelonReg.FindWithBeginAndEnd(result.Cookie, "cap_id", ";");
                var n_c = MelonReg.FindWithBeginAndEnd(result.Cookie, "n_c", ";");
                var z_c0 = MelonReg.FindWithBeginAndEnd(result.Cookie, "z_c0", ";");
                var unlock_ticket = MelonReg.FindWithBeginAndEnd(result.Cookie, "unlock_ticket", ";");
                //获取cookie
                var cookie = string.Format("{0}{1}{2}{3}{4}{5}", q_c1, xsrf, cap_id, n_c, z_c0, unlock_ticket);
                Cookie = cookie;
            }
            else
            {
                //登陆失败
                throw new Exception("loginErr:" + loginRes.msg);
            }
        }
    }
}
