using Core;
using EF.Entity.Core;
using ManageEF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManageService
{
    public class RepositoryBase<TEntity> : DbContextRepository<TEntity> where TEntity : class
    {
        //空构造函数，传null所继承的构造函数会传递默认实体
        public RepositoryBase() : this(null)
        { }

        public RepositoryBase(IUnitOfWork unitOfWork) : base(unitOfWork ?? new ExtManageEntities())
        { }
    }
}
