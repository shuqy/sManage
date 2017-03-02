using Core;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YaoService.LeShare;

namespace WechatQyApp.Controllers {
    public class AutoSignController : Controller {
        // GET: AutoSign
        public ActionResult Index() {
            ViewBag.Txt = "妖！";
            return View();
        }

        public ActionResult AutoSignList() {
            return View(AutoSign.SignList);
        }

        public ActionResult AutoSignDetials(string username) {
            var res = AutoSign.GetAutoSignUser(username);
            if (res == null) return Redirect("/autosign/checklogin");
            ViewBag.IsCurrentUser = AppContext.Current.CurrentUser.Customer.Mobile.Equals(username);
            return View(res);
        }
        public ActionResult CheckLogin() {
            ViewBag.Mobile = AppContext.Current.CurrentUser.Customer.Mobile;
            return View();
        }

        public ActionResult AutoSignHistory() {
            return View(AutoSign.GetSignHistory());
        }

        public JsonResult ChangeNextSignDateTime(int userId, DateTime signDateTime) {
            return Json(new ResultData {
                code = AutoSign.ChangeNextSignDateTime(userId, signDateTime) ? ResultCode.ok : ResultCode.faile,
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CloseAutoSign(int userId) {
            return Json(new ResultData {
                code = AutoSign.CloseAutoSign(userId) ? ResultCode.ok : ResultCode.faile,
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddAutoLogin(string loginUserName, string password) {
            if (!AppContext.Current.CurrentUser.Customer.Mobile.Equals(loginUserName))
                return Json(new ResultData {
                    code = ResultCode.faile,
                    msg = "当前手机号与绑定手机号不一致"
                }, JsonRequestBehavior.AllowGet);

            AutoSignDbService autoSignDbService = new AutoSignDbService();
            bool isCanAutoSign = AutoSign.CheckIsAutoSign(loginUserName);
            if (isCanAutoSign)
                return Json(new ResultData {
                    code = ResultCode.faile,
                    msg = "已加入自动打卡队列，请勿重复操作"
                }, JsonRequestBehavior.AllowGet);


            SignModel sm = AutoSign.GetCurrentSignModel(loginUserName, password);

            if (sm == null)
                return Json(new ResultData {
                    code = ResultCode.faile,
                    msg = "用户名或密码不正确，加入自动打卡失败"
                }, JsonRequestBehavior.AllowGet);
            AutoSign.SignList.Add(sm);

            return Json(new ResultData {
                code = ResultCode.ok,
                msg = "加入自动打卡队列成功"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}