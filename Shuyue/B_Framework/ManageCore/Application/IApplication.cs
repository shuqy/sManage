using Core.Entities;
using ManageEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application
{
    public interface IApplication
    {
        SQY_ManageEntities DbContext { get; }
        BaseUser CurrentUser { get; }
        List<sys_user_menu> UserMenu { get; set; }
    }
}
