using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.Data;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Pages.RanchNutrients
{
    public class CreateEdit : BasePageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Command Data { get; set; }

        [BindProperty]
        public bool IsModal { get; set; }

        public CreateEdit(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetCreateAsync()
        {
            Data = await _mediator.Send(new Query());
        }

        public async Task OnGetEditAsync(Query query)
        {
            Data = await _mediator.Send(query);
        }

        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
        }

        public class Command : IRequest<Unit>
        {
            public int? Id { get; set; }
            public SelectList RanchManures { get; set; }
            public SelectList BeefNutrientAnalysisOptions { get; set; }
        }

        public class Handler : IRequestHandler<Query, Command>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;
            private readonly IAgriConfigurationRepository _sd;

            public Handler(UserData ud, IMapper mapper, IAgriConfigurationRepository sd)
            {
                _ud = ud;
                _mapper = mapper;
                _sd = sd;
            }

            public Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                //var ranchManures = _ud.
                var beefManuresNutrients = _sd.GetManures()
                    .Where(m => m.ManureClass.Contains("Beef - feedlot"))
                    .Select(m => new { Id = m.Id, Name = m.Name })
                    .ToList();

                //command.CattleSubTypeOptions = new SelectList(subTypeOptions, "Id", "Value");
                var command = new Command
                {
                    BeefNutrientAnalysisOptions = new SelectList(beefManuresNutrients, "Id", "Name")
                };
                return Task.FromResult(command);
            }
        }
    }
}