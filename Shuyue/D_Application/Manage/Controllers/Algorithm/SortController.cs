﻿using Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Manage.Controllers.Algorithm
{
    public class SortController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Selection()
        {
            StringBuilder selectionHtml = new StringBuilder();
            selectionHtml.AppendLine("<p style=\"text-indent:2em;\">public class SortTemp<T> where T : IComparable<T> {</p>");
            selectionHtml.AppendLine("<p style=\"text-indent:4em;\">public static bool Less(T a, T b) {</p>");
            selectionHtml.AppendLine("<p style=\"text-indent:6em;\">return a.CompareTo(b) < 0;</p>");
            selectionHtml.AppendLine("<p style=\"text-indent:4em;\">}</p>");
            selectionHtml.AppendLine("<p style=\"text-indent:4em;\">public static void Exch(T[] arr, int i, int j) {</p>");
            selectionHtml.AppendLine("<p style=\"text-indent:6em;\">var temp = arr[i];</p>");
            selectionHtml.AppendLine("<p style=\"text-indent:6em;\">arr[i] = arr[j];</p>");
            selectionHtml.AppendLine("<p style=\"text-indent:6em;\">arr[j] = temp;</p>");
            selectionHtml.AppendLine("<p style=\"text-indent:4em;\">}</p>");
            selectionHtml.AppendLine("<p style=\"text-indent:2em;\">}</p>");
            selectionHtml.AppendLine("<p style=\"text-indent:2em;\">public class SortController : Controller </p>");
            selectionHtml.AppendLine("<p style=\"text-indent:2em;\">public ActionResult Index()</p>");
            selectionHtml.AppendLine("<p style=\"text-indent:2em;\">StringBuilder selectionHtml = new StringBuilder();</p>");
            selectionHtml.AppendLine("<p style=\"text-indent:2em;\">return View();</p>");
            ViewBag.SelectionSortStr = CSharpHelper.ChangeColor(selectionHtml.ToString());
            return View();
        }
    }
}