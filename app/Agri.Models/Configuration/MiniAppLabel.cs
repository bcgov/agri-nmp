using System;
using System.Collections.Generic;
using System.Text;

namespace Agri.Models.Configuration
{
    public class MiniAppLabel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LabelText { get; set; }
        public int MiniAppId { get; set; }
        public MiniApp MiniApp { get; set; }
    }
}