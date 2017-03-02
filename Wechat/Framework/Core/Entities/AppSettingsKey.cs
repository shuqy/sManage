using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities {
    public class AppSettingsKey {
        public static string AppId { get { return "AppId"; } }

        public static string AppSecret { get { return "AppSecret"; } }

        public static string DebugOpenId { get { return "DEBUG_OPENID"; } }

        public static string WeixinServiceType { get { return "WeixinServiceType"; } }

        public static string WeixinAuthorizationCallbackUrl { get { return "WeixinAuthorizationCallbackUrl"; } }
        public static string IsNeedRedirectToLoginpage { get { return "IsNeedRedirectToLoginpage"; } }

        public static string CorpId { get { return "CorpId"; } }
        public static string CorpSecret { get { return "CorpSecret"; } }
    }
}
