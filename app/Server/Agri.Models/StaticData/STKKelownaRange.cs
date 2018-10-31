using System.Collections.Generic;

namespace Agri.Models.StaticData
{
    public class STKKelownaRange
    {
        public STKKelownaRange()
        {
            STKRecommendations = new List<STKRecommend>();
        }
        public int Id { get; set; }
        public string Range { get; set; }
        public int RangeLow { get; set; }
        public int RangeHigh { get; set; }

        public List<STKRecommend> STKRecommendations { get; set; }
    }
}