using AliYunModel;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application {
    public interface IApplication {
        AliDbEntities DbContext { get; }

        BaseUser CurrentUser { get; }
    }
}
