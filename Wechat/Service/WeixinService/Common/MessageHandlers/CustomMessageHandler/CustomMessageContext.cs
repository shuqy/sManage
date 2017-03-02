using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeixinService.Common.MessageHandlers.CustomMessageHandler {
    public class CustomMessageContext : MessageContext<IRequestMessageBase, IResponseMessageBase> {

        public CustomMessageContext() {
            base.MessageContextRemoved += CustomMessageContext_MessageContextRemoved;
        }

        /// <summary>
        /// 当上下文过期，被移除时触发的时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomMessageContext_MessageContextRemoved(object sender, WeixinContextRemovedEventArgs<IRequestMessageBase, IResponseMessageBase> e) {
            var messageContext = e.MessageContext as CustomMessageContext;
            if (messageContext == null) {
                return;//如果是正常的调用，messageContext不会为null
            }
        }
    }
}
