using AliYunModel;
using Core.Entities;
using Core.Utilities;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeixinService.Common.MessageHandlers {

    /// <summary>
    /// 微信关注与取消关注逻辑
    /// </summary>
    public class WeixinSubscribeBL {
        private WeixinMessenger _User;
        private AliDbEntities _DbContext;
        public AliDbEntities DbContext { get { return _DbContext; } }
        private string AccessToken {
            get {
                return AccessTokenContainer.GetAccessToken(ConfigHelper.Get(AppSettingsKey.AppId), false);
            }
        }

        public WeixinSubscribeBL(WeixinMessenger user) {
            _User = user;
            //设计要点：EF的上下文在Web的应用场景下，采用每个请求对应一个Context。
            _DbContext = new AliDbEntities();
        }

        /// <summary>
        /// 处理关注的逻辑
        /// </summary>
        internal void ProcessSubscribe() {
            // 更新客户资料
            CreateOrUpdateCustomer(_User, true);
        }

        /// <summary>
        /// 创建或更新用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="calledBySubscribeEvent"></param>
        private void CreateOrUpdateCustomer(WeixinMessenger user, bool calledBySubscribeEvent = true) {
            var focusItem = DbContext.WeixinCustomer.FirstOrDefault(u => u.OpenId.Equals(user.OpenId, StringComparison.OrdinalIgnoreCase));
            if (focusItem == null) {//新客户关注
                focusItem = new WeixinCustomer {
                    OpenId = user.OpenId,
                    CreateOn = DateTime.Now
                };
                DbContext.WeixinCustomer.Add(focusItem);
            } else {
                if (calledBySubscribeEvent)
                    user.IsResubscribed = true;
                focusItem.UnSubscribed = false;
                focusItem.ModifyOn = DateTime.Now;
            }

            UserInfoJson wxUser = RequestUserInfo(user);

            if (wxUser != null) {
                focusItem.Nickname = wxUser.nickname;
                focusItem.HeadImg = wxUser.headimgurl;
                focusItem.Gender = wxUser.sex == 0 ? false : true;
                focusItem.Country = wxUser.country;
                focusItem.Province = wxUser.province;
                focusItem.City = wxUser.city;
                focusItem.Language = wxUser.language;
                focusItem.SubscribeDate = DateTime.Now;
                focusItem.UnSubscribed = false;
                if (calledBySubscribeEvent)//只有在客户关注时才更新
                    focusItem.LastActivityTime = DateTime.Now;
                if (user.IsSubscribedFromQrScene) // 注意：这里是带前缀的
                {//如果是通过扫描专属二维码关注的逻辑
                }

                DbContext.SaveChanges();

                var customer = CreateOrUpdateCustomerWithFocusUser(focusItem);

                //关注成功后动作
                //if (calledBySubscribeEvent)
                //    ExecuteNotification(customer, user);
            }
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        internal void ProcessUnSubscribe() {
            var wxUser = DbContext.WeixinCustomer.SingleOrDefault(m => m.OpenId == _User.OpenId);
            if (wxUser != null) {
                wxUser.UnSubscribed = true;
                wxUser.UnSubscribeDate = DateTime.Now;
                DbContext.SaveChanges();
            }
        }

        private void ExecuteNotification(object customer, WeixinMessenger user) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据关注用户创建或更新系统用户记录
        /// </summary>
        /// <param name="focusItem"></param>
        /// <returns></returns>
        private object CreateOrUpdateCustomerWithFocusUser(WeixinCustomer focusItem) {
            Customer customer;
            if (!focusItem.CustomerId.HasValue) {
                customer = CreateCustomerWithFocusUser(focusItem);
                focusItem.CustomerId = customer.Id;
            } else {
                int customerId = (int)focusItem.CustomerId;
                customer = DbContext.Customer.FirstOrDefault(c => c.Id == customerId);
                if (customer.Username.ToString().StartsWith("user_"))
                    customer.Username = focusItem.Nickname;
            }
            DbContext.SaveChanges();
            return customer;
        }

        /// <summary>
        /// 首次关注用户创建Customer信息
        /// </summary>
        /// <param name="focusItem"></param>
        /// <returns></returns>
        private Customer CreateCustomerWithFocusUser(WeixinCustomer focusItem) {
            Customer customer = new Customer {
                Username = focusItem.Nickname,
                CreateOn = DateTime.Now,
                Deleted = false,
            };
            DbContext.Customer.Add(customer);
            DbContext.SaveChanges();
            //todo:添加默认角色
            return customer;
        }

        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private UserInfoJson RequestUserInfo(WeixinMessenger user) {
            UserInfoJson wxUser = null;
            wxUser = UserApi.Info(AccessToken, user.OpenId);
            return wxUser;
        }

        private void CreateWeixinCustomer(WeixinMessenger user) {

        }
    }
}
