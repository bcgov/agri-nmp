﻿using Agri.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agri.CalculateService
{
    public interface INitrateTestCalculator
    {
        double CalculateResult(string selectedDepth, double nitrateValue, double bulkDensity);
    }
    public class CalculateNitrateTestResult:INitrateTestCalculator
    {
        private readonly IAgriConfigurationRepository _sd;

        public CalculateNitrateTestResult(IAgriConfigurationRepository sd)
        {
            _sd = sd;
        }
        public double CalculateResult(string selectedDepth,double nitrateValue, double bulkDensity)
        {
            double returnValue = 0;
            var depths = _sd.GetDepths();
            var selectedDepthName = depths.Find(x => x.Id == Convert.ToInt32(selectedDepth)).Name;
            if (selectedDepthName == "DepthZeroToThirty") 
            {
                returnValue = (nitrateValue * bulkDensity * 3000) / (Math.Pow(10, 6));
                returnValue = Math.Round(returnValue);
            }
            else
            {
                returnValue = (nitrateValue * bulkDensity * 1500) / (Math.Pow(10, 6));
                returnValue = Math.Round(returnValue);
            }

            return returnValue;
        }
    }
}
