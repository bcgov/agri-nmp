using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class ProductRateUnit
    {
        public ProductRateUnit()
        {

        }

        public int Id { get; set; }
        public string Name { get; set; }
        // public decimal ConvFactor { get; set; }

    }
}