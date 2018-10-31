using System.Collections.Generic;

namespace Agri.Models.StaticData
{
    public class STPKelownaRange
    {
        public STPKelownaRange()
        {
            STPRecommendations = new List<STPRecommend>();
        }
        public int Id { get; set; }
        public string Range { get; set; }
        public int RangeLow { get; set; }
        public int RangeHigh { get; set; }

        public List<STPRecommend> STPRecommendations { get; set; }
    }
}