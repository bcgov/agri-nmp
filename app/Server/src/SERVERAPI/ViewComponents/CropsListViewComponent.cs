using Agri.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class CropsList : ViewComponent
    {
        private readonly IAgriConfigurationRepository _sd;
        private readonly UserData _ud;
        private readonly IMapper _mapper;

        public CropsList(IAgriConfigurationRepository sd, UserData ud, IMapper mapper)
        {
            _sd = sd;
            _ud = ud;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetFields());
        }

        private async Task<CropsListViewModel> GetFields()
        {
            var fields = _ud.GetFields();
            var resultFields = _mapper.Map<List<Agri.Models.Farm.Field>, List<CropsListViewModel.Field>>(fields);

            foreach (var field in resultFields)
            {
                foreach (var crop in field.Crops)
                {
                    crop.CropName = _sd.GetCrop(crop.CropId).CropName;
                }
            }

            return await Task.FromResult(new CropsListViewModel
            {
                Fields = resultFields,
            });
        }

        public class CropsListViewModel
        {
            public List<Field> Fields { get; set; } = new List<Field>();

            public class Field
            {
                public int Id { get; set; }
                public string FieldName { get; set; }
                public List<FieldCrop> Crops { get; set; } = new List<FieldCrop>();
            }

            public class FieldCrop
            {
                public int Id { get; set; }
                public int CropId { get; set; }
                public string CropName { get; set; }
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Agri.Models.Farm.Field, CropsListViewModel.Field>();
                CreateMap<Agri.Models.Farm.FieldCrop, CropsListViewModel.FieldCrop>()
                    .ForMember(m => m.CropId, opts => opts.MapFrom(s => !string.IsNullOrWhiteSpace(s.cropId) ? Convert.ToInt32(s.cropId) : 0))
                    .ForMember(m => m.CropName, opts => opts.MapFrom(s => s.cropOther));
            }
        }
    }
}