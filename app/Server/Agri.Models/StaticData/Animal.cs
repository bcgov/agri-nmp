using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.StaticData
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AnimalSubType> AnimalSubTypes { get; set; }
    }
}
