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
            FarmDetails fd = _ud.FarmDetails();

            if (fd.FarmRegion.HasValue)
            {
                fvm.regionFnd = true;
            }
            else
            {
                fvm.regionFnd = false;
                fvm.noRegion = _sd.GetUserPrompt("missingregion");
            }

            fvm.fields = new List<RancherField>();

            List<RancherField> fldList = _ud.GetRancherFields();

            foreach (var f in fldList)
            {
                RancherField nf = new RancherField();
                nf.FieldName = f.FieldName;
                nf.Area = Convert.ToDecimal((f.Area).ToString("G29"));
                nf.Comment = f.Comment;
                nf.SeasonalFeedingArea = f.SeasonalFeedingArea;
                fvm.fields.Add(nf);
            }

            if (fvm.fields.Count == 0)
            {
                fvm.actn = null;
                fvm.cntl = "RancherFields";
                fvm.act = "Add";
                fvm.target = "#rancherFields";
                fvm.selPrevYrManureOptions = _sd.GetPrevManureApplicationInPrevYears();
            }

            return Task.FromResult(fvm);
        }
    }

    public class RancherFieldsViewModel
    {
        [Display(Name = "Field Name")]
        [Required]
        public string fieldName { get; set; }

        [Display(Name = "Area")]
        [Required]
        public string fieldArea { get; set; }

        [Display(Name = "Comments (optional)")]
        public string fieldComment { get; set; }

        public List<PreviousManureApplicationYear> selPrevYrManureOptions { get; set; }

        [Display(Name = "Manure application in previous years")]
        public string selPrevYrManureOption { get; set; }

        public bool isSeasonalFeedingArea { get; set; }
        public string seasonalFeedingArea { get; set; }

        public string act { get; set; }
        public string userDataField { get; set; }
        public string currFieldName { get; set; }
        public string target { get; set; }
        public string cntl { get; set; }
        public string actn { get; set; }
        public string currFld { get; set; }
        public int fieldId { get; set; }
        public string placehldr { get; set; }
        public bool regionFnd { get; set; }
        public string noRegion { get; set; }
        public List<RancherField> fields { get; set; }
    }
}