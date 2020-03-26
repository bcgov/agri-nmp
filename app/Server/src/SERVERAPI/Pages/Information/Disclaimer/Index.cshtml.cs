using Agri.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SERVERAPI.Pages.Information.Disclaimer
{
    public class Index : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Model Data { get; set; }

        public Index(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query());
        }

        public class Query : IRequest<Model>
        {
        }

        public class Model
        {
            public string Message { get; set; }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly IAgriConfigurationRepository _sd;

            public Handler(IAgriConfigurationRepository sd)
            {
                _sd = sd;
            }

            public async Task<Model> Handle(Query message, CancellationToken cancellationToken)
            {
                var text = _sd.GetUserPrompt("Disclaimer");
                var result = new Model
                {
                    Message = text
                };
                return await Task.FromResult(result);
            }
        }
    }
}