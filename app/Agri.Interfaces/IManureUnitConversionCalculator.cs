using Agri.Models;
using Agri.Models.Configuration;

namespace Agri.Interfaces
{
    public interface IManureUnitConversionCalculator
    {
        decimal GetCubicMetersVolume(ManureMaterialType manureMaterialType, decimal moistureWholePercent, decimal amountToConvert, AnnualAmountUnits amountUnit);
        decimal GetCubicYardsVolume(ManureMaterialType manureMaterialType, decimal moistureWholePercent, decimal amountToConvert, AnnualAmountUnits amountUnit);
        decimal GetUSGallonsVolume(ManureMaterialType manureMaterialType, decimal amountToConvert, AnnualAmountUnits amountUnit);
        decimal GetTonsWeight(ManureMaterialType manureMaterialType, decimal moistureWholePercent, decimal amountToConvert, AnnualAmountUnits amountUnit);
        decimal GetSolidsTonsPerAcreApplicationRate(decimal moistureWholePercent, decimal amountToConvert, ApplicationRateUnits applicationRateUnit);
        decimal GetSolidsTonsPerAcreApplicationRate(int manureId, decimal amountToConvert, ApplicationRateUnits applicationRateUnit);
    }
}