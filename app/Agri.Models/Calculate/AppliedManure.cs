using Agri.Models.Farm;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Agri.Models.Calculate
{
    public abstract class AppliedManure
    {
        public AppliedManure(List<FieldAppliedManure> fieldAppliedManures)
        {
            FieldAppliedManures = fieldAppliedManures;
        }

        public List<FieldAppliedManure> FieldAppliedManures { get; private set; }

        public abstract string SourceName { get; }

        public abstract ManureMaterialType? ManureMaterialType { get; }

        public ApplicationRateUnits ApplicationRateUnit => ManureMaterialType == Models.ManureMaterialType.Liquid
            ? ApplicationRateUnits.ImperialGallonsPerAcre
            : ApplicationRateUnits.TonsPerAcre;

        public abstract decimal TotalAnnualManureToApply { get; }

        public decimal TotalApplied
        {
            get
            {
                if (ManureMaterialType == Models.ManureMaterialType.Liquid)
                {
                    var totalLiquidApplied = FieldAppliedManures
                                                            .Where(f => f.USGallonsApplied.HasValue)
                                                            .Sum(f => f.USGallonsApplied.Value);
                    return totalLiquidApplied;
                }
                var totalSolidApplied = FieldAppliedManures
                                                    .Where(f => f.TonsApplied.HasValue)
                                                    .Sum(f => f.TonsApplied.Value);
                return totalSolidApplied;
            }
        }

        public decimal TotalAnnualManureRemainingToApply => TotalAnnualManureToApply - TotalApplied;
        public decimal WholePercentAppiled => TotalApplied / TotalAnnualManureToApply * 100;
        public decimal WholePercentRemaining => TotalAnnualManureRemainingToApply / TotalAnnualManureToApply * 100;
        public string AppliedMessage => $"{SourceName}: {WholePercentAppiled}%";
        public string RemainingToApplMessage => $"{SourceName}: {WholePercentRemaining}%";
    }
}
