using Core;
using Core.Entities;
using DotNet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YaoService.Zhihu;

namespace WechatQyApp.Controllers {
    public class ZhihuController : Controller {
        private ZhihuService _zhiService;
        public ZhihuController() {
            _zhiService = new ZhihuService();
        }
        // GET: ZhihuRecommend
        public ActionResult Index() {
            return View();
        }

        public ActionResult QADetials(int zid) {
            _zhiService.AddReadHistory(new AliYunModel.ZhihuReadHistory {
                CustomerId = AppContext.Current.CurrentUser.Customer.Id,
                ZhihuId = zid,
                CreatedOn = DateTime.Now,
                Deleted = false
            });
            return View();
        }

        public JsonResult GetQADetials(int zid) {
            var zhiqa = _zhiService.GetQAById(zid);
            if (zhiqa == null) return Json(new ResultData { code = ResultCode.faile }, JsonRequestBehavior.AllowGet);
            zhiqa.Content = FileOperate.ReadFile(zhiqa.Content);
            return Json(new ResultData { code = ResultCode.ok, data = zhiqa }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult QAList() {
            var res = _zhiService.GetList();
            return View(res);
        }
    }
}