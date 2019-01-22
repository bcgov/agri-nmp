using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Configuration
{
    public class Breed
    {
        public int Id { get; set; }
        public string BreedName { get; set; }
        public int AnimalId { get; set; }
        public decimal BreedManureFactor { get; set; }
        public Animal Animal { get; set; }
    }
}
