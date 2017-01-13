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
            return ConfigurationManager.ConnectionStrings[key];
        }

        public static string GetConnStr(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
    }
}
