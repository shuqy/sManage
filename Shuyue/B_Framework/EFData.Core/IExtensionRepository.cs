using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EF.Entity.Core
{
    public interface IExtensionRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 添加集合[集合数目不大时用此方法，超大集合使用BulkInsert]
        /// </summary>
        /// <param name="item"></param>
        void Insert(IEnumerable<TEntity> item);

        /// <summary>
        /// 修改集合[集合数目不大时用此方法，超大集合使用BulkUpdate]
        /// </summary>
        /// <param name="item"></param>
        void Update(IEnumerable<TEntity> item);

        /// <summary>
        /// 删除集合[集合数目不大时用此方法，超大集合使用批量删除]
        /// </summary>
        /// <param name="item"></param>
        void Delete(IEnumerable<TEntity> item);

        /// <summary>
        /// 扩展更新方法，只对EF支持
        /// </summary>
        /// <param name="entity"></param>
        void Update<T>(Expression<Action<T>> entity) where T : class;

        /// <summary>
        /// 根据指定规约,得到延时结果集
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetModel(ISpecification<TEntity> specification);

        /// <summary>
        /// 根据指定lambda表达式,得到延时结果集
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetModel(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据指定规约,得到延时结果集
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetEntities(ISpecification<TEntity> specification);

        /// <summary>
        /// 根据指定lambda表达式,得到延时结果集
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetEntities(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据指定lambda表达式,得到第一个实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据指定规约,得到第一个实体
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        TEntity Find(ISpecification<TEntity> specification);

    }
}
