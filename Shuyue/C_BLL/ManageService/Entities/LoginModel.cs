using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageService.Entities
{
    public class LoginModel
    {
        public string UserCode { get; set; }
        public string PassCode { get; set; }
        public string Redirect { get; set; }
    }
}
