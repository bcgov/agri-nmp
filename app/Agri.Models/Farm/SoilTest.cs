using System;

namespace Agri.Models.Farm
{
    public class SoilTest
    {
        public DateTime sampleDate { get; set; }
        public decimal valNO3H { get; set; }
        public decimal ValP { get; set; }
        public decimal valK { get; set; }
        public decimal valPH { get; set; }
        public int ConvertedKelownaP { get; set; }
        public int ConvertedKelownaK { get; set; }
    }
}