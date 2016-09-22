using ManageEF;
using ManageService.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manage.Controllers.Common
{
    public class MenuCommon
    {
        /// <summary>
        /// 获取用户所有菜单
        /// </summary>
        /// <returns></returns>
        public List<sys_user_menu> GetUserMenu()
        {
            MenuBLL menuBLL = new MenuBLL();
            return menuBLL.GetUserMenu();
        }
        /// <summary>
        /// 获取菜单下第一个链接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetRootUrl(int id)
        {
            MenuBLL menuBLL = new MenuBLL();
            return menuBLL.GetRootUrl(id);
        }
        /// <summary>
        /// 获取根菜单id
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public int GetRootMenuId(List<sys_user_menu> menu, string controller, string action)
        {
            sys_user_menu currentMenu = menu.FirstOrDefault(m => m.ControllName == controller && m.ActionName == action);
            if (currentMenu == null || currentMenu.ParentId == null || currentMenu.ParentId == 0)
                return currentMenu == null ? 0 : currentMenu.Id;
            return RetRootID(menu, (int)currentMenu.ParentId);
        }
        /// <summary>
        /// 获取根菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private int RetRootID(List<sys_user_menu> menu, int parentId)
        {
            var pmenu = menu.FirstOrDefault(p => p.Id == parentId);
            if (pmenu == null || pmenu.ParentId == null || pmenu.ParentId == 0) return parentId;
            return RetRootID(menu, (int)pmenu.ParentId);
        }
        /// <summary>
        /// 获取当前页面所属菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public sys_user_menu GetWapMenu(List<sys_user_menu> menu, string controller, string action)
        {
            return menu.FirstOrDefault(p => p.ControllName == controller && p.ActionName == action);
        }
        public sys_user_menu GetWapMenu(List<sys_user_menu> menu, int id)
        {
            return menu.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// 获得当前路径完整地址
        /// </summary>
        /// <returns></returns>
        public List<sys_user_menu> GetNavMenuList(List<sys_user_menu> menu, string controller, string action)
        {
            sys_user_menu m = GetWapMenu(menu, controller, action);

            List<sys_user_menu> menus = new List<sys_user_menu>();
            int i = 0;
            while (m != null)
            {
                menus.Add(m);
                if (m.ParentId.HasValue)
                    m = GetWapMenu(menu, m.ParentId.Value);
                else
                    m = null;
                i++;
                if (i > 10) break;
            }
            return menus.ToArray().Reverse().Skip(1).ToList();
        }
    }
}