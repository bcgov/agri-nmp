namespace Agri.Models
{
    public class Message
    {
        public int id { get; set; }
        public string text { get; set; }
        public string displayMessage { get; set; }
        public string icon { get; set; }
        public string balanceType { get; set; }
        public int balance_low { get; set; }
        public int balance_high { get; set; }
        public decimal soiltest_low { get; set; }
        public decimal soiltest_high { get; set; }
        public int balance1_low { get; set; }
        public int balance1_high { get; set; }
    }
}