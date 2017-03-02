using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeixinService.Common.MessageHandlers {
    /// <summary>
    /// 微信关注客户消息对象
    /// </summary>
    public class WeixinMessenger {
        /// <summary>
        /// 设置或获取微信服务号的用户OpenId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 设置或获取关注次序
        /// </summary>
        public int SubscribedSequence { get; set; }

        /// <summary>
        /// 设置或获取用户是否为再次关注的
        /// </summary>
        public bool IsResubscribed { get; set; }

        /// <summary>
        /// 设置或获取用户关注的场景二维码参数（格式qrscene_{数字}）
        /// </summary>
        public string SubscribeScene { get; set; }

        /// <summary>
        /// 获取用户是否通过场景二维码关注的
        /// </summary>
        public bool IsSubscribedFromQrScene {
            get {
                return !string.IsNullOrEmpty(SubscribeScene) && SubscribeScene.ToLower().StartsWith("qrscene_");
            }
        }
        /// <summary>
        /// 获取用户关注的场景二维码数值
        /// </summary>
        public int QrScene {
            get {
                return SubscribeScene.IndexOf('_') > 0 ? int.Parse(SubscribeScene.Split('_')[1]) : 0;
            }
        }

        public DateTime SubscribedOn { get; set; }

        internal void Subscribe(object state) {
            WeixinSubscribeBL bl = new WeixinSubscribeBL(this);
            //
            bl.ProcessSubscribe();
        }

        internal void UnSubscribe(object state) {
            WeixinSubscribeBL bl = new WeixinSubscribeBL(this);
            bl.ProcessUnSubscribe();
        }

        /// <summary>
        /// 设置关注次序
        /// </summary>
        internal void PrepareToSubscribe() {
            //this.SubscribedSequence = WeixinSubscribeHelper.CurrentFocusSequence; // 计算关注次序
        }

        internal void PrepareToUnSubscribe() {
            //
            //TODO：Do something
            //
        }

    }
}
