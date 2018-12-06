using Agri.Models;

namespace Agri.Interfaces
{
    public interface IManureUnitConversionCalculator
    {
        decimal GetCubicMetersVolume(ManureMaterialType manureMaterialType, decimal amountToConvert, AnnualAmountUnits amountUnit);
        decimal GetCubicYardsVolume(ManureMaterialType manureMaterialType, decimal amountToConvert, AnnualAmountUnits amountUnit);
        decimal GetUSGallonsVolume(ManureMaterialType manureMaterialType, decimal amountToConvert, AnnualAmountUnits amountUnit);
        decimal GetTonsWeight(ManureMaterialType manureMaterialType, decimal amountToConvert, AnnualAmountUnits amountUnit);
    }
}