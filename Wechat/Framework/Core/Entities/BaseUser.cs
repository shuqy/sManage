using AliYunModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities {
    public class BaseUser {
        public Customer Customer { get; set; }
        public WeixinCustomer WeixinCustomer { get; set; }
        public Employee Employee { get; set; } 
    }
}
