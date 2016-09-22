using Core.Entities;
using Core.Enum;
using Core.Util;
using ManageEF;
using ManageEF.ViewModel;
using ManageService.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manage.Controllers.Menu
{
    public class MenuController : ControllerBase
    {
        MenuBLL menuBLL = new MenuBLL();
        /// <summary>
        /// 菜单首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.parentId = Convert.ToInt32(Request.QueryString["parentId"] ?? "0");
            return View();
        }
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult Search(DataTableRequest param)
        {
            int parentId = Request["parentId"].GetValueOrNull<int>() ?? 0;
            var list = menuBLL.GetEntities(m => m.ParentId == parentId && m.Deleted == false);
            var res = list.OrderByDescending(l => l.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var query = res.Select(r => new
            {
                r.Title,
                r.Code,
                r.ParentId,
                r.Description,
                r.State,
                r.SortValue,
                r.Id,
                r.IsPath,
                r.ShowInMenu,
                r.ControllName,
                r.ActionName
            });
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = list.Count(),
                iTotalDisplayRecords = list.Count(),
                aaData = query.ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加或修改菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            sys_user_menu menu = null;
            int parentId = Request.QueryString["parentId"].GetValueOrNull<int>() ?? 0;
            if (id > 0) menu = menuBLL.GetEntities(m => m.Id == id).FirstOrDefault();
            if (menu == null)
            {
                menu = new sys_user_menu();
                menu.ParentId = parentId;
            }
            return View(menu);
        }
        /// <summary>
        /// 编辑菜单，添加或修改
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Edit(sys_user_menu menu)
        {
            menu.ControllName = menu.ControllName == null ? "" : menu.ControllName.ToLower();
            menu.ActionName = menu.ActionName == null ? "" : menu.ActionName.ToLower();
            if (menu.Id <= 0)
            {
                menu.Deleted = false;
                menu.State = true;
                menuBLL.Insert(menu);
            }
            else
            {
                sys_user_menu nmenu = menuBLL.GetEntities(m => m.Id == menu.Id).FirstOrDefault();
                if (nmenu == null)
                    return Json(new JsonData { Code = ResultCode.Fail, }, JsonRequestBehavior.DenyGet);
                nmenu.ActionName = menu.ActionName;
                nmenu.Code = menu.Code;
                nmenu.ControllName = menu.ControllName;
                nmenu.Description = menu.Description;
                nmenu.IsPath = menu.IsPath;
                nmenu.ShowInMenu = menu.ShowInMenu;
                nmenu.SortValue = menu.SortValue;
                nmenu.State = menu.State;
                nmenu.STitle = menu.STitle;
                nmenu.Title = menu.Title;
                menuBLL.Update(nmenu);
            }
            Core.AppContext.Current.UserMenu = null;
            return Json(new JsonData { Code = ResultCode.OK, }, JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 更改菜单状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditState(int id)
        {
            if (id <= 0) return null;
            sys_user_menu menu = menuBLL.GetEntities().FirstOrDefault(g => g.Id == id);
            menu.State = !menu.State;
            menuBLL.Update(menu);
            Core.AppContext.Current.UserMenu = null;
            return Json(new JsonData { Code = ResultCode.OK, }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (id <= 0) return null;
            sys_user_menu menu = menuBLL.GetEntities().FirstOrDefault(g => g.Id == id);
            menu.Deleted = true;
            menuBLL.Update(menu);
            Core.AppContext.Current.UserMenu = null;
            return Json(new JsonData { Code = ResultCode.OK, }, JsonRequestBehavior.DenyGet);
        }
    }
}