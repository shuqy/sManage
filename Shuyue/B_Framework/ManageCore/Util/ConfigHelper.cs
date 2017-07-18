using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Util
{
    public class ConfigHelper
    {
        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static ConnectionStringSettings GetConn(string key)
        {
            var values = ConfigurationManager.ConnectionStrings[key];
            if (values == null) throw new Exception();
            return values;
        }

        public static string GetConnStr(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
    }
}
