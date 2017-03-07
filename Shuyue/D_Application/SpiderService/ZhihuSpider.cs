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
                    string htmlcontent = cp.Crawl("https://www.zhihu.com/");
                    var qalist = cp.PageQuestionListHandel(htmlcontent);
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
