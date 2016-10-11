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
            StringBuilder selectionHtml = new StringBuilder();
            selectionHtml.AppendLine("<p>    public class SortTemp<T> where T : IComparable<T> {</p>");
            selectionHtml.AppendLine("<p>        public static bool Less(T a, T b) {</p>");
            selectionHtml.AppendLine("<p>            console.log(\"abc\")</p>");
            selectionHtml.AppendLine("<p>            return a.CompareTo(b) < 0;</p>");
            selectionHtml.AppendLine("<p>        }</p>");
            selectionHtml.AppendLine("<p>        public static void Exch(T[] arr, int i, int j) {</p>");
            selectionHtml.AppendLine("<p>            var temp = arr[i];</p>");
            selectionHtml.AppendLine("<p>            arr[i] = arr[j];</p>");
            selectionHtml.AppendLine("<p>            arr[j] = temp;</p>");
            selectionHtml.AppendLine("<p>        }</p>");
            selectionHtml.AppendLine("<p>    }</p>");
            ViewBag.SelectionSortStr = CSharpHelper.ChangeColor(selectionHtml.ToString().Replace(" ", "&nbsp;"));
            return View();
        }
        public ActionResult Selection()
        {
            return View();
        }
    }
}