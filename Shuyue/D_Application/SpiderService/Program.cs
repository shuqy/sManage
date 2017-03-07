using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SpiderService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
#if DEBUG
            ZhihuSpider zs = new ZhihuSpider();
            zs.GetZhihuAnswer(null);
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ZhihuSpider()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
