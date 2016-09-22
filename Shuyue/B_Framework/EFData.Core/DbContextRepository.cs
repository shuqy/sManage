using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EF.Entity.Core
{
    public class DbContextRepository<TEntity> : IExtensionRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 数据上下文
        /// </summary>
        protected DbContext Db { get; private set; }

        /// <summary>
        /// 工作单元上下文,子类可以直接使用它
        /// </summary>
        protected IUnitOfWork UnitWork { get; set; }

        public DbContextRepository(IUnitOfWork db)
        {
            UnitWork = db;
            Db = (DbContext)db;
        }

        /// <summary>
        /// 根据工作单元的IsNotSubmit的属性，去判断是否提交到数据库
        /// 一般地，在多个repository类型进行组合时，这个IsNotSubmit都会设为true，即不马上提交，
        /// 而对于单个repository操作来说，它的值不需要设置，使用默认的false，将直接提交到数据库，这也保证了操作的原子性。
        /// </summary>
        protected void SaveChanges()
        {
            if (!UnitWork.IsExplicitSubmit)
                UnitWork.Save();
        }

        #region IRepository<T>方法实现

        /// <summary>
        /// 根据主键id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity Find(params object[] id)
        {
            return Db.Set<TEntity>().Find(id);
        }

        /// <summary>
        /// 返回原生态结果集
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetEntities()
        {
            return Db.Set<TEntity>();
        }

        /// <summary>
        /// 返回一个结果集，不在EF中缓存
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetModel()
        {
            return Db.Set<TEntity>().AsNoTracking();
        }

        public void Insert(TEntity item)
        {
            Db.Entry<TEntity>(item);
            Db.Set<TEntity>().Add(item);
            SaveChanges();
            Db.Entry(item).State = EntityState.Detached;
        }

        public void Delete(TEntity item)
        {
            Db.Set<TEntity>().Attach(item);
            Db.Set<TEntity>().Remove(item);
            this.SaveChanges();
        }

        public void Update(TEntity item)
        {
            Db.Set<TEntity>().Attach(item);
            Db.Entry(item).State = EntityState.Modified;
            this.SaveChanges();
            Db.Entry(item).State = EntityState.Detached;
        }
        #endregion

        #region IExtensionRepository扩展方法实现

        public void Insert(IEnumerable<TEntity> item)
        {
            item.ToList().ForEach(i =>
            {
                Db.Entry<TEntity>(i);
                Db.Set<TEntity>().Add(i);
            });
            this.SaveChanges();
        }

        public void Update(IEnumerable<TEntity> item)
        {
            item.ToList().ForEach(i =>
            {
                Db.Set<TEntity>().Attach(i);
                Db.Entry(i).State = EntityState.Modified;
            });
            this.SaveChanges();
        }

        public void Delete(IEnumerable<TEntity> item)
        {
            item.ToList().ForEach(i =>
            {
                Db.Set<TEntity>().Attach(i);
                Db.Set<TEntity>().Remove(i);
            });
            this.SaveChanges();
        }

        public void Update<T>(Expression<Action<T>> entity) where T : class
        {
            T newEntity = typeof(T).GetConstructor(Type.EmptyTypes).Invoke(null) as T;//建立指定类型的实例
            List<string> propertyNameList = new List<string>();
            MemberInitExpression param = entity.Body as MemberInitExpression;
            foreach (var item in param.Bindings)
            {
                string propertyName = item.Member.Name;
                object propertyValue;
                var memberAssignment = item as MemberAssignment;
                if (memberAssignment.Expression.NodeType == ExpressionType.Constant)
                {
                    propertyValue = (memberAssignment.Expression as ConstantExpression).Value;
                }
                else
                {
                    propertyValue = Expression.Lambda(memberAssignment.Expression, null).Compile().DynamicInvoke();
                }
                typeof(T).GetProperty(propertyName).SetValue(newEntity, propertyValue, null);
                propertyNameList.Add(propertyName);
            }
            Db.Set<T>().Attach(newEntity);
            Db.Configuration.ValidateOnSaveEnabled = false;
            var ObjectStateEntry = ((IObjectContextAdapter)Db).ObjectContext.ObjectStateManager.GetObjectStateEntry(newEntity);
            propertyNameList.ForEach(x => ObjectStateEntry.SetModifiedProperty(x.Trim()));
            this.SaveChanges();
        }

        public IQueryable<TEntity> GetModel(ISpecification<TEntity> specification)
        {
            return GetModel().Where(specification.SatisfiedBy());
        }

        public IQueryable<TEntity> GetModel(Expression<Func<TEntity, bool>> predicate)
        {
            return GetModel().Where(predicate);
        }

        public IQueryable<TEntity> GetEntities(ISpecification<TEntity> specification)
        {
            return GetEntities().Where(specification.SatisfiedBy());
        }

        public IQueryable<TEntity> GetEntities(Expression<Func<TEntity, bool>> predicate)
        {
            return GetEntities().Where(predicate);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return GetModel(predicate).FirstOrDefault();
        }

        public TEntity Find(ISpecification<TEntity> specification)
        {
            return GetModel(specification).FirstOrDefault();
        }
        #endregion
    }
}
