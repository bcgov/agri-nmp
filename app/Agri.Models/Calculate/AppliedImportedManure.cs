using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Models.Farm;

namespace Agri.Models.Calculate
{
    public class AppliedImportedManure : AppliedManure
    {
        public AppliedImportedManure(List<FieldAppliedManure> fieldAppliedManures, ImportedManure importedManure) :
            base(fieldAppliedManures)
        {
            ImportedManure = importedManure;
        }

        public ImportedManure ImportedManure { get; private set; }
        public override ManureMaterialType? ManureMaterialType => ImportedManure?.ManureType;

        public override string SourceName => ImportedManure.ManagedManureName;

        public override decimal TotalAnnualManureToApply => ManureMaterialType == Models.ManureMaterialType.Liquid
            ? ImportedManure?.AnnualAmountUSGallonsVolume ?? 0
            : ImportedManure?.AnnualAmountTonsWeight ?? 0;

        public override string ManureMaterialName { get; set; }
    }
}