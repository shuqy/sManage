using StockApp.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockApp.Controllers
{
    [SmAuthorize]
    public class ControllerBase : Controller
    {
    }
}