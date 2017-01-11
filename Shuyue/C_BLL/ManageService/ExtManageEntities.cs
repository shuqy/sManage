using EF.Entity.Core;
using Model;

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
