using Agri.Data;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewComponents
{
    public class CalcFertigationViewComponent : ViewComponent
    {
        private IAgriConfigurationRepository _sd;
        private Models.Impl.UserData _ud;
        private Fertigation _fd;

        public CalcFertigationViewComponent(IAgriConfigurationRepository sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
            _fd = GetFertigationData();
        }

        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetFertigationAsync(fldName));
        }

        private Task<CalcFertigationViewModel> GetFertigationAsync(string fldName)
        {
            CalcFertigationViewModel fgvm = new CalcFertigationViewModel();
            fgvm.fldFertilizers = new List<DisplayNutrientFertigation>();

            List<NutrientFertilizer> fldFertilizers = _ud.GetFieldNutrientsFertilizers(fldName);
            foreach (var f in fldFertilizers.Where(f => f.isFertigation))
            {
                DisplayNutrientFertigation dm = new DisplayNutrientFertigation();

                FertilizerType ft = _sd.GetFertilizerType(f.fertilizerTypeId.ToString());

                string fertilizerName;
                if (ft.Custom)
                {
                    fertilizerName = ft.DryLiquid == "dry" ? "Custom (Dry) " : "Custom (Liquid) ";
                    fertilizerName = fertilizerName + f.customN.ToString() + "-" + f.customP2o5.ToString() + "-" + f.customK2o.ToString();
                }
                else
                {
                    Fertilizer ff = GetFertigationFertilizer(f.fertilizerId);
                    fertilizerName = ff.Name;
                }

                dm.fldName = fldName;
                dm.fertilizerId = f.id;
                dm.fertilizerName = fertilizerName;

                // int startIndex = fertilizerName.IndexOf('(');
                // int endIndex = fertilizerName.IndexOf(')');
                // if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                // {
                //     string result = fertilizerName.Substring(startIndex +1, endIndex - startIndex -1);
                //     dm.fertilizerName = result;
                // }
                dm.valN = f.fertN.ToString("G29");
                dm.valP = f.fertP2o5.ToString("G29");
                dm.valK = f.fertK2o.ToString("G29");
                dm.isFertigation = true;
                dm.eventsPerSeason = f.eventsPerSeason;
                dm.date = f.applDate;
                dm.dateAsString = f.applDate?.ToString("dd-MMM");
                dm.fertilizerTypeId = f.fertilizerTypeId;
                dm.groupID = f.groupID;
                fgvm.fldFertilizers.Add(dm);
            }

            return Task.FromResult(fgvm);
        }
        public Fertigation GetFertigationData()
        {
            var filePath = "../../../Agri.Data/SeedData/FertigationData.json";
            var jsonData = System.IO.File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Fertigation>(jsonData);
        }
        private Fertilizer GetFertigationFertilizer(int id)
        {
            List<Fertilizer> fertilizers = _fd.Fertilizers;
            return fertilizers.Find(x => x.Id == id);
        }
    }
    public class CalcFertigationViewModel
{
    public List<DisplayNutrientFertigation> fldFertilizers { get; set; }
}

public class DisplayNutrientFertigation
{
    public string fldName { get; set; }
    public int fertilizerId { get; set; }
    public string fertilizerName { get; set; }
    public string valN { get; set; }
    public string valP { get; set; }
    public string valK { get; set; }
    public bool isFertigation { get; set; }
    public int eventsPerSeason { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? date { get; set; }
    public string dateAsString { get; set; }
    public int fertilizerTypeId { get; set; }
    public string groupID { get; set; }
}
}
