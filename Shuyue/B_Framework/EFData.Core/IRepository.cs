using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Entity.Core
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 添加实体并提交到数据服务器
        /// </summary>
        /// <param name="item">Item to add to repository</param>
        void Insert(TEntity item);

        /// <summary>
        /// 移除实体并提交到数据服务器
        /// 如果表存在约束，需要先删除子表信息
        /// </summary>
        /// <param name="item">Item to delete</param>
        void Delete(TEntity item);

        /// <summary>
        /// 修改实体并提交到数据服务器
        /// </summary>
        /// <param name="item"></param>
        void Update(TEntity item);

        /// <summary>
        /// 子类在实现时，可以重写，加一些状态过滤
        /// Get all elements of type {T} in repository
        /// </summary>
        /// <returns>List of selected elements</returns>
        IQueryable<TEntity> GetModel();

        /// <summary>
        /// 得到所有实体
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetEntities();
        /// <summary>
        /// 根据主键得到实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Find(params object[] id);
    }
}
