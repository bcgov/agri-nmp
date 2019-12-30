using Agri.Models;

namespace SERVERAPI.ViewModels
{
    public class NextPrevNavViewModel
    {
        public CoreSiteActions CurrentAction { get; set; }
        public FeaturePages CurrentPage { get; set; }
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
        public FeaturePages PreviousPage { get; set; }
        public FeaturePages NextPage { get; set; }
        public string ViewPreviousUrl { get; set; }
        public string ViewNextUrl { get; set; }
        public bool CurrentIsAPage => CurrentPage != FeaturePages.NotUsed;
        public bool PreviousIsAPage => PreviousPage != FeaturePages.NotUsed;
        public bool NextIsAPage => NextPage != FeaturePages.NotUsed;
    }
}