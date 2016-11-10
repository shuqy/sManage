using Core.Entities;
using Core.Util;
using ManageEF;
using ManageService.Entities;
using ManageService.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockApp.Controllers
{
    public class HomeController : ControllerBase
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Test()
        {
            var table = NPOIHelper.ImportExceltoDt("D:\\jgd.xlsx");
            var list = ModelHelper.ExcelFillModelList<delivery_order>(table);
            list = list.Where(l => l.OccurrenceAmount != 0).ToList();
            foreach (var item in list)
            {
                item.SecurityCode = item.SecurityCode.PadLeft(6, '0');
                item.OperationType = item.Operation == "买入" ? 0 : 1;
            }
            var db = Core.AppContext.Current.DbContext;
            db.delivery_order.AddRange(list);
            db.SaveChanges();
            return View();
        }

        [Description("登录")]
        public ActionResult Login()
        {
            LoginModel model = new LoginModel();
            string returnUrl = ParamHelper.GetString("redirect");
            model.Redirect = returnUrl ?? "/";
            model.UserCode = Encryptor.DecryptDES(CookieHelper.GetCookie("manageuser", "username"));
            model.PassCode = Encryptor.DecryptDES(CookieHelper.GetCookie("manageuser", "password"));
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            UserBLL userBll = new UserBLL();
            if (string.IsNullOrEmpty(model.UserCode) || string.IsNullOrEmpty(model.PassCode)) return View(model);
            var user = userBll.CheckUser(model.UserCode, Encryptor.EncryptDES(model.PassCode));
            if (user != null)
            {
                if (user.State == 0)
                {
                    ModelState.AddModelError("", "账号被禁用！");
                    return View(model);
                }
                BaseUser baseUser = new BaseUser
                {
                    Id = user.Id,
                    UserCode = user.UserCode,
                    UserName = user.UserName,
                    State = (int)user.State,
                };
                Session[SessionKey.CurrentUser] = baseUser;
                int isSavePwd = RequestHelper.GetFormInt("rePassword", 0);
                if (isSavePwd > 0)
                {
                    CookieHelper.SetCookie("manageuser", "username", Encryptor.EncryptDES(user.UserCode), isSavePwd);
                    CookieHelper.SetCookie("manageuser", "password", user.PassCode.Trim(), isSavePwd);
                }
                else { CookieHelper.DelCookie("manageuser"); }
                if (model.Redirect != null && model.Redirect.Length > 0)
                    return Redirect(model.Redirect);
                else
                    return Redirect("/home/index");
            }
            else
            {
                ModelState.AddModelError("", "登录帐号不存在或密码错误！");
            }
            return View(model);
        }
    }
}