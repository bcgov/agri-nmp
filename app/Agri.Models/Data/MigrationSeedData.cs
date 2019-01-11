using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Data
{
    public class MigrationSeedData<T> : AppliedMigrationSeedData
    {
        public T Data { get; set; }
    }
}
