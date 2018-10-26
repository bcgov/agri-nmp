using System.Collections.Generic;

namespace Agri.Models
{
    public class STKKelownaRange
    {
        public int Id { get; set; }
        public string Range { get; set; }
        public int RangeLow { get; set; }
        public int RangeHigh { get; set; }

        public List<STKRecommend> STKRecommendations { get; set; }
    }
}