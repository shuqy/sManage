using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities {
    public class SessionKey {
        public static string CurrentUser = "CurrentUser";

        public static string OAuthAccessTokenResult = "OAuthAccessTokenResult";

        public static string OpenId { get { return "OpenId"; } }
        public static string WeixinCode { get { return "WeixinCode"; } }
    }
}
