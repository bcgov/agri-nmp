﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models
{
    public enum AnnualAmountUnits
    {
        //The integer values match the Units in Static Data
        [Description("Imp. gallons")]
        ImperialGallons = 1,

        [Description("m³")]
        CubicMeters = 2,

        [Description("US gallons")]
        USGallons = 3,

        tons = 4,
        tonnes = 5,

        [Description("yards³")]
        CubicYards = 6
    }

    public enum ApplicationRateUnits
    {
        //The integer values match the Units in Static Data
        [Description("Imp. gallons/ac")]
        ImperialGallonsPerAcre = 1,

        [Description("m³/ha")]
        CubicMetersPerHectare = 2,

        [Description("US gallons/ac")]
        USGallonsPerAcre = 3,

        [Description("tons/ac")]
        TonsPerAcre = 4,

        [Description("tonnes/ha")]
        TonnesPerHecatre = 5,

        [Description("yards³/ac")]
        CubicYardsPerAcre = 6
    }

    public enum AppControllers
    {
        Farm,
        Animals,
        ManureManagement,
        RanchManure,
        Fields,
        Soil,
        Feeding,
        Nutrients,
        Report,
        Error
    }

    public enum CoreSiteActions
    {
        Home,
        Farm,
        AddAnimals,
        ManureGeneratedObtained,
        ManureImported,
        ManureStorage,
        ManureNutrientAnalysis,
        RanchManure,
        Fields,
        SoilTest,
        Feeding,
        Calculate,
        Report,
        RefreshNavigation,
        RefreshNextPreviousNavigation,
        SessionExpired
    }

    public enum DairyCattleAnimalSubTypes
    {
        Calves0To3Months = 4,
        Calves3To6Months = 5,
        Heifers6To15Months = 6,
        Heifers15To26Months = 7,
        DryCows = 8
    }

    public enum UserJourney : int
    {
        Initial = 1,
        Dairy = 2,
        Ranch = 3,
        Poultry = 4,
        Crops = 5,
        Mixed = 6
    }

    public enum ManureMaterialType
    {
        Liquid = 1,
        Solid = 2
    }

    public enum NutrientAnalysisTypes
    {
        Stored = 1,
        Imported = 2
    }

    public enum StorageShapes
    {
        [Description("Rectangular")]
        Rectangular = 1,

        [Description("Circular")]
        Circular = 2,

        [Description("Sloped wall (Rectangular)")]
        SlopedWallRectangular = 3
    }

    public enum WashWaterUnits
    {
        [Description("US gallons/day/animal")]
        USGallonsPerDayPerAnimal = 1,

        [Description("US gallons/day")]
        USGallonsPerDay = 2
    }
}