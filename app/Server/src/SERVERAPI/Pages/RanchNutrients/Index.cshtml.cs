using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Pages.RanchNutrients
{
    public class Index : BasePageModel
    {
        [BindProperty]
        public Model Data { get; set; }

        public async Task OnGetAsync()
        {
        }

        public class Query : IRequest<Model>
        {
        }

        public class Model
        {
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

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}