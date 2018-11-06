using System.Collections.Generic;

namespace SERVERAPI.ViewModels
{
    public class DownLoadViewModel
    {
        public List<string> images { get; set; }
        public string browserAgent { get; set; }
        public string browserName { get; set; }
        public string os { get; set; }
    }
}