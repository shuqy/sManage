using Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Manage.Controllers.Algorithm
{
    public class SortController : ControllerBase
    {
        public ActionResult Index()
        {
            tempHtml();
            return View();
        }

        //选择排序
        public ActionResult Selection()
        {
            StringBuilder sortHtml = new StringBuilder();
            sortHtml.AppendLine("<p>    public class Selection<T> : SortTemp<T> where T :IComparable<T></p>");
            sortHtml.AppendLine("<p>    {</p>");
            sortHtml.AppendLine("<p>        // 选择排序，时间复杂度O(N*N)</p>");
            sortHtml.AppendLine("<p>        public static void Sort(T[] arr)</p>");
            sortHtml.AppendLine("<p>        {</p>");
            sortHtml.AppendLine("<p>            int n = arr.Length;</p>");
            sortHtml.AppendLine("<p>            for (var i = 0; i < n; i++)</p>");
            sortHtml.AppendLine("<p>            {</p>");
            sortHtml.AppendLine("<p>                int min = i;</p>");
            sortHtml.AppendLine("<p>                for (var j = i; j < n; j++)</p>");
            sortHtml.AppendLine("<p>                    if (Less(arr[j], arr[min]))</p>");
            sortHtml.AppendLine("<p>                        min = j;</p>");
            sortHtml.AppendLine("<p>                Exch(arr, i, min);</p>");
            sortHtml.AppendLine("<p>            }</p>");
            sortHtml.AppendLine("<p>        }</p>");
            sortHtml.AppendLine("<p>    }</p>");
            sortHtml.AppendLine("<p></p>");
            sortHtml.AppendLine("<p></p>");
            ViewBag.SortHtml = CSharpHelper.ChangeColor(sortHtml.ToString().Replace(" ", "&nbsp;"));
            tempHtml();
            return View();
        }

        /// <summary>
        /// 希尔排序
        /// </summary>
        /// <returns></returns>
        public ActionResult Shell()
        {
            StringBuilder sortHtml = new StringBuilder();
            sortHtml.AppendLine("<p>    public class ShellSort<T> : SortTemp<T> where T : IComparable<T> {</p>");
            sortHtml.AppendLine("<p>        // 希尔排序，基于插入排序的排序算法</p>");
            sortHtml.AppendLine("<p>        public static void Sort(T[] arr) {</p>");
            sortHtml.AppendLine("<p>            int n = arr.Length;</p>");
            sortHtml.AppendLine("<p>            var h = 1;</p>");
            sortHtml.AppendLine("<p>            while (h < n / 3) h = 3 * h + 1;</p>");
            sortHtml.AppendLine("<p>            while (h >= 1) {</p>");
            sortHtml.AppendLine("<p>                for (var i = h; i < n; i++) {</p>");
            sortHtml.AppendLine("<p>                    for (var j = i; j >= h && Less(arr[j], arr[j - h]); j -= h)</p>");
            sortHtml.AppendLine("<p>                        Exch(arr, j, j - h);</p>");
            sortHtml.AppendLine("<p>                }</p>");
            sortHtml.AppendLine("<p>                h = h / 3;</p>");
            sortHtml.AppendLine("<p>            }</p>");
            sortHtml.AppendLine("<p>        }</p>");
            ViewBag.SortHtml = CSharpHelper.ChangeColor(sortHtml.ToString().Replace(" ", "&nbsp;"));
            tempHtml();
            return View();
        }

        /// <summary>
        /// 插入排序
        /// </summary>
        /// <returns></returns>
        public ActionResult Insertion()
        {
            StringBuilder sortHtml = new StringBuilder();
            sortHtml.AppendLine("<p>    public class Insertion<T> : SortTemp<T> where T : IComparable<T> {</p>");
            sortHtml.AppendLine("<p></p>");
            sortHtml.AppendLine("<p>        // 插入排序，在大部分数字的顺序都是正确的时候，可能是所有排序方法中效率最高的</p>");
            sortHtml.AppendLine("<p>        public static void Sort(T[] arr) {</p>");
            sortHtml.AppendLine("<p>            int n = arr.Length;</p>");
            sortHtml.AppendLine("<p>            for (var i = 0; i < n; i++) {</p>");
            sortHtml.AppendLine("<p>                for (var j = i; j > 0 && Less(arr[j], arr[j - 1]); j--)</p>");
            sortHtml.AppendLine("<p>                    Exch(arr, j, j - 1);</p>");
            sortHtml.AppendLine("<p>            }</p>");
            sortHtml.AppendLine("<p>        }</p>");
            sortHtml.AppendLine("<p>    }</p>");
            sortHtml.AppendLine("<p></p>");
            ViewBag.SortHtml = CSharpHelper.ChangeColor(sortHtml.ToString().Replace(" ", "&nbsp;"));
            tempHtml();
            return View();
        }

        /// <summary>
        /// 快速排序
        /// </summary>
        /// <returns></returns>
        public ActionResult Quick()
        {
            StringBuilder sortHtml = new StringBuilder();
            sortHtml.AppendLine("<p>    public class Quick<T> : SortTemp<T> where T : IComparable<T> {</p>");
            sortHtml.AppendLine("<p>        // 快速排序，时间复杂度O(NlogN)，使用最广泛的排序算法</p>");
            sortHtml.AppendLine("<p>        public static void Sort(T[] arr) {</p>");
            sortHtml.AppendLine("<p>            Sort(arr, 0, arr.Length - 1);</p>");
            sortHtml.AppendLine("<p>        }</p>");
            sortHtml.AppendLine("<p>        private static void Sort(T[] arr, int lo, int hi) {</p>");
            //sortHtml.AppendLine("<p>            //if (lo >= hi) return;</p>");
            sortHtml.AppendLine("<p>            if (hi <= lo + 10) { Insertion<T>.Sort(arr, lo, hi); return; }</p>");
            sortHtml.AppendLine("<p>            int c = Partition(arr, lo, hi);</p>");
            sortHtml.AppendLine("<p>            Sort(arr, lo, c - 1);</p>");
            sortHtml.AppendLine("<p>            Sort(arr, c + 1, hi);</p>");
            sortHtml.AppendLine("<p>        }</p>");
            sortHtml.AppendLine("<p>        private static int Partition(T[] arr, int lo, int hi) {</p>");
            sortHtml.AppendLine("<p>            int i = lo, j = hi + 1;</p>");
            sortHtml.AppendLine("<p>            var v = arr[lo];</p>");
            sortHtml.AppendLine("<p>            while (true) {</p>");
            sortHtml.AppendLine("<p>                while (Less(arr[++i], v)) if (i == hi) break;</p>");
            sortHtml.AppendLine("<p>                while (Less(v, arr[--j])) if (j == lo) break;</p>");
            sortHtml.AppendLine("<p>                if (i >= j) break;</p>");
            sortHtml.AppendLine("<p>                Exch(arr, i, j);</p>");
            sortHtml.AppendLine("<p>            }</p>");
            sortHtml.AppendLine("<p>            Exch(arr, lo, j);</p>");
            sortHtml.AppendLine("<p>            return j;</p>");
            sortHtml.AppendLine("<p>        }</p>");
            sortHtml.AppendLine("<p>    }</p>");
            ViewBag.SortHtml = CSharpHelper.ChangeColor(sortHtml.ToString().Replace(" ", "&nbsp;"));
            tempHtml();
            return View();
        }

        //公用排序模块
        private void tempHtml()
        {
            StringBuilder selectionHtml = new StringBuilder();
            selectionHtml.AppendLine("<p>    public class SortTemp<T> where T : IComparable<T> {</p>");
            selectionHtml.AppendLine("<p>        public static bool Less(T a, T b) {</p>");
            selectionHtml.AppendLine("<p>            return a.CompareTo(b) < 0;</p>");
            selectionHtml.AppendLine("<p>        }</p>");
            selectionHtml.AppendLine("<p>        public static void Exch(T[] arr, int i, int j) {</p>");
            selectionHtml.AppendLine("<p>            var temp = arr[i];</p>");
            selectionHtml.AppendLine("<p>            arr[i] = arr[j];</p>");
            selectionHtml.AppendLine("<p>            arr[j] = temp;</p>");
            selectionHtml.AppendLine("<p>        }</p>");
            selectionHtml.AppendLine("<p>    }</p>");
            ViewBag.TempStr = CSharpHelper.ChangeColor(selectionHtml.ToString().Replace(" ", "&nbsp;"));
        }
    }
}