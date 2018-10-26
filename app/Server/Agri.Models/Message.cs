namespace Agri.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string DisplayMessage { get; set; }
        public string Icon { get; set; }
        public string BalanceType { get; set; }
        public int BalanceLow { get; set; }
        public int BalanceHigh { get; set; }
        public decimal SoilTestLow { get; set; }
        public decimal SoilTestHigh { get; set; }
        public int Balance1Low { get; set; }
        public int Balance1High { get; set; }
    }
}