using Application.Governate.Dto;
using Common;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Governate.Queries
{
    public class GetAllGovernateQuery : IRequest<Result>
    {
        public class Handler : IRequestHandler<GetAllGovernateQuery, Result>
        {
            private IAppDbContext _context;

            public Handler(IAppDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(GetAllGovernateQuery request, CancellationToken cancellationToken)
            {
                var govers = await _context.Governates.ProjectToType<GovernateDto>().ToListAsync(cancellationToken);
                return Result<List<GovernateDto>>.Success(govers);
            }
        }
    }
}
