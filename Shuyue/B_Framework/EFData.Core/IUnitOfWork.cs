using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Entity.Core
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 将操作提交到数据库，
        /// </summary>
        void Save();

        /// <summary>
        /// 是否显式进行提交(sql2008完善了对事务的支持，这个属性将不需要)
        /// 默认为false，即默认直接提交到数据库，值为true表示需要手动调用Save方法
        /// </summary>
        /// <returns></returns>
        bool IsExplicitSubmit { get; set; }
    }
}
