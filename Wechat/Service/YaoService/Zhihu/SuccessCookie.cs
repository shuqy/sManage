using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaoService.Zhihu {
    public class LoginSuccess {
        private static string _cookie;
        private static string _xsrf;
        public static string Cookie {
            get {
                if (string.IsNullOrEmpty(_cookie)) {
                    string email = "617086902@qq.com";//ConfigHelper.Get("ZhihuUser")
                    string pwd = "t123456";//ConfigHelper.Get("ZhihuPwd")
                    Login login = new Login();
                    login.TryEmailLogin(email, pwd);
                }
                return _cookie;
            }
            set {
                _cookie = value;
            }
        }

        public static string XSRF {
            get {
                if (string.IsNullOrEmpty(_xsrf)) {
                    Login login = new Login();
                    _xsrf = login.GetXSRF();
                }
                return _xsrf;
            }
        }
    }
}
