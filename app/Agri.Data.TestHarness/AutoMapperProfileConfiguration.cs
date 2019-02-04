using System.Collections.Generic;
using Agri.Data.Migrations;
using Agri.Models;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using AutoMapper;
using Version = Agri.Models.Configuration.Version;


namespace Agri.Data.TestHarness
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
            : this("MyProfile")
        {
        }
        protected AutoMapperProfileConfiguration(string profileName)
            : base(profileName)
        {
            CreateMap<AmmoniaRetention, AmmoniaRetention>();
            CreateMap<ManureStorageSystem, ManureStorageSystem>();
            CreateMap<ImportedManure, ImportedManure>();
            CreateMap<UserPrompt, UserPrompt>();
            CreateMap<AnimalSubType, AnimalSubType>();
            CreateMap<SeparatedSolidManure, SeparatedSolidManure>();
            CreateMap<SubRegions, SubRegions>();
            CreateMap<PreviousYearManureApplicationNitrogenDefault, PreviousYearManureApplicationNitrogenDefault>();

            CreateMap<Version, Version>();
        }
    }
}
