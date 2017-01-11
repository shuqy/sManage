using EF.Entity.Core;

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
