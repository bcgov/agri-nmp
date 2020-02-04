using Agri.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;
using System.Threading.Tasks;

namespace SERVERAPI.Pages.MiniApps
{
    public class ManureConvertor : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public ConverterQuery Data { get; set; }

        [BindProperty]
        public ResultModel Result { get; set; }

        public ManureConvertor(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query { PopulatedData = new ConverterQuery() });
            Result = new ResultModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                //Send the Data to Receive conversion result
                Result = await _mediator.Send(Data);
            }
            Data = await _mediator.Send(new Query { PopulatedData = Data });
            return Page();
        }

        public class Query : IRequest<ConverterQuery>
        {
            public ConverterQuery PopulatedData { get; set; }
        }

        public class ConverterQuery : IRequest<ResultModel>
        {
        }

        public class ResultModel
        {
        }

        public class ModelValidator : AbstractValidator<ConverterQuery>
        {
            public ModelValidator()
            {
            }
        }

        public class Handler :
            IRequestHandler<Query, ConverterQuery>,
            IRequestHandler<ConverterQuery, ResultModel>
        {
            private readonly IAgriConfigurationRepository _sd;

            public Handler(IAgriConfigurationRepository sd)
            {
                _sd = sd;
            }

            public async Task<ConverterQuery> Handle(Query request, CancellationToken cancellationToken)
            {
                var command = request.PopulatedData;

                return await Task.FromResult(command);
            }

            public Task<ResultModel> Handle(ConverterQuery request, CancellationToken cancellationToken)
            {
                var result = new ResultModel
                {
                };
                return Task.FromResult(result);
            }
        }
    }
}