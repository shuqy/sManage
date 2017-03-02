using AliYunModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaoService.Zhihu {
    public class ZhihuService {

        private AliDbEntities dbcontext;
        public ZhihuService() {
            dbcontext = new AliDbEntities();
        }

        public ZhihuAnswer GetQAById(int id) {
            return dbcontext.ZhihuAnswer.FirstOrDefault(z => z.Id == id);
        }

        public List<ZhihuAnswer> GetList() {
            return dbcontext.ZhihuAnswer.Where(a => a.ZanCount >= 5000).OrderByDescending(z => z.Id).ToList();
        }

        public bool AddReadHistory(ZhihuReadHistory history) {
            dbcontext.ZhihuReadHistory.Add(history);
            return dbcontext.SaveChanges() > 0;
        }
    }
}
