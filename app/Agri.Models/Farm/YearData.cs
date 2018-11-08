using System.Collections.Generic;

namespace Agri.Models.Farm
{
    public class YearData
    {
        public string year { get; set; }
        public List<Field> fields { get; set; }
        public List<FarmManure> farmManures { get; set; }
        public List<GeneratedManure> GeneratedManures { get; set; }
    }
}