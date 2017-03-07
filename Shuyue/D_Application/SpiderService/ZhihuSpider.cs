using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using YaoService.Zhihu;

namespace SpiderService
{
    partial class ZhihuSpider : ServiceBase
    {
        public ZhihuSpider()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
        }
        private void AutoWork()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.GetZhihuAnswer));
        }

        public void GetZhihuAnswer(object args)
        {

            while (true)
            {
                try
                {
                    CrawlPage cp = new CrawlPage();
                    var qalist = cp.PageQuestionListHandel(cp.Crawl("https://www.zhihu.com/"));
                    StringBuilder sqlsb = new StringBuilder();
                    sqlsb.Append("insert into ZhihuAnswer (QuestionId,AnswerId,Question,Author,Bio,Summary,Content,ZanCount,ViewCount) ");
                    int addIndex = 0;
                    foreach (var item in qalist)
                    {
                        sqlsb.Append(string.Format("select '{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},{8}",
                            item.QuestionId, item.AnswerId, item.Question, item.Author, item.Bio, item.Summary, item.Content
                            , item.ZanCount, item.ViewCount));
                        if (++addIndex < qalist.Count)
                        {
                            sqlsb.Append(" union ");
                        }
                    }
                    Core.AppContext.Current.ESqlUtil(Core.Enum.DbConnEnum.ZhiHu).RunSql(sqlsb.ToString());
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    Thread.Sleep(5 * 60 * 1000);
                }
            }
        }
    }
}
