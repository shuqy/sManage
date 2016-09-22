using EF.Entity.Core;
using ManageEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageService
{
    /// <summary>
    /// 继承IUnitOfWork，便于实体传递
    /// </summary>
    public partial class ExtManageEntities : SQY_ManageEntities, IUnitOfWork
    {
        public bool IsExplicitSubmit { get; set; }

        public void Save()
        {
            this.SaveChanges();
        }
    }
}
