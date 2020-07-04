using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Agri.Models.Farm
{
    public class ManureStorageSystem
    {
        public ManureStorageSystem()
        {
            ManureStorageStructures = new List<ManureStorageStructure>();
            GeneratedManuresIncludedInSystem = new List<GeneratedManure>();
            ImportedManuresIncludedInSystem = new List<ImportedManure>();
            SeparatedSolidManuresIncludedInSystem = new List<SeparatedSolidManure>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ManureMaterialType ManureMaterialType { get; set; }
        public List<GeneratedManure> GeneratedManuresIncludedInSystem { get; set; }
        public List<ImportedManure> ImportedManuresIncludedInSystem { get; set; }
        public List<SeparatedSolidManure> SeparatedSolidManuresIncludedInSystem { get; set; }

        [JsonIgnore]
        public List<ManagedManure> MaterialsIncludedInSystem
        {
            get
            {
                var manures = new List<ManagedManure>();
                if (GeneratedManuresIncludedInSystem.Any())
                {
                    manures.AddRange(GeneratedManuresIncludedInSystem);
                }

                if (ImportedManuresIncludedInSystem.Any())
                {
                    manures.AddRange(ImportedManuresIncludedInSystem);
                }

                if (SeparatedSolidManuresIncludedInSystem.Any())
                {
                    manures.AddRange(SeparatedSolidManuresIncludedInSystem);
                }

                return manures.ToList();
            }
        }

        public bool GetsRunoffFromRoofsOrYards { get; set; }
        public int? RunoffAreaSquareFeet { get; set; }
        public bool IsThereSolidLiquidSeparation { get; set; }
        public int PercentageOfLiquidVolumeSeparated { get; set; }
        public decimal SeparatedLiquidsUSGallons { get; set; }
        public decimal SeparatedSolidsTons { get; set; }
        public List<ManureStorageStructure> ManureStorageStructures { get; }
        public int? AnnualPrecipitation { get; set; }
        public bool AssignedWithNutrientAnalysis { get; set; }
        public string ManureStorageVolume { get; set; }
        public decimal OctoberToMarchSeparatedLiquidsUSGallons { get; set; }

        public double OctoberToMarchPrecipitation =>
            ManureStorageStructures
                .Where(mss => mss.IsStructureCovered == false)
                .Sum(ss => ss.OctoberToMarchPrecipitation);

        public string OctoberToMarchPrecipitationText => OctoberToMarchPrecipitation.ToString("#,#");
        public double OctoberToMarchRunoff { get; set; }
        public string OctoberToMarchRunoffText => OctoberToMarchRunoff.ToString("#,#");
        public decimal OctoberToMarchManagedManures { get; set; }
        public string OctoberToMarchManagedManuresText => OctoberToMarchManagedManures.ToString("#,#");
        public decimal TotalStored { get; set; }
        public string TotalStoredText => TotalStored.ToString("#,#");

        [JsonIgnore]
        public List<ManureStorageItemSummary> ManureStorageItemSummaries
        {
            get
            {
                var summaries = new List<ManureStorageItemSummary>();
                foreach (var generatedManure in GeneratedManuresIncludedInSystem)
                {
                    var annualAmount = Convert.ToDecimal(generatedManure.AnnualAmount.Split(' ')[0]);
                    if (@ManureMaterialType != @generatedManure.ManureType && @generatedManure.ManureType == ManureMaterialType.Solid)
                    {
                        if (generatedManure.SolidPerGalPerAnimalPerDay.HasValue)
                        {
                            // if solid material is added to the liquid system change the calculations to depict that of liquid
                            annualAmount = Math.Round(Convert.ToInt32(generatedManure.AverageAnimalNumber) *
                                                      generatedManure.SolidPerGalPerAnimalPerDay.Value) * 365;
                        }
                    }

                    annualAmount += Convert.ToInt32(generatedManure.WashWaterGallons);
                    var summary = new ManureStorageItemSummary(generatedManure, annualAmount, AnnualAmountUnit);
                    summaries.Add(summary);
                }

                foreach (var separatedSolidManure in SeparatedSolidManuresIncludedInSystem)
                {
                    var summary = new ManureStorageItemSummary(separatedSolidManure,
                        separatedSolidManure.AnnualAmountTonsWeight, AnnualAmountUnits.tons);
                    summaries.Add(summary);
                }

                foreach (var importedManure in ImportedManuresIncludedInSystem)
                {
                    decimal totalImportedManure;
                    if (importedManure.ManureType == ManureMaterialType.Liquid)
                    {
                        totalImportedManure = ImportedManuresIncludedInSystem.Sum(im => im.AnnualAmountUSGallonsVolume);
                    }
                    else
                    {
                        totalImportedManure = ImportedManuresIncludedInSystem.Sum(im => im.AnnualAmountTonsWeight);
                    }
                    summaries.Add(new ManureStorageItemSummary(importedManure, totalImportedManure, AnnualAmountUnit));
                }

                return summaries;
            }
        }

        [JsonIgnore]
        public int TotalAreaOfUncoveredLiquidStorage => ManureStorageStructures
                                                                                    .Where(ss => !ss.IsStructureCovered)
                                                                                    .Sum(ss => ss.UncoveredAreaSquareFeet ?? 0);

        [JsonIgnore]
        public decimal AnnualTotalPrecipitation
        {
            get
            {
                decimal conversionForLiquid = 0.024542388m;
                decimal conversionForSolid = 0.000102408m;
                var precipitation = 0m;
                if (ManureMaterialType == ManureMaterialType.Liquid)
                {
                    precipitation =
                        (Convert.ToDecimal(RunoffAreaSquareFeet) +
                            Convert.ToDecimal(TotalAreaOfUncoveredLiquidStorage))
                            * Convert.ToDecimal(AnnualPrecipitation) * conversionForLiquid;
                }
                else if (ManureMaterialType == ManureMaterialType.Solid)
                {
                    precipitation =
                        (Convert.ToDecimal(RunoffAreaSquareFeet) + Convert.ToDecimal(TotalAreaOfUncoveredLiquidStorage))
                                   * Convert.ToDecimal(AnnualPrecipitation) * conversionForSolid;
                }

                return precipitation;
            }
        }

        [JsonIgnore]
        public AnnualAmountUnits AnnualAmountUnit => ManureMaterialType == ManureMaterialType.Liquid
            ? AnnualAmountUnits.USGallons
            : AnnualAmountUnits.tons;

        [JsonIgnore]
        public decimal AnnualTotalStoredGeneratedManure
        {
            get
            {
                var totalAnnualGeneratedManure = ManureStorageItemSummaries
                    .Where(ms => ms.ManagedManure is GeneratedManure || ms.ManagedManure is SeparatedSolidManure)
                    .Sum(ms => ms.ItemTotalAnnualStored);

                return totalAnnualGeneratedManure;
            }
        }

        [JsonIgnore]
        public decimal AnnualTotalImportedManure
        {
            get
            {
                var totalImportedManure = ManureStorageItemSummaries
                    .Where(ms => ms.ManagedManure is ImportedManure)
                    .Sum(ms => ms.ItemTotalAnnualStored);

                return totalImportedManure;
            }
        }

        [JsonIgnore]
        public decimal AnnualTotalSeparatedSolidManure
        {
            get
            {
                var totalSeparatedSolid = ManureStorageItemSummaries
                    .Where(ms => ms.ManagedManure is SeparatedSolidManure)
                    .Sum(ms => ms.ItemTotalAnnualStored);

                return totalSeparatedSolid;
            }
        }

        [JsonIgnore]
        public decimal AnnualTotalAmountofManureInStorage
        {
            get
            {
                var total = 0m;
                if (IsThereSolidLiquidSeparation)
                {
                    if (SeparatedLiquidsUSGallons > 0)
                    {
                        return SeparatedLiquidsUSGallons + AnnualTotalPrecipitation;
                    }
                    return 0;
                }
                else
                {
                    total = AnnualTotalStoredGeneratedManure + AnnualTotalImportedManure;
                    if (total > 0)
                    {
                        return total + AnnualTotalPrecipitation;
                    }
                    return 0;
                }
            }
        }

        #region Methods

        public void AddUpdateManureStorageStructure(ManureStorageStructure manureStorageStructure)
        {
            var savedStructure = ManureStorageStructures.SingleOrDefault(mss => mss.Id == manureStorageStructure.Id);

            if (savedStructure == null)
            {
                manureStorageStructure.Id = ManureStorageStructures.Any()
                    ? ManureStorageStructures.Select(mss => mss.Id).Max() + 1
                    : 0;

                ManureStorageStructures.Add(manureStorageStructure);
            }
            else
            {
                savedStructure.Name = manureStorageStructure.Name;
                savedStructure.UncoveredAreaSquareFeet = manureStorageStructure.UncoveredAreaSquareFeet;
            }
        }

        public void UpdateManureStorageStructure(ManureStorageStructure manureStorageStructure)
        {
            if (manureStorageStructure.Id > 0)
            {
                var savedStructure = ManureStorageStructures.Single(mss => mss.Id == manureStorageStructure.Id);
                savedStructure.Name = manureStorageStructure.Name;
                savedStructure.UncoveredAreaSquareFeet = manureStorageStructure.UncoveredAreaSquareFeet;
            }
        }

        public ManureStorageStructure GetManureStorageStructure(int id)
        {
            return ManureStorageStructures.Single(mss => mss.Id == id);
        }

        public void AddIncludedMaterial(ManagedManure manureToRemove)
        {
            if (manureToRemove is GeneratedManure)
            {
                GeneratedManuresIncludedInSystem.Add(manureToRemove as GeneratedManure);
            }
            else if (manureToRemove is ImportedManure)
            {
                ImportedManuresIncludedInSystem.Add(manureToRemove as ImportedManure);
            }
            else if (manureToRemove is SeparatedSolidManure)
            {
                SeparatedSolidManuresIncludedInSystem.Add(manureToRemove as SeparatedSolidManure);
            }
        }

        public void RemoveIncludedMaterial(ManagedManure manureToRemove)
        {
            if (manureToRemove is GeneratedManure)
            {
                GeneratedManuresIncludedInSystem.Remove(manureToRemove as GeneratedManure);
            }
            else if (manureToRemove is ImportedManure)
            {
                ImportedManuresIncludedInSystem.Remove(manureToRemove as ImportedManure);
            }
            else if (manureToRemove is SeparatedSolidManure)
            {
                SeparatedSolidManuresIncludedInSystem.Remove(manureToRemove as SeparatedSolidManure);
            }
        }

        #endregion Methods
    }
}