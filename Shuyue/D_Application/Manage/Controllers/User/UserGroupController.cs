using Core.Entities;
using Core.Enum;
using Core.Util;
using ManageEF;
using ManageEF.ViewModel;
using ManageService.Menu;
using ManageService.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Manage.Controllers
{
    public class UserGroupController : ControllerBase
    {
        UserGroupBLL userGroupBLL = new UserGroupBLL();
        /// <summary>
        /// 角色管理首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Search(DataTableRequest param)
        {
            var list = userGroupBLL.GetEntities();
            var res = list.OrderBy(l => l.SortValue).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var query = res.Select(r => new { r.Deleted, r.Title, r.Code, r.Description, r.State, r.SortValue, r.Id });
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = list.Count(),
                iTotalDisplayRecords = list.Count(),
                aaData = query.ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
        //编辑
        public ActionResult Edit(int id = 0)
        {
            sys_user_group group = new sys_user_group();
            if (id != 0) group = userGroupBLL.GetEntities().FirstOrDefault(g => g.Id == id);
            return View(group);
        }
        [HttpPost]
        public ActionResult Edit(sys_user_group group)
        {
            if (group.Id == 0)
            {
                if (userGroupBLL.GetEntities().Any(g => g.Code == group.Code))
                    return Json(new JsonData { Code = ResultCode.Fail, }, JsonRequestBehavior.DenyGet);
                group.Deleted = false;
                group.IsFree = 0;
                userGroupBLL.Insert(group);
            }
            else
            {
                sys_user_group userGroup = userGroupBLL.GetEntities().FirstOrDefault(g => g.Id == group.Id);
                userGroup.Title = group.Title;
                userGroup.Description = group.Description;
                userGroup.Code = group.Code;
                userGroup.SortValue = group.SortValue;
                userGroup.State = group.State;
                userGroupBLL.Update(userGroup);
            }
            return Json(new JsonData { Code = ResultCode.OK, }, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public JsonResult EditState(int id)
        {
            if (id <= 0) return null;
            sys_user_group userGroup = userGroupBLL.GetEntities().FirstOrDefault(g => g.Id == id);
            userGroup.State = !userGroup.State;
            userGroupBLL.Update(userGroup);
            return Json(new JsonData { Code = ResultCode.OK, }, JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 设置菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SetMenu(int id)
        {
            ViewBag.GroupId = id;
            MenuBLL menuBLL = new MenuBLL();
            MenuTree menuTree = menuBLL.GetMenuTree();
            GroupMenuMappingBLL groupMenuBLL = new GroupMenuMappingBLL();
            var menuList = groupMenuBLL.GetEntities(r => r.GroupId == id);
            ViewBag.html = GetMenuHtml(menuTree, menuList.ToList());
            return View(menuTree);
        }
        private string GetMenuHtml(MenuTree menuTree, List<group_menu_mapping> menuList)
        {
            StringBuilder sb = new StringBuilder();
            if (menuTree.ChildrenMenu != null && menuTree.ChildrenMenu.Any())
            {
                if (menuTree.Id != 0)
                {
                    sb.AppendLine("<ul style=\"margin-bottom: 10px;padding-left: 30px;clear:both;\">");
                    sb.AppendLine("<li>");
                    sb.AppendLine("<label class=\"checkbox parentCheck\"><input name=\"menuId\" type=\"checkbox\" value=\""
                        + menuTree.Id + "\" " + (menuList.Any(m => m.MenuID == menuTree.Id) ? "checked" : "") + " />"
                        + menuTree.Title + "</label>");
                }
                foreach (MenuTree temp in menuTree.ChildrenMenu)
                {
                    sb.AppendLine(GetMenuHtml(temp, menuList));
                }
                if (menuTree.Id != 0)
                {
                    sb.AppendLine("</li>");
                    sb.AppendLine("</ul>");
                }
            }
            else
            {
                if (menuTree.Id != 0)
                {
                    sb.AppendLine("<li style=\"padding-left: 30px;float:left;\">");
                    sb.AppendLine("<label class=\"checkbox\"><input name=\"menuId\" type=\"checkbox\" value=\""
                        + menuTree.Id + "\" " + (menuList.Any(m => m.MenuID == menuTree.Id) ? "checked" : "") + " />"
                        + menuTree.Title + "</label>");
                    sb.AppendLine("</li>");
                }
            }
            return sb.ToString();
        }
        [HttpPost]
        public JsonResult SetMenu()
        {
            int groupId = Request["groupId"].GetValueOrNull<int>() ?? 0;
            GroupMenuMappingBLL groupMenuBLL = new GroupMenuMappingBLL();
            int[] items = Request["menuId"].GetArrayNoNull<int>();
            groupMenuBLL.Delete(groupMenuBLL.GetEntities(r => r.GroupId == groupId));
            List<group_menu_mapping> groupMenuList = new List<group_menu_mapping>();
            foreach (int menuId in items)
            {
                groupMenuList.Add(new group_menu_mapping() { GroupId = groupId, MenuID = menuId });
            }
            groupMenuBLL.Insert(groupMenuList);
            Core.AppContext.Current.UserMenu = null;
            return Json(new JsonData { Code = ResultCode.OK, }, JsonRequestBehavior.DenyGet);
        }
    }
}