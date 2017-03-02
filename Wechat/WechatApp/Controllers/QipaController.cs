using Core;
using Core.Utilities;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeixinService.Common;
using YaoService.SubwayStation;

namespace WechatApp.Controllers {
    public class QipaController : Controller {
        // GET: Qipa
        public ActionResult Index() {
            return View();
        }

        [HttpGet]
        public JsonResult SendMsgTest() {
            WeixinMsgHelper.SendMessage(AppContext.Current.CurrentUser.WeixinCustomer.OpenId, "这是一条测试消息");
            return null;
        }
        [HttpGet]
        public JsonResult SendMsgNews() {
            List<Article> articles = new List<Article>() {
                new Article {
                    Description="你那可悲的法术起不了什么作用",
                    Title="法师们，小心了",
                }
            };
            WeixinMsgHelper.SendNews(AppContext.Current.CurrentUser.WeixinCustomer.OpenId, articles);
            return null;
        }

        public ActionResult SubwayMap() {
            return View();
        }

        //地铁路线图
        public JsonResult GetSubwayPath(string from, string to) {
            var path = SubwayStationHandle.Path(from, to);
            return Json(path, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStations() {
            return Json(SubwayStationHandle.GetAllStations(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSitePath() {
            ViewBag.SitePath = HttpContext.Server.MapPath("/Log");
            return View();
        }
    }
}