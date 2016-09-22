using Core.Entities;
using Core.Util;
using Manage.Attributes;
using ManageEF;
using ManageEF.ViewModel;
using ManageService.Entities;
using ManageService.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manage.Controllers
{
    public class HomeController : Controller
    {
        [SmAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            LoginModel model = new LoginModel();
            string returnUrl = ParamHelper.GetString("ReturnUrl");
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
                    CookieHelper.SetCookie("manageuser", "password", Encryptor.EncryptDES(user.PassCode.Trim()), isSavePwd);
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

        public ActionResult Logout()
        {
            Session.Abandon();
            CookieHelper.DelCookie("manageuser");
            return RedirectToAction("Login");
        }
    }
}