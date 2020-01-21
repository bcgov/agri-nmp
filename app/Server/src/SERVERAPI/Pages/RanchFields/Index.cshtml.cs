using Agri.Models.Farm;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SERVERAPI.Pages.RanchFields
{
    public class Index : BasePageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Model Data { get; private set; }

        public Index(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> OnGetAsync(Query query)
        {
            var data = await _mediator.Send(query);

            //If no fields exist redirect to the CreateEdit PAGE
            if (!data.Fields.Any())
            {
                return RedirectToPage("CreateEdit", "Create", new { ismodal = false });
            }

            Data = data;

            return Page();
        }

        public class Query : IRequest<Model>
        {
        }

        public class Model
        {
            public List<Field> Fields { get; set; }

            public class Field
            {
                public Field()
                {
                    Crops = new List<FieldCrop>();
                }

                public int Id { get; set; }
                public string FieldName { get; set; }
                public decimal Area { get; set; }
                public string Comment { get; set; }
                public Nutrients Nutrients { get; set; }
                public List<FieldCrop> Crops { get; set; }
                public SoilTest SoilTest { get; set; }
                public string PrevYearManureApplicationFrequency { get; set; }
                public int? PrevYearManureApplicationNitrogenCredit { get; set; }
                public decimal? SoilTestNitrateOverrideNitrogenCredit { get; set; }
                public bool IsSeasonalFeedingArea { get; set; }
                public string SeasonalFeedingArea { get; set; }
            }

            public class FieldCrop
            {
                public int Id { get; set; }
                public string CropId { get; set; }
                public string CropOther { get; set; }
                public decimal Field { get; set; } // tons/acre
                public decimal ReqN { get; set; }
                public decimal StdN { get; set; }
                public decimal ReqP2o5 { get; set; }
                public decimal ReqK2o { get; set; }
                public decimal RemN { get; set; }
                public decimal RemP2o5 { get; set; }
                public decimal RemK2o { get; set; }
                public decimal? CrudeProtien { get; set; }
                public int PrevCropId { get; set; }
                public bool? CoverCropHarvested { get; set; }
                public int PrevYearManureAppl_volCatCd { get; set; }
                public int? YieldHarvestUnit { get; set; }
                public decimal YieldByHarvestUnit { get; set; }
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Agri.Models.Farm.Field, Model.Field>();
                CreateMap<Agri.Models.Farm.FieldCrop, Model.FieldCrop>();
            }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;

            public Handler(UserData ud, IMapper mapper)
            {
                _ud = ud;
                _mapper = mapper;
            }

            public Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                var fields = _ud.GetFields();

                return Task.FromResult(new Model
                {
                    Fields = _mapper.Map<List<Agri.Models.Farm.Field>, List<Model.Field>>(fields)
                });
            }
        }
    }
}