namespace Agri.Models.Configuration
{
    public class SoilTestRange : ConfigurationBase
    {
        public int LowerLimit { get; set; }
        public int UpperLimit { get; set; }
        public string Rating { get; set; }
    }
}