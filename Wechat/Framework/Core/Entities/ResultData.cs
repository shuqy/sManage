using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities {
    public class ResultData {
        public ResultCode code { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
    }
}
