using Core.Entities;
using Core.Enum;
using Core.Util;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application
{
    public interface IApplication
    {
        SQY_ManageEntities ManageDbContext { get; }
        SQY_StockEntities StockDbContext { get; }
        BaseUser CurrentUser { get; }
        List<sys_user_menu> UserMenu { get; set; }

        CommonSqlHelper SqlHelper(SqlTypeEnum sqlTypeEnum);
    }
}
