using Core;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WeixinService.Common.MessageHandlers {
    public static class WeixinSubscribeHelper {
        private static Queue<WeixinMessenger> SubscribedQueue;
        private static Queue<WeixinMessenger> UnSubscribedQueue;

        static WeixinSubscribeHelper() {
            Init();
        }

        static void Init() {
            var dbcontext = AppContext.Current.DbContext;
            SubscribedQueue = new Queue<WeixinMessenger>();
            UnSubscribedQueue = new Queue<WeixinMessenger>();

            // 启动线程进行处理关注逻辑
            Thread subscribeThread = new Thread(new ParameterizedThreadStart(ThreadProcessSubscribedUser));
            subscribeThread.Start();

            // 启动线程进行处理取消关注逻辑
            Thread unsubscribeThread = new Thread(new ParameterizedThreadStart(ThreadProcessUnSubscribedUser));
            unsubscribeThread.Start();
        }

        /// <summary>
        /// 线程处理用户关注逻辑
        /// </summary>
        /// <param name="state"></param>
        private static void ThreadProcessSubscribedUser(object state) {
            lock (SubscribedQueue) {
                while (true) {
                    if (SubscribedQueue.Count > 0) {
                        var user = SubscribedQueue.Dequeue();
                        ThreadPool.QueueUserWorkItem(new WaitCallback(user.Subscribe));
                    }
                    Monitor.Wait(SubscribedQueue, 5000);
                }
            }
        }
        /// <summary>
        /// 线程处理取消关注逻辑
        /// </summary>
        /// <param name="state"></param>
        private static void ThreadProcessUnSubscribedUser(object state) {
            lock (UnSubscribedQueue) {
                while (true) {
                    if (UnSubscribedQueue.Count > 0) {
                        var user = UnSubscribedQueue.Dequeue();
                        ThreadPool.QueueUserWorkItem(new WaitCallback(user.UnSubscribe));
                    }
                    Monitor.Wait(UnSubscribedQueue, 5000);
                }
            }
        }

        /// <summary>
        /// 处理用户关注后的业务逻辑
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public static IResponseMessageBase ProcessSubscribeRequest(RequestMessageEvent_Subscribe requestMessage) {
            StartSubscribe(requestMessage.FromUserName, requestMessage.EventKey, requestMessage.CreateTime);

            // 此时应该立即给微信服务器一个应答结果避免重复推送关注事件
            // 然而，有时候确实因为数据往返需要的时间超过了5秒，也会收到重复关注事件的消息
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = ""; //表示收到推送
            return responseMessage;
        }


        private static void StartSubscribe(string openId, string qrscene, DateTime subscribedOn) {
            var user = new WeixinMessenger {
                OpenId = openId,
                SubscribeScene = qrscene,
                SubscribedOn = subscribedOn,
            };

            // 启动线程处理关注逻辑
            Thread mainThread = new Thread(new ParameterizedThreadStart(PrepareOneSubscribedUser));
            mainThread.Start(user);
        }

        private static void PrepareOneSubscribedUser(object state) {
            lock (SubscribedQueue) {
                var user = state as WeixinMessenger;
                // 队列里已经存在一个
                if (!SubscribedQueue.Any(c => c.OpenId == user.OpenId && c.SubscribedOn == user.SubscribedOn)) {
                    SubscribedQueue.Enqueue(user); // 可以进队列, 等关注处理逻辑处理完毕后会出队列。
                    user.PrepareToSubscribe();
                }
                Monitor.Pulse(SubscribedQueue);
            }
        }

        /// <summary>
        /// 处理用户取消关注的业务逻辑
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public static IResponseMessageBase ProcessUnSubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage) {
            ProcessUnSubscribe(requestMessage.FromUserName, requestMessage.CreateTime);

            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = ""; // 取消关注用户并不能收消息。
            return responseMessage;
        }

        private static void ProcessUnSubscribe(string fromUserName, DateTime createTime) {
            var user = new WeixinMessenger {
                OpenId = fromUserName,
                SubscribedOn = createTime
            };

            // 启动线程处理取消关注逻辑
            Thread mainThread = new Thread(new ParameterizedThreadStart(PrepareOneUnSubscribedUser));
            mainThread.Start(user);
        }
        private static void PrepareOneUnSubscribedUser(object state) {
            lock (UnSubscribedQueue) {
                var user = state as WeixinMessenger;
                // 队列里已经存在一个
                if (!UnSubscribedQueue.Any(c => c.OpenId == user.OpenId && c.SubscribedOn == user.SubscribedOn)) {
                    UnSubscribedQueue.Enqueue(user); // 可以进队列, 等取消关注处理逻辑处理完毕后会出队列。
                    user.PrepareToUnSubscribe();
                }
                Monitor.Pulse(UnSubscribedQueue);
            }
        }
    }
}
