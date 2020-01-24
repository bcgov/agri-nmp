using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.Data;
using Agri.Models;
using AutoMapper;
using Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Pages.Ranch.RanchFeeding
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

            ////If no fields exist with seasonal feed skip
            if (!data.Fields.Any())
            {
                if (Request.Headers["referer"].ToString().Contains("RanchFields"))
                {
                    return RedirectToAction(CoreSiteActions.SoilTest.ToString(), AppControllers.Soil.ToString());
                }
                else
                {
                    return RedirectToPage(FeaturePages.RanchFieldsIndex.GetDescription());
                }
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
            public string feedingAreaWarning { get; set; }

            public class Field
            {
                public int Id { get; set; }
                public string FieldName { get; set; }
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Agri.Models.Farm.Field, Model.Field>();
            }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly UserData _ud;
            private readonly IAgriConfigurationRepository _sd;
            private readonly IMapper _mapper;

            public Handler(UserData ud, IMapper mapper, IAgriConfigurationRepository sd)
            {
                _ud = ud;
                _sd = sd;
                _mapper = mapper;
            }

            public Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                var fields = _ud.GetFields().Where(x => x.IsSeasonalFeedingArea == true).ToList();

                return Task.FromResult(new Model
                {
                    Fields = _mapper.Map<List<Agri.Models.Farm.Field>, List<Model.Field>>(fields),
                    feedingAreaWarning = _sd.GetUserPrompt("FeedingAreaWarning")
                }); ;
            }
        }
    }
}