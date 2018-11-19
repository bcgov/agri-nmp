using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SERVERAPI.Models.Impl;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.Models;
using static SERVERAPI.Models.StaticData;
using System.Data;


namespace SERVERAPI.Utility
{
    public class CalculateAnimalRequirement
    {
        private UserData _ud;
        private Models.Impl.StaticData _sd;

        public CalculateAnimalRequirement(UserData ud, Models.Impl.StaticData sd)
        {
            _ud = ud;
            _sd = sd;
        }
        public decimal? washWater { get; set; }
        public decimal? milkProduction { get; set; }

        // default for Wash Water
        public decimal? GetWashWaterBySubTypeId(int _subTypeId)
        {
            decimal? defaultWashWater = null;
            defaultWashWater = _sd.GetIncludeWashWater(_subTypeId);

            return defaultWashWater;
        }

        // default for Milk Production
        public decimal? GetDefaultMilkProductionBySubTypeId(int _subTypeId)
        {
            decimal? defaultMilkProduction = null;
            defaultMilkProduction = _sd.GetMilkProduction(_subTypeId);

            return defaultMilkProduction;
        }

    }

}