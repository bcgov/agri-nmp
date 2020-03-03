using Agri.Models.Farm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Calculate
{
    public class AppliedCollectedManure : AppliedManure
    {
        public AppliedCollectedManure(List<FieldAppliedManure> fieldAppliedManures, FarmAnimal farmAnimal) : base(fieldAppliedManures)
        {
            FarmAnimal = farmAnimal;
        }

        public FarmAnimal FarmAnimal { get; private set; }

        public override string SourceName => FarmAnimal.AnimalSubTypeName;

        public override ManureMaterialType? ManureMaterialType => FarmAnimal.ManureType;

        public override decimal TotalAnnualManureToApply => ManureMaterialType == Models.ManureMaterialType.Liquid ?
            FarmAnimal?.ManureGeneratedTonsPerYear.GetValueOrDefault(0) ?? 0 :
            FarmAnimal?.ManureGeneratedGallonsPerYear.GetValueOrDefault(0) ?? 0;
    }
}