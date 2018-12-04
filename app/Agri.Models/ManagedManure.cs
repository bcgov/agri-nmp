using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models
{
    public abstract class ManagedManure
    {
        public int? Id { get; set; }
        public ManureMaterialType ManureType { get; set; }
        public bool AssignedToStoredSystem { get; set; }
        public abstract string ManureId { get; }
    }
}
