using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Data
{
    public class AppliedMigrationSeedData
    {
        public int Id { get; set; }
        public string JsonFilename { get; set; }
        public string ReasonReference { get; set; }
        public DateTime AppliedDateTime { get; set; }
    }
}
