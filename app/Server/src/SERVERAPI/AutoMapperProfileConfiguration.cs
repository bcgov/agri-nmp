using Agri.Models.Farm;
using AutoMapper;
using SERVERAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Models;

namespace SERVERAPI
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
            CreateMap<ManureStorageSystem, ManureStorageSystem>();
            CreateMap<ManureImportedDetailViewModel, ImportedManure>()
                .ForMember(dest => dest.Id, x => x.MapFrom(src => src.ManureImportId))
                .ForMember(dest => dest.ManureType, x => x.MapFrom(src => src.SelectedManureType))
                .ForMember(dest => dest.ManureTypeName, x => x.MapFrom(src => EnumHelper<ManureMaterialType>.GetDisplayValue(src.SelectedManureType)))
                .ForMember(dest => dest.Units, x => x.MapFrom(src => src.SelectedAnnualAmountUnit))
                .ReverseMap();
            CreateMap<ImportedManure, ImportedManure>();

        }
    }
}
