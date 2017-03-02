using AliYunModel;
using Core;
using Core.Entities;
using Core.Utilities;
using DotNet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YaoService.Zhihu {
    public class CrawlPage {
        private HttpHelper _httpHelper;
        private LogHelper _logger;
        private string _userAgent;
        private int _zanLevel;
        public CrawlPage() {
            _httpHelper = new HttpHelper();
            _logger = new LogHelper();
            _userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0";
            _zanLevel = Convert.ToInt32(ConfigHelper.Get("ZanLevel"));

            //test
            //SuccessCookie.Cookie = "";
        }

        public string CrawlHomePage() {
            HttpItem item = new HttpItem {
                URL = "https://www.zhihu.com/",
                UserAgent = _userAgent,
                Cookie = LoginSuccess.Cookie,
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
            };
            HttpResult result = _httpHelper.GetHtml(item);
            return result.Html;
        }

        public string Crawl(string url) {
            HttpItem item = new HttpItem {
                URL = url,
                UserAgent = _userAgent,
                Cookie = LoginSuccess.Cookie,
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
            };
            HttpResult result = _httpHelper.GetHtml(item);
            return result.Html;
        }

        public List<ZhihuAnswer> PageQuestionListHandel(string html) {
            var dbcontext = AppContext.Current.DbContext;
            var nqalist = dbcontext.ZhihuAnswer.ToList();
            List<ZhihuAnswer> answerList = new List<ZhihuAnswer>();
            List<string> contentList = HtmlReg.FindList(html, "<div class=\"content\">", "<div class=\"feed-meta\">");
            foreach (var content in contentList) {
                string zan = HtmlReg.FindContent(content, "<span class=\"count\">", "<");
                zan = zan.Replace("K", "000");
                if (string.IsNullOrEmpty(zan) || Convert.ToInt32(zan) < _zanLevel) continue;
                string questionLink = HtmlReg.FindContent(content, "<h2>", "</h2>");
                string questionId = HtmlReg.FindContent(questionLink, "/question/", "#");
                string answerId = HtmlReg.FindContent(questionLink, "answer-", "\"");
                string question = HtmlReg.FindLinkTitle(questionLink);
                string author = HtmlReg.FindContent(content, "<a class=\"author-link\".*?>", "</a>");
                author = string.IsNullOrEmpty(author) ? "匿名用户" : author;
                //判断文章是否已存在
                if (nqalist.Any(n => n.QuestionId == questionId && n.AnswerId == answerId)) continue;
                //作者介绍
                string bio = HtmlReg.FindContent(content, "<span.*?class=\"bio\">", "</span>");
                bio = bio.Length > 0 ? bio.Substring(1) : "";
                string summary = HtmlReg.FindContent(content, "<div class=\"zh-summary.*?>", "<a.*?>显示全部</a>");
                summary = HtmlReg.RemoveLink(summary);
                string contentbody = WebUtility.HtmlDecode(HtmlReg.FindContent(content, "<textarea.*?>", "</textarea>"));
                //获取文章中图片列表，并下载
                List<ImgData> imgList = HtmlReg.FindImgList(contentbody);
                foreach (var img in imgList) {
                    DownloadImg.Load(img.realPath, img.path);
                    contentbody = contentbody.Replace(img.src, img.url);
                }
                //改图片样式
                contentbody = HtmlReg.ChangeWidth(contentbody);
                //去掉最后编辑日期链接
                contentbody = HtmlReg.RemoveEditDateLink(contentbody);
                //将文件内容写入txt文件，节省数据库空间
                string txtfile = ConfigHelper.Get("TxtPath") + RandomOperate.RansdomName(10) + ".txt";
                FileOperate.WriteFile(txtfile, contentbody);
                //获得知乎答案对象
                ZhihuAnswer ans = new ZhihuAnswer {
                    QuestionId = questionId,
                    AnswerId = answerId,
                    Question = question,
                    Author = author,
                    Bio = bio,
                    Summary = summary,
                    Content = txtfile,
                    ZanCount = Convert.ToInt32(zan),
                    CreatedOn = DateTime.Now,
                    deleted = false,
                    ViewCount = 0,
                    Recommended = false
                };

                answerList.Add(ans);
            }
            return answerList;
        }

        public void RQ(string url) {
            string htmlcontent = Crawl(url);
            PageQuestionListHandel(htmlcontent);
        }
    }
}
