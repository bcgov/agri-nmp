using Agri.CalculateService;
using Agri.Data;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class CalcFeedAreaViewComponent : ViewComponent
    {
        private readonly IAgriConfigurationRepository _sd;
        private readonly UserData _ud;
        private readonly IFeedAreaCalculator _feedCalculator;

        public CalcFeedAreaViewComponent(IAgriConfigurationRepository sd, UserData ud,
            IFeedAreaCalculator feedCalculator)
        {
            _sd = sd;
            _ud = ud;
            _feedCalculator = feedCalculator;
        }

        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetFeedAreaAsync(fldName));
        }

        private async Task<CalcFeedAreaModel> GetFeedAreaAsync(string fieldName)
        {
            //var analytic = _ud.GetFeedForageAnalysis(fieldName);
            var field = _ud.GetFieldDetails(fieldName);
            var region = _sd.GetRegion(_ud.FarmDetails().FarmRegion.Value);

            CalcFeedAreaModel model = null;
            if (field.FeedForageAnalyses != null && field.FeedForageAnalyses.Any())
            {
                model = new CalcFeedAreaModel
                {
                    FieldId = field.Id,
                    FieldName = fieldName,
                    NAgroBalance = _feedCalculator.GetNitrogenAgronomicBalance(field, region),
                    P205AgroBalance = _feedCalculator.GetP205AgronomicBalance(field),
                    K20AgroBalance = _feedCalculator.GetK20AgronomicBalance(field),
                    NCropRemovalValue = _feedCalculator.GetNitrogenCropRemovalValue(field, region),
                    P205CropRemovalValue = _feedCalculator.GetP205CropRemovalValue(field),
                    K20CropRemovalValue = _feedCalculator.GetK20CropRemovalValue(field)
                };
            }

            return await Task.FromResult(model);
        }
    }

    public class CalcFeedAreaModel
    {
        public int FieldId { get; set; }
        public string FieldName { get; set; }
        public decimal NAgroBalance { get; set; }
        public decimal P205AgroBalance { get; set; }
        public decimal K20AgroBalance { get; set; }
        public decimal NCropRemovalValue { get; set; }
        public decimal P205CropRemovalValue { get; set; }
        public decimal K20CropRemovalValue { get; set; }
    }
}