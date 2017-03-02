using AliYunModel;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application {
    public abstract class ApplicationBase : IApplication {
        public virtual BaseUser CurrentUser {
            get {
                return null;
            }
        }

        public AliDbEntities DbContext {
            get {
                return new AliDbEntities();
            }
        }


    }
}
