using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Farm
{
    public class FeedForageAnalysis
    {
        public int Id { get; set; }
        public int FeedForageTypeId { get; set; }
        public int FeedForageId { get; set; }
        public bool UseBookValues { get; set; }
        public decimal CrudeProteinPercent { get; set; }
        public decimal Phosphorus { get; set; }
        public decimal Potassium { get; set; }
        public decimal PercentOfTotalFeedForageToAnimals { get; set; }
        public decimal PercentOfFeedForageWastage { get; set; }
    }
}