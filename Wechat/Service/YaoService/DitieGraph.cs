using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaoService {
    public class DitieGraph : SymbolGraph {
        public DitieGraph(IEnumerable<string> list, char sp) : base(list.Select(l => l.Split('-').FirstOrDefault()), sp) {

        }
    }
}
