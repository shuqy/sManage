using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Application {
    public class WebApplication : ApplicationBase {

        public override BaseUser CurrentUser {
            get {
                return HttpContext.Current.Session[SessionKey.CurrentUser] as BaseUser;
            }
        }
    }
}
