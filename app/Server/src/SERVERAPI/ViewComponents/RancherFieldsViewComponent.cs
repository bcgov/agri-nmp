using Agri.Data;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class RancherFields : ViewComponent
    {
        private readonly UserData _ud;
        private readonly IAgriConfigurationRepository _sd;

        public RancherFields(UserData ud, IAgriConfigurationRepository sd)
        {
            _ud = ud;
            _sd = sd;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetRancherFieldsAsync());
        }

        private Task<RancherFieldsViewModel> GetRancherFieldsAsync()
        {
            RancherFieldsViewModel fvm = new RancherFieldsViewModel();
            fvm.Placehldr = _sd.GetUserPrompt("fieldcommentplaceholder");
            FarmDetails fd = _ud.FarmDetails();

            if (fd.FarmRegion.HasValue)
            {
                fvm.RegionFnd = true;
            }
            else
            {
                fvm.RegionFnd = false;
                fvm.NoRegion = _sd.GetUserPrompt("missingregion");
            }

            fvm.Fields = new List<RancherField>();

            List<RancherField> fldList = _ud.GetRancherFields();

            foreach (var f in fldList)
            {
                RancherField nf = new RancherField();
                nf.FieldName = f.FieldName;
                nf.Area = Convert.ToDecimal((f.Area).ToString("G29"));
                nf.Comment = f.Comment;
                nf.SeasonalFeedingArea = f.SeasonalFeedingArea;
                fvm.Fields.Add(nf);
            }

            if (fvm.Fields.Count == 0)
            {
                fvm.Actn = null;
                fvm.Cntl = "RancherFields";
                fvm.Act = "Add";
                fvm.Target = "#rancherFields";
                fvm.SelectPrevYrManureOptions = _sd.GetPrevManureApplicationInPrevYears();
            }

            return Task.FromResult(fvm);
        }
    }

    public class RancherFieldsViewModel
    {
        [Display(Name = "Field Name")]
        [Required]
        public string FieldName { get; set; }

        [Display(Name = "Area")]
        [Required]
        public string FieldArea { get; set; }

        [Display(Name = "Comments (optional)")]
        public string FieldComment { get; set; }

        public List<PreviousManureApplicationYear> SelectPrevYrManureOptions { get; set; }

        [Display(Name = "Manure application in previous years")]
        public string SelectPrevYrManureOption { get; set; }

        public bool IsSeasonalFeedingArea { get; set; }
        public string SeasonalFeedingArea { get; set; }

        public string Act { get; set; }
        public string UserDataField { get; set; }
        public string CurrFieldName { get; set; }
        public string Target { get; set; }
        public string Cntl { get; set; }
        public string Actn { get; set; }
        public string CurrFld { get; set; }
        public int FieldId { get; set; }
        public string Placehldr { get; set; }
        public bool RegionFnd { get; set; }
        public string NoRegion { get; set; }
        public List<RancherField> Fields { get; set; }
    }
}