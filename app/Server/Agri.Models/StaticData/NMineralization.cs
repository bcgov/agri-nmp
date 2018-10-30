using System.Collections.Generic;

namespace Agri.Models.StaticData
{
    public class NMineralization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Locationid { get; set; }
        public decimal FirstYearValue { get; set; }
        public decimal LongTermValue { get; set; }

        public List<Manure> Manures { get; set; }
    }
}