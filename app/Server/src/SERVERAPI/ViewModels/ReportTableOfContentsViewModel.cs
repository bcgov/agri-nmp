using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.ViewModels
{
    public class ReportTableOfContentsViewModel
    {
        public ReportTableOfContentsViewModel()
        {
            ContentItems = new List<ContentItem>();
        }
        public List<ContentItem> ContentItems { get; set; }

    }

    public class ContentItem
    {
        public string SectionName { get; set; }
        public int PageNumber { get; set; }
    }
}
