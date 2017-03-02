using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities {
    public class ConfigHelper {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key) {
            return ConfigurationManager.AppSettings[key];
        }

        public static string Get(object appSecret) {
            throw new NotImplementedException();
        }
    }
}
