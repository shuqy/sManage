using Manage.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manage.Controllers
{
    [SmAuthorize]
    [UserGroup]
    public class ControllerBase : Controller
    {
    }
}