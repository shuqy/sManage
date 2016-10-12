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
            StringBuilder selectionHtml = new StringBuilder();
            selectionHtml.AppendLine("<p>    public class Selection<T> : SortTemp<T> where T :IComparable<T></p>");
            selectionHtml.AppendLine("<p>    {</p>");
            selectionHtml.AppendLine("<p>        // 选择排序，时间复杂度O(N*N)</p>");
            selectionHtml.AppendLine("<p>        public static void Sort(T[] arr)</p>");
            selectionHtml.AppendLine("<p>        {</p>");
            selectionHtml.AppendLine("<p>            int n = arr.Length;</p>");
            selectionHtml.AppendLine("<p>            for (var i = 0; i < n; i++)</p>");
            selectionHtml.AppendLine("<p>            {</p>");
            selectionHtml.AppendLine("<p>                int min = i;</p>");
            selectionHtml.AppendLine("<p>                for (var j = i; j < n; j++)</p>");
            selectionHtml.AppendLine("<p>                    if (Less(arr[j], arr[min]))</p>");
            selectionHtml.AppendLine("<p>                        min = j;</p>");
            selectionHtml.AppendLine("<p>                Exch(arr, i, min);</p>");
            selectionHtml.AppendLine("<p>            }</p>");
            selectionHtml.AppendLine("<p>        }</p>");
            selectionHtml.AppendLine("<p>    }</p>");
            selectionHtml.AppendLine("<p></p>");
            selectionHtml.AppendLine("<p></p>");
            ViewBag.SelectionSortStr = CSharpHelper.ChangeColor(selectionHtml.ToString().Replace(" ", "&nbsp;"));
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