using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class PageData
    {
        public School School { get; set; }
        public IEnumerable<Criteria> Criterias { get; set; }
        public IEnumerable<Index> Indexes { get; set; }
        public IEnumerable<Rate> Rates { get; set; }
        public IEnumerable<SchoolMenu> SchoolMenus { get; set; }
    }
}
