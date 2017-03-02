using Core;
using Core.Authorization;
using Core.Entities;
using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeixinService.Common;
using YaoService.Service;
using YaoService.Zhihu;

namespace WechatQyApp.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            ViewBag.ShowTxt = "妖！";
            return View();
        }

        public ActionResult GetIp() {
            ViewBag.Ip = IpHelper.GetIP();
            return View();
        }

        public ActionResult BindMobile() {
            return View();
        }

        public JsonResult SendMsgTest() {
            WeixinQyMsgHelper.SendMsg("624842", "6", "测试消息");
            Senparc.Weixin.QY.Entities.Article article = new Senparc.Weixin.QY.Entities.Article {
                Title = "测试下哈哈哈哈",
                Description = "第一次体(zhuang)验(bi)失败。去超市发现有银联闪付标志的POS机器，于是去挑了一瓶水准备开始装逼了(▭-▭)✧收银员：“6元。”我：“刷卡。”收银员一脸吃惊状，“6元还刷卡 ?”我点了点头，然后指了下POS机。收银员不耐烦地伸出手来，“卡拿来。”我…",
                Url = "http://www.wmylife.com/Zhihu/QADetials?zid=1"
            };
            WeixinQyMsgHelper.SendNews("624842", "6", new List<Senparc.Weixin.QY.Entities.Article>() { article });
            return null;
        }

        public JsonResult BindMobileService(string mobile) {
            if (string.IsNullOrEmpty(mobile))
                return Json(new ResultData { code = ResultCode.faile }, JsonRequestBehavior.AllowGet);
            if (AppContext.Current.CurrentUser.Customer == null)
                return Json(new ResultData { code = ResultCode.error, msg = "未登录授权" }, JsonRequestBehavior.AllowGet);
            CustomerService cs = new CustomerService();
            bool res = cs.BindMobile(AppContext.Current.CurrentUser.Customer.Id, mobile);
            if (res) {
                Session[SessionKey.CurrentUser] = null;
            } else {
                return Json(new ResultData { code = ResultCode.error, msg = "请检查输入手机号是否正确" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultData { code = ResultCode.ok }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GrawlZhihu() {
            AutoGrawl.GetAnswer();
            return Json(new ResultData { code = ResultCode.ok }, JsonRequestBehavior.AllowGet);
        }
    }
}