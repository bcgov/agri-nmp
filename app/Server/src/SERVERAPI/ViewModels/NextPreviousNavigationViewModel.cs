using Agri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.ViewModels
{
    public class NextPreviousNavigationViewModel
    {
        public bool UseJSInterceptMethod { get; set; }
        public CoreSiteActions PreviousAction { get; set; }
        public CoreSiteActions NextAction { get; set; }
        public AppControllers PreviousController { get; set; }
        public AppControllers NextController { get; set; }
        public object PreviousParameters { get; set; }
        public object NextParameters { get; set; }
        public string ViewPreviousAction => PreviousAction.ToString();
        public string ViewNextAction => NextAction.ToString();
        public string ViewPreviousController => PreviousController.ToString();
        public string ViewNextController => NextController.ToString();
        public string ViewPreviousUrl { get; set; }
        public string ViewNextUrl { get; set; }
    }
}
