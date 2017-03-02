using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities {
    public class RandomOperate {
         
        public static string RansdomName(int length) {
            Guid g = Guid.NewGuid();
            return g.ToString().Replace("-", "").Substring(length);
        }
    }
}
