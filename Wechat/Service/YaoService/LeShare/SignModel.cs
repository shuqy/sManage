using AliYunModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaoService.LeShare {
    public class SignModel {
        public MansUser MansUser { get; set; }
        public AutoSignUser AutoSignUser { get; set; }
        public Employee Employee { get; set; }
        public string SuccessCookie { get; set; }
    }

    public class SignHistory {
        public MansUser MansUser { get; set; }
        public AutoSignHistory AutoSignHistory { get; set; } 
        public string SignType { get; set; }
    }
}
