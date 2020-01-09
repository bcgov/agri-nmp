using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Agri.Models.Configuration
{
    public class MiniApp
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public List<MiniAppLabel> MiniAppLabels { get; set; }
    }
}