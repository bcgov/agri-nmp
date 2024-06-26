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
        NotUsed = 0,
        Farm,
        Animals,
        ManureManagement,
        RanchManure,
        Fields,
        Soil,
        Crops,
        Feeding,
        Nutrients,
        Report,
        Error
    }

    public enum CoreSiteActions
    {
        NotUsed,
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
        Crops,
        Feeding,
        Calculate,
        Report,
        RefreshNavigation,
        RefreshNextPreviousNavigation,
        SessionExpired,
        Error
    }

    public enum DairyCattleAnimalSubTypes
    {
        Calves0To3Months = 4,
        Calves3To6Months = 5,
        Heifers6To15Months = 6,
        Heifers15To26Months = 7,
        DryCows = 8
    }

    public enum ManureMaterialType
    {
        Liquid = 1,
        Solid = 2
    }

    public enum NutrientAnalysisTypes
    {
        Stored = 1,
        Imported = 2,
        Collected = 3
    }

    public enum FeaturePages
    {
        [Description("Pages Not Used")]
        NotUsed = 0,

        [Description("/Ranch/RanchAnimals/Index")]
        RanchAnimalsIndex,

        [Description("/Ranch/RanchManure/Index")]
        RanchManureIndex,

        [Description("/Ranch/RanchNutrients/Index")]
        RanchNutrientsIndex,

        [Description("/Ranch/RanchFields/Index")]
        RanchFieldsIndex,

        [Description("/Ranch/RanchFeeding/Index")]
        RanchFeedingIndex,

        [Description("/Poultry/PoultryAnimals/Index")]
        PoultryAnimalsIndex,

        [Description("/Poultry/PoultryManure/Index")]
        PoultryManureIndex,

        [Description("/Poultry/PoultryNutrients/Index")]
        PoultryNutrientsIndex,

        [Description("/Mixed/MixedAnimals/Index")]
        MixedAnimalsIndex,

        [Description("/Mixed/MixedManure/Index")]
        MixedManureIndex,

        [Description("/Mixed/MixedNutrients/Index")]
        MixedNutrientsIndex,

        [Description("/Mixed/MixedFields/Index")]
        MixedFieldsIndex,
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

    public enum UserJourney : int
    {
        Initial = 1,
        Dairy = 2,
        Ranch = 3,
        Poultry = 4,
        Crops = 5,
        Mixed = 6,
        Berries = 7
    }

    public enum UserPromptPage
    {
        Home,
        Farm,
        AnimalsList,
        AnimalsCreateEdit,
        AnimalsDelete,
        ManureGeneratedObtained,
        ManureGeneratedObtainedModal,
        ManureGeneratedObtainedDelete,
        ManureImported,
        ManureImportedModal,
        ManureImportedDelete,
        ManureStorage,
        ManureStorageModal,
        ManureStorageDelete,
        ManureNutrientAnalysis,
        ManureNutrientAnalysisModal,
        ManureNutrientDelete,
        ManureList,
        ManureCreateEdit,
        ManureDelete,
        NutrientsAnalysisList,
        NutrientsAnalysisCreateEdit,
        NutrientsAnalysisDelete,
        Fields,
        FieldsModal,
        FieldsDelete,
        SoilTest,
        SoilTestModal,
        SoilTestDelete,
        Feeding,
        FieldsList,
        FieldCreateEdit,
        SoilTestList,
        SoilCreateEdit,
        FeedingList,
        FeedingCreateEdit,
        FeedingDelete,
        Calculate,
        Report
    }

    public enum WashWaterUnits
    {
        [Description("US gallons/day/animal")]
        USGallonsPerDayPerAnimal = 1,

        [Description("US gallons/day")]
        USGallonsPerDay = 2
    }
}