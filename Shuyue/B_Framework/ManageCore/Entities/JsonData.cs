using Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class JsonData
    {
        public ResultCode Code { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }
}
