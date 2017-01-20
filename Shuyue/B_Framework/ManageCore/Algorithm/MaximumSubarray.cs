using Model.Extend;
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
        /// 获取子序列时间段
        /// </summary>
        /// <param name="recordList"></param>
        /// <returns></returns>
        public static List<SubarrayRose> GetSubarray(List<T_TransactionRecord> recordList)
        {
            List<SubarrayRose> srList = new List<SubarrayRose>();
            int shockday = 60;//shock days
            decimal rose = 0.2m, drop = -0.2m;//rose or drop range
            decimal max = 0, smax = 0, min = 0, smin = 0, cval = 0;
            bool isRose = false, isDrop = false, isShock = false;
            DateTime beginDate, endDate, maxbegin, maxend, minbegin, minend;
            beginDate = endDate = maxbegin = maxend = minbegin = minend = default(DateTime);
            SubarrayRoseEnum lastType = SubarrayRoseEnum.Default;
            //handle
            Action<SubarrayRoseEnum, DateTime, DateTime> run = (a, b, c) =>
            {
                if (lastType == a)
                {
                    srList.LastOrDefault().EndDate = c;
                }
                else
                {
                    srList.Add(new SubarrayRose { Type = a, BeginData = b, EndDate = c, });
                }
                lastType = a;
                switch (a)
                {
                    case SubarrayRoseEnum.Rose: isRose = false; break;
                    case SubarrayRoseEnum.Drop: isDrop = false; break;
                    case SubarrayRoseEnum.Shock: isShock = false; break;
                }
            };
            for (int i = 0; i < recordList.Count(); i++)
            {
                cval += recordList[i].Rose;
                smax += recordList[i].Rose;
                smin += recordList[i].Rose;
                //first
                if (i == 0) beginDate = endDate = maxbegin = minbegin = recordList[i].TradingDate;

                //is rose
                if (smax > (rose > max ? rose : max))
                {
                    if (isDrop) run(SubarrayRoseEnum.Drop, minbegin, minend);
                    if (isShock) run(SubarrayRoseEnum.Shock, beginDate, endDate);
                    isRose = true; max = smax; min = smin = cval = 0;
                    minbegin = beginDate = maxend = recordList[i].TradingDate;
                }

                //is drop
                else if (smin < (drop < min ? drop : min))
                {
                    if (isRose) run(SubarrayRoseEnum.Rose, maxbegin, maxend);
                    if (isShock) run(SubarrayRoseEnum.Shock, beginDate, endDate);
                    isDrop = true; min = smin; max = smax = cval = 0;
                    maxbegin = beginDate = minend = recordList[i].TradingDate;
                }

                //is shock
                else if ((recordList[i].TradingDate - beginDate).Days > shockday && drop <= cval && cval <= rose)
                {
                    if (isRose) run(SubarrayRoseEnum.Rose, maxbegin, maxend);
                    if (isDrop) run(SubarrayRoseEnum.Drop, minbegin, minend);
                    isShock = true; max = smax = min = smin = 0;
                    maxbegin = minbegin = endDate = recordList[i].TradingDate;
                    run(SubarrayRoseEnum.Shock, beginDate, endDate);
                }

                //last
                if (i == recordList.Count() - 1)
                {
                    if (isRose) run(SubarrayRoseEnum.Rose, maxbegin, maxend);
                    if (isDrop) run(SubarrayRoseEnum.Drop, minbegin, minend);
                    if (isShock) run(SubarrayRoseEnum.Shock, beginDate, endDate);
                }
            }
            return srList;
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
