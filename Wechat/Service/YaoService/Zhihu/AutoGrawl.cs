using AliYunModel;
using Core;
using Core.Utilities;
using Senparc.Weixin.QY.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeixinService.Common;

namespace YaoService.Zhihu {
    public class AutoGrawl {
        public static void GetAnswer() {
            try {

                CrawlPage cp = new CrawlPage();
                string htmlcontent = cp.Crawl("https://www.zhihu.com/");
                var qalist = cp.PageQuestionListHandel(htmlcontent);
                var dbcontext = AppContext.Current.DbContext;
                var nqalist = dbcontext.ZhihuAnswer.ToList();
                var clist = new List<ZhihuAnswer>();
                foreach (var a in qalist) {
                    if (!nqalist.Any(n => n.QuestionId == a.QuestionId && n.AnswerId == a.AnswerId)) {
                        clist.Add(a);
                    }
                }
                dbcontext.ZhihuAnswer.AddRange(clist);
                dbcontext.SaveChanges();
                foreach (var a in clist) {
                    if (a.ZanCount < Convert.ToInt32(ConfigHelper.Get("RecommendZanLevel"))) continue;
                    Article article = new Article {
                        Title = a.Question,
                        Description = a.Summary,
                        Url = "http://www.wmylife.com/Zhihu/QADetials?zid=" + a.Id
                    };
                    WeixinQyMsgHelper.SendNews("@all", ConfigHelper.Get("YaoAgentId"), new List<Article>() { article });
                }
            } catch (Exception ex) {
                LogHelper lh = new LogHelper();
                lh.Write(new Msg {
                    Datetime = DateTime.Now,
                    Type = MsgType.Error,
                    Text = string.Format("知乎抓取异常，请检查。{0}", ex.Message),
                });
            }
        }
    }
}
