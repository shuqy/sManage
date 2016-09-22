using ManageEF;
using ManageService.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageService.Menu
{
    public class MenuBLL : RepositoryBase<sys_user_menu>
    {
        /// <summary>
        /// 获取所有菜单树
        /// </summary>
        /// <returns></returns>
        public MenuTree GetMenuTree()
        {
            List<sys_user_menu> list = GetEntities().ToList();
            MenuTree tree = new MenuTree()
            {
                Title = "",
                ParentId = 0,
                Id = 0,
                ChildrenMenu = new List<MenuTree>(),
            };
            GetChildrenMenu(list, tree);
            return tree;
        }
        private void GetChildrenMenu(List<sys_user_menu> list, MenuTree tree)
        {
            int parentId = tree.Id;
            var clist = list.Where(m => m.ParentId == parentId);
            if (clist == null || !clist.Any()) return;
            tree.ChildrenMenu = clist.Select(a => new MenuTree
            {
                Title = a.Title,
                Description = a.Description,
                Id = a.Id,
                ParentId = a.ParentId,
                SortValue = a.SortValue,
                ChildrenMenu = new List<MenuTree>(),
            }).OrderBy(m => m.SortValue).ToList();
            foreach (MenuTree menu in tree.ChildrenMenu)
                GetChildrenMenu(list, menu);
        }
        public List<sys_user_menu> GetUserMenu()
        {
            if (Core.AppContext.Current.UserMenu == null)
            {
                string sql = "select menu.* from [dbo].[user_group_mapping] u "
                    + "inner join [dbo].[group_menu_mapping] m on u.GroupId=m.GroupId "
                    + "inner join [dbo].[sys_user_menu] menu on m.MenuID=menu.Id "
                    + "where u.UserId=@userid and menu.state=1";
                if (Core.AppContext.Current.CurrentUser == null) return null;
                SqlParameter param = new SqlParameter("@userid", Core.AppContext.Current.CurrentUser.Id);
                List<sys_user_menu> menuList = Db.Database.SqlQuery<sys_user_menu>(sql, param).OrderBy(m => m.SortValue).ToList();
                Core.AppContext.Current.UserMenu = menuList;
            }
            return Core.AppContext.Current.UserMenu;
        }

        public IEnumerable<sys_user_menu> GetChildMenu(int parentId)
        {
            return GetUserMenu().Where(p => p.ParentId == parentId).OrderBy(p => p.SortValue);
        }

        public string GetRootUrl(int id)
        {
            sys_user_menu secondMenu = GetChildMenu(id).FirstOrDefault();
            if (secondMenu == null) return "#";
            if (secondMenu.IsPath == true)
            {
                sys_user_menu thirdMenu = GetChildMenu(secondMenu.Id).FirstOrDefault();
                if (thirdMenu == null) return "#";
                return string.Format("/{0}/{1}", thirdMenu.ControllName, thirdMenu.ActionName);
            }
            return string.Format("/{0}/{1}", secondMenu.ControllName, secondMenu.ActionName);
        }
    }
}
