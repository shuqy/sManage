using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Html;
using Core.Util;
using Model.ViewModel.Zhihu;
using Core;
using System.Net;

namespace YaoService.Zhihu
{
    /// <summary>
    /// 页面数据抓取
    /// </summary>
    public class CrawlPage
    {
        private HttpHelper _httpHelper;
        private LogHelper _logger;
        private string _userAgent;
        private int _zanLevel;
        private ZhihuLogin _zhihuLogin;
        public CrawlPage()
        {
            _httpHelper = new HttpHelper();
            _logger = new LogHelper();
            _userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0";
            _zanLevel = Convert.ToInt32(ConfigHelper.Get("ZanLevel"));
            _zhihuLogin = new ZhihuLogin();
        }

        /// <summary>
        /// 抓取
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string Crawl(string url)
        {
            HttpItem item = new HttpItem
            {
                URL = url,
                UserAgent = _userAgent,
                Cookie = _zhihuLogin.Cookie,
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
            };
            HttpResult result = _httpHelper.GetHtml(item);
            return result.Html;
        }

        public List<ZhihuAnswer> PageQuestionListHandel(string html)
        {
            var dbcontext = AppContext.Current.ESqlUtil(Core.Enum.DbConnEnum.ZhiHu);
            var nqalist = dbcontext.Get<ZhihuAnswer>().ToList();
            List<ZhihuAnswer> answerList = new List<ZhihuAnswer>();
            List<string> contentList = MelonReg.FindList(html, "<div class=\"feed-item-inner\">", "<div class=\"feed-meta\">");
            foreach (var content in contentList)
            {
                string zan = MelonReg.FindContent(content, "<span class=\"count\">", "<");
                zan = zan.Replace("K", "000");
                if (string.IsNullOrEmpty(zan) || Convert.ToInt32(zan) < _zanLevel) continue;
                string questionLink = MelonReg.FindContent(content, "<h2 class=\"feed-title\">", "</h2>");
                string questionId = MelonReg.FindContent(questionLink, "/question/", "/");
                string answerId = MelonReg.FindContent(questionLink, "answer/", "\"");
                string question = MelonReg.FindLinkTitle(questionLink);
                string author = MelonReg.FindContent(content, "data-author-name=\"", "\"");
                author = string.IsNullOrEmpty(author) ? "匿名用户" : author;
                //判断文章是否已存在
                if (nqalist.Any(n => n.QuestionId == questionId && n.AnswerId == answerId)) continue;
                //作者介绍
                string bio = MelonReg.FindContent(content, "<span.*?class=\"bio\">", "</span>");
                bio = bio.Length > 0 ? bio.Substring(1) : "";
                string summary = MelonReg.FindContent(content, "<div class=\"zh-summary.*?>", "<a.*?>显示全部</a>");
                summary = MelonReg.RemoveLink(summary);
                string contentbody = WebUtility.HtmlDecode(MelonReg.FindContent(content, "<textarea.*?>", "</textarea>"));
                //获取文章中图片列表，并下载
                List<ImgData> imgList = MelonReg.FindImgList(contentbody);
                foreach (var img in imgList)
                {
                    DownloadImg.Load(img.realPath, img.path);
                    contentbody = contentbody.Replace(img.src, img.url);
                }
                //改图片样式
                contentbody = MelonReg.ChangeWidth(contentbody);
                //去掉最后编辑日期链接
                contentbody = MelonReg.RemoveEditDateLink(contentbody);
                //将文件内容写入txt文件，节省数据库空间
                string txtName = RandomOperate.RansdomName(10) + ".txt";
                string txtfile = ConfigHelper.Get("TxtPath") + txtName;
                FileOperate.WriteFile(txtfile, contentbody);
                //获得知乎答案对象
                ZhihuAnswer ans = new ZhihuAnswer
                {
                    QuestionId = questionId,
                    AnswerId = answerId,
                    Question = question,
                    Author = author,
                    Bio = bio,
                    Summary = summary,
                    Content = "/Content/TxtFile/" + txtfile,
                    ZanCount = Convert.ToInt32(zan),
                    CreatedOn = DateTime.Now,
                    Deleted = false,
                    ViewCount = 0,
                    Recommended = false
                };

                answerList.Add(ans);
            }
            return answerList;
        }
    }
}
