using Core.Entities;
using Core.Enum;
using Core.Util;
using ManageEF;
using ManageEF.ViewModel;
using ManageService.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Manage.Controllers
{
    public class UserController : ControllerBase
    {
        UserBLL userBLL = new UserBLL();
        /// <summary>
        /// 用户首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult SearchUser(DataTableRequest param)
        {
            var list = userBLL.GetEntities();
            var res = list.OrderByDescending(l => l.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var query = res.Select(r => new { r.UserName, r.UserCode, r.PassCode, r.Mobile, r.Email, r.Id, r.State, r.Deleted });
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = list.Count(),
                iTotalDisplayRecords = list.Count(),
                aaData = query.ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
        [Description("添加或编辑")]
        public ActionResult Edit(int id = 0)
        {
            sys_user user = null;
            if (id > 0) user = userBLL.GetEntities(u => u.Id == id).FirstOrDefault();
            return View(user ?? new sys_user());
        }
        [HttpPost]
        public JsonResult Edit(sys_user user, FormCollection collect)
        {
            sys_user nu = new sys_user();
            if (user.Id == 0)
            {
                nu = user;
                nu.CreatedOn = DateTime.Now;
                nu.Deleted = false;
                nu.CreatedBy = 0;
                nu.PassCode = Encryptor.EncryptDES(nu.PassCode);
                userBLL.Insert(user);
            }
            else
            {
                nu = userBLL.GetEntities(u => u.Id == user.Id).FirstOrDefault();
                if (nu == null)
                    return Json(new JsonData { Code = ResultCode.Fail, }, JsonRequestBehavior.DenyGet);
                nu.Mobile = user.Mobile;
                nu.Email = user.Email;
                nu.UserCode = user.UserCode;
                nu.PassCode = Encryptor.EncryptDES(user.PassCode);
                nu.State = user.State;
                nu.UserName = user.UserName;
                userBLL.Update(nu);
            }
            //保存管理组数据
            UserGroupMappingBLL ugmBll = new UserGroupMappingBLL();
            var grouplist = ugmBll.GetEntities(a => a.UserId == nu.Id);
            List<user_group_mapping> userGroupMapping = new List<user_group_mapping>();
            if (collect.GetValues("checkboxGroup") != null)
            {
                string strRoles = collect.GetValue("checkboxGroup").AttemptedValue;
                string[] lstRoles = strRoles.Split(',');
                foreach (string role in lstRoles)
                {
                    userGroupMapping.Add(new user_group_mapping { UserId = nu.Id, GroupId = Convert.ToInt32(role) });
                }
            }
            if (grouplist.Any())
                ugmBll.Delete(grouplist);
            if (userGroupMapping.Any())
                ugmBll.Insert(userGroupMapping);
            return Json(new JsonData { Code = ResultCode.OK, }, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public JsonResult EditState(int id)
        {
            sys_user nu = userBLL.GetEntities().FirstOrDefault(u => u.Id == id);
            nu.State = nu.State == 0 ? 1 : 0;
            userBLL.Update(nu);
            return Json(new JsonData { Code = ResultCode.OK, }, JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 获取用户管理组列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userGroupIds"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string UserGroupOption(int userId, IEnumerable<user_group_mapping> userGroupIds, string name = "")
        {
            UserGroupBLL userGroupBLL = new UserGroupBLL();
            UserBLL userBLL = new UserBLL();
            IEnumerable<sys_user_group> groupList = userGroupBLL.GetEntities(u => u.State == true);
            StringBuilder sb = new StringBuilder();
            foreach (sys_user_group group in groupList)
            {
                sb.AppendLine("<input type=\"checkbox\" name=\"" + name + "\" value=\"" + group.Id + "\""
                    + (userGroupIds.Any(m => m.GroupId == group.Id) ? "checked" : "") + " />" + group.Title);
            }
            return sb.ToString();
        }        
    }
}