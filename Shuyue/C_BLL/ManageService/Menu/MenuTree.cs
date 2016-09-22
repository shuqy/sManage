using ManageEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageService.Menu
{
    public class MenuTree
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public int? SortValue { get; set; }
        public List<MenuTree> ChildrenMenu { get; set; }
    }
}
