using AliYunModel;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Authorization {
    public interface IWexinUserManagement {
        BaseUser Login(string userKey);
        BaseUser CreateAndRelevance(string code);
        
        bool Add(WeixinCustomer entity);

        bool Relevance(string code, int userId);
        bool UnRelevance(string code);
    }
}
