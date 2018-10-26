namespace Agri.Models
{
    public class Crop
    {
        public int id { get; set; }
        public string cropname { get; set; }
        public int croptypeid { get; set; }
        public int yieldcd { get; set; }
        public decimal? cropremovalfactor_N { get; set; }
        public decimal? cropremovalfactor_P2O5 { get; set; }
        public decimal? cropremovalfactor_K2O { get; set; }
        public decimal n_recommcd { get; set; }
        public decimal? n_recomm_lbperac { get; set; }
        public decimal? n_high_lbperac { get; set; }
        public int prevcropcd { get; set; }
        public int sortNum { get; set; }
        public int prevYearManureAppl_volCatCd { get; set; }
        public decimal? harvestBushelsPerTon { get; set; }
    }
}