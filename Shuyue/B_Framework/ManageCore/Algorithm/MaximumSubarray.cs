using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Algorithm
{
    /// <summary>
    /// 最大连续子序列问题
    /// </summary>
    public class MaximumSubarray
    {
        public static int Sum(int[] arr)
        {
            int max = 0, startIndex = 0, endIndex = 0, smax = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (smax <= 0)
                {
                    smax = 0;
                    startIndex = i;
                }
                smax += arr[i];
                if (smax > max)
                {
                    max = smax;
                    endIndex = i;
                }
            }
            return max;
        }

        /// <summary>
        /// 最大子序列
        /// </summary>
        /// <param name="recordList"></param>
        /// <returns></returns>
        public static MaximumSubarrayRose GetMaximumSubarrayRose(List<T_TransactionRecord> recordList)
        {
            MaximumSubarrayRose msr = new MaximumSubarrayRose();
            decimal max = 0, smax = 0;
            DateTime begin = default(DateTime), end = default(DateTime);
            for (int i = 0; i < recordList.Count(); i++)
            {
                smax += recordList[i].Rose;
                if (smax <= 0)
                {
                    smax = 0;
                    begin = recordList[i].TradingDate;
                }
                if (smax > max)
                {
                    max = smax;
                    end = recordList[i].TradingDate;
                    msr.BeginData = begin;
                    msr.EndDate = end;
                }
            }
            msr.RecordList = recordList.Where(a => a.TradingDate >= msr.BeginData && a.TradingDate <= msr.EndDate).ToList();
            return msr;
        }

        /// <summary>
        /// 最小子序列
        /// </summary>
        /// <param name="recordList"></param>
        /// <returns></returns>
        public static MinimumSubarrayRose GetMinimumSubarrayRose(List<T_TransactionRecord> recordList)
        {
            MinimumSubarrayRose msr = new MinimumSubarrayRose();
            decimal min = 0, smin = 0;
            DateTime begin = default(DateTime), end = default(DateTime);
            for (int i = 0; i < recordList.Count(); i++)
            {
                smin += recordList[i].Rose;
                if (smin >= 0)
                {
                    smin = 0;
                    begin = recordList[i].TradingDate;
                }
                if (smin < min)
                {
                    min = smin;
                    end = recordList[i].TradingDate;
                    msr.BeginData = begin;
                    msr.EndDate = end;
                }
            }
            msr.RecordList = recordList.Where(a => a.TradingDate >= msr.BeginData && a.TradingDate <= msr.EndDate).ToList();
            return msr;
        }
    }
}
