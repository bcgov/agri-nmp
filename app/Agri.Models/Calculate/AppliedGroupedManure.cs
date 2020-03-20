using Agri.Models.Farm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Calculate
{
    public class AppliedGroupedManure : AppliedManure
    {
        private readonly string _sourceName;

        public AppliedGroupedManure(List<FieldAppliedManure> fieldAppliedManures,
            List<ManagedManure> includedAppliedManures,
            string sourceName) : base(fieldAppliedManures)
        {
            IncludedManagedManures = includedAppliedManures ?? new List<ManagedManure>();
            _sourceName = sourceName;
        }

        public List<ManagedManure> IncludedManagedManures { get; private set; }

        public override string SourceName => _sourceName;

        public override ManureMaterialType? ManureMaterialType =>
            IncludedManagedManures.FirstOrDefault()?.ManureType;

        public override decimal TotalAnnualManureToApply
        {
            get
            {
                var total = 0m;

                foreach (var manure in IncludedManagedManures)
                {
                    if (manure is FarmAnimal)
                    {
                        var farmAnimal = manure as FarmAnimal;
                        total += farmAnimal.ManureType == Models.ManureMaterialType.Liquid ?
                                farmAnimal?.ManureGeneratedGallonsPerYear.GetValueOrDefault(0) ?? 0 :
                                farmAnimal?.ManureGeneratedTonsPerYear.GetValueOrDefault(0) ?? 0;
                    }
                    else if (manure is ImportedManure)
                    {
                        var importedManure = manure as ImportedManure;
                        total += importedManure.ManureType == Models.ManureMaterialType.Liquid ?
                                importedManure?.AnnualAmountUSGallonsVolume ?? 0 :
                                importedManure?.AnnualAmountTonsWeight ?? 0;
                    }
                }

                return total;
            }
        }

        public override string ManureMaterialName { get; set; }
    }
}