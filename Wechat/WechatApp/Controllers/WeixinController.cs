using Core.Entities;
using Core.Utilities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Menu;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WeixinService.Common.MessageHandlers.CustomMessageHandler;

namespace WechatApp.Controllers {
    public class WeixinController : Controller {


        private readonly string Token = ConfigHelper.Get("Token");//与微信公众账号后台的Token设置保持一致，区分大小写。
        private string AppId = ConfigHelper.Get("AppId");//与微信公众账号后台的AppId设置保持一致，区分大小写。
        private string EncodingAESKey = ConfigHelper.Get("AppSecret");//与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。



        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：url/weixin
        /// </summary>
        [HttpGet]
        public ActionResult Index(PostModel postModel, string echostr) {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token)) {
                return Content(echostr); //返回随机字符串则表示验证通过
            } else {
                return Content("签名：" + CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url。");
            }
        }

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML
        /// 包括关注，取消关注等事件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(PostModel postModel) {
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token)) {
                return Content("参数错误！");
            }
            postModel.Token = Token;
            postModel.EncodingAESKey = EncodingAESKey;//根据自己后台的设置保持一致
            postModel.AppId = AppId;//根据自己后台的设置保持一致

            //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
            var maxRecordCount = 10;

            var logPath = Server.MapPath(string.Format("~/App_Data/MP/{0}/", DateTime.Now.ToString("yyyy-MM-dd")));
            if (!Directory.Exists(logPath)) {
                Directory.CreateDirectory(logPath);
            }
            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new CustomMessageHandler(Request.InputStream, postModel, maxRecordCount);
            try {

                //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                messageHandler.RequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_{1}.txt", DateTime.Now.Ticks, messageHandler.RequestMessage.FromUserName)));
                if (messageHandler.UsingEcryptMessage) {
                    messageHandler.EcryptRequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_Ecrypt_{1}.txt", DateTime.Now.Ticks, messageHandler.RequestMessage.FromUserName)));
                }

                /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
                 * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
                messageHandler.OmitRepeatedMessage = true;

                //执行微信处理过程
                messageHandler.Execute();

                if (messageHandler.ResponseDocument != null) {
                    messageHandler.ResponseDocument.Save(Path.Combine(logPath, string.Format("{0}_Response_{1}.txt", DateTime.Now.Ticks, messageHandler.RequestMessage.FromUserName)));
                }

                if (messageHandler.UsingEcryptMessage) {
                    //记录加密后的响应信息
                    messageHandler.FinalResponseDocument.Save(Path.Combine(logPath, string.Format("{0}_Response_Final_{1}.txt", DateTime.Now.Ticks, messageHandler.RequestMessage.FromUserName)));
                }

                //return Content(messageHandler.ResponseDocument.ToString());//v0.7-
                //return new FixWeixinBugWeixinResult(messageHandler);
                return new WeixinResult(messageHandler);//v0.8+
            } catch (Exception ex) {
                using (TextWriter tw = new StreamWriter(Server.MapPath("~/App_Data/Error_" + DateTime.Now.Ticks + ".txt"))) {
                    tw.WriteLine("ExecptionMessage:" + ex.Message);
                    tw.WriteLine(ex.Source);
                    tw.WriteLine(ex.StackTrace);
                    //tw.WriteLine("InnerExecptionMessage:" + ex.InnerException.Message);

                    if (messageHandler.ResponseDocument != null) {
                        tw.WriteLine(messageHandler.ResponseDocument.ToString());
                    }

                    if (ex.InnerException != null) {
                        tw.WriteLine("========= InnerException =========");
                        tw.WriteLine(ex.InnerException.Message);
                        tw.WriteLine(ex.InnerException.Source);
                        tw.WriteLine(ex.InnerException.StackTrace);
                    }

                    tw.Flush();
                    tw.Close();
                }
                return Content("");
            }
        }


        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateMenu() {
            GetMenuResultFull menu = new GetMenuResultFull {
                menu = new MenuFull_ButtonGroup {
                    button = new List<MenuFull_RootButton> {
                        new MenuFull_RootButton {
                            name = "要起风了",
                            type = ButtonType.view.ToString(),
                              url = string.Format("{0}/Qipa",ConfigHelper.Get("LocalUrl"))
                        },
                        new MenuFull_RootButton {
                            name = "为了西凉",
                            type = ButtonType.view.ToString(),
                              url = string.Format("{0}",ConfigHelper.Get("LocalUrl"))
                        },
                    }
                }
            };
            WeixinService.Mp.Menu.Create(menu);
            return null;
        }
    }
}