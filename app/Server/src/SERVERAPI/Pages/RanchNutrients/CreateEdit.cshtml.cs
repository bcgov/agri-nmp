using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        }
    }
}