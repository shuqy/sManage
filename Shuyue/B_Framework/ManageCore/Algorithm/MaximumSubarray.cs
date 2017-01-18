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

        public static List<SubarrayRose> GetSubarrayRose(List<T_TransactionRecord> recordList)
        {
            List<SubarrayRose> srList = new List<SubarrayRose>();
            int shockday = 20;
            decimal shockrose = 0.1m, shockdrop = -0.1m;
            decimal rose = 0.3m, drop = -0.3m;
            decimal max = 0, smax = 0, min = 0, smin = 0, cval = 0;
            bool isRose = false, isDrop = false, isShock = false;
            DateTime beginDate = default(DateTime), endDate = default(DateTime), maxbegin = default(DateTime), maxend = default(DateTime), minbegin = default(DateTime), minend = default(DateTime);
            int go = 0;
            for (int i = 0; i < recordList.Count(); i++)
            {
                cval += recordList[i].Rose;
                smax += recordList[i].Rose;
                smin += recordList[i].Rose;
                if (i == 0)
                {
                    beginDate = endDate = maxbegin = minbegin = recordList[i].TradingDate;
                }

                //is rose
                if (smax > (rose > max ? rose : max))
                {
                    if (isDrop)
                    {
                        srList.Add(new SubarrayRose
                        {
                            Type = SubarrayRoseEnum.Drop,
                            BeginData = minbegin,
                            EndDate = minend,
                        });
                        isDrop = false;
                        //minbegin = beginDate = recordList[i].TradingDate;
                        min = smin = cval = 0;
                    }
                    if (isShock)
                    {
                        srList.Add(new SubarrayRose
                        {
                            Type = SubarrayRoseEnum.Shock,
                            BeginData = beginDate,
                            EndDate = endDate,
                        });
                        isShock = false;
                        //beginDate = minbegin = recordList[i].TradingDate;
                        cval = min = smin = 0;
                    }
                    isRose = true;
                    max = smax;
                    minbegin = beginDate = maxend = recordList[i].TradingDate;
                }
                //is drop
                if (smin < (drop < min ? drop : min))
                {
                    if (isRose)
                    {
                        srList.Add(new SubarrayRose
                        {
                            Type = SubarrayRoseEnum.Rose,
                            BeginData = maxbegin,
                            EndDate = maxend,
                        });
                        isRose = false;
                        //maxbegin = beginDate = recordList[i].TradingDate;
                        max = smax = cval = 0;
                    }
                    if (isShock)
                    {
                        srList.Add(new SubarrayRose
                        {
                            Type = SubarrayRoseEnum.Shock,
                            BeginData = beginDate,
                            EndDate = endDate,
                        });
                        isShock = false;
                        //beginDate = maxbegin = recordList[i].TradingDate;
                        cval = max = smax = 0;
                    }
                    isDrop = true;
                    min = smin;
                    maxbegin = beginDate = minend = recordList[i].TradingDate;
                }

                //is shock
                if ((recordList[i].TradingDate - beginDate).Days > shockday && shockdrop <= cval && cval <= shockrose)
                {
                    if (isRose)
                    {
                        srList.Add(new SubarrayRose
                        {
                            Type = SubarrayRoseEnum.Rose,
                            BeginData = maxbegin,
                            EndDate = maxend,
                        });
                        isRose = false;
                        //maxbegin = minbegin = recordList[i].TradingDate;
                        max = smax = min = smin = 0;
                    }
                    if (isDrop)
                    {
                        srList.Add(new SubarrayRose
                        {
                            Type = SubarrayRoseEnum.Drop,
                            BeginData = minbegin,
                            EndDate = minend,
                        });
                        isDrop = false;
                        //minbegin = maxbegin = recordList[i].TradingDate;
                        max = smax = min = smin = 0;
                    }
                    isShock = true;
                    maxbegin = minbegin = endDate = recordList[i].TradingDate;
                }
                //最后一天
                if (i == recordList.Count() - 1)
                {
                    if (isRose)
                    {
                        srList.Add(new SubarrayRose
                        {
                            Type = SubarrayRoseEnum.Rose,
                            BeginData = maxbegin,
                            EndDate = maxend,
                        });
                    }
                    if (isDrop)
                    {
                        srList.Add(new SubarrayRose
                        {
                            Type = SubarrayRoseEnum.Drop,
                            BeginData = minbegin,
                            EndDate = minend,
                        });
                    }
                    if (isShock)
                    {
                        srList.Add(new SubarrayRose
                        {
                            Type = SubarrayRoseEnum.Shock,
                            BeginData = beginDate,
                            EndDate = endDate,
                        });
                    }
                }
            }
            return srList;
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
