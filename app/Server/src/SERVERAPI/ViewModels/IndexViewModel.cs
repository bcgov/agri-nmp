namespace SERVERAPI.ViewModels
{
    public class IndexViewModel
    {
        public bool unsavedData { get; set; }
        public string userData { get; set; }
        public string welcomeMsg { get; set; }
        public string disclaimerMsg { get; set; }
        public string staticDataVersionMsg { get; set; }
        public string newMsg { get; set; }
        public string loadMsg { get; set; }
        public string pageMsg1 { get; set; }
        public string pageMsg2 { get; set; }
        public string browserMsg { get; set; }
        public string browserAgent { get; set; }
        public bool IsFileUploaded { get; set; }
        public string ButtonPressed { get; set; }
        public string ExplainFileLoad { get; set; }
        public string fileLoadLabelText { get; set; }
    }
}