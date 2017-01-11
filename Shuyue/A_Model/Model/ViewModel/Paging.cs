using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class Paging
    {
        private int _pageSize;
        /// <summary>
        /// 每页数据量
        /// </summary>
        public int PageSize
        {
            get { return _pageSize == 0 ? 10 : _pageSize; }
            set { _pageSize = value; }
        }
        private int _pageCount;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get { return CounterPageCount(); }
            set { _pageCount = value; }
        }
        private int _skipNumber;
        /// <summary>
        /// 跳过条数
        /// </summary>
        public int SkipNumber
        {
            get { return _skipNumber; }
            set { _skipNumber = value; }
        }
        /// <summary>
        /// 总数据量
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 实体
        /// </summary>
        public IQueryable Entity { get; set; }

        /// <summary>
        /// 计算总页数
        /// </summary>
        /// <returns></returns>
        private int CounterPageCount()
        {
            if (Amount % PageSize == 0)
            {
                return Amount / PageSize > 0 ? Amount / PageSize : 0;
            }
            else
            {
                return Amount / PageSize > 0 ? Amount / PageSize + 1 : 0;
            }
        }
    }
}
