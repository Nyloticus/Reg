using Application.City.Dto;
using Common;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.City.Queries
{
    public class GetAllCityQuery : IRequest<Result>
    {
        public class Handler : IRequestHandler<GetAllCityQuery, Result>
        {
            private IAppDbContext _context;

            public Handler(IAppDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(GetAllCityQuery request, CancellationToken cancellationToken)
            {
                var cities = await _context.Cities.ProjectToType<CityDto>().ToListAsync(cancellationToken);
                return Result<List<CityDto>>.Success(cities);
            }
        }
    }
}
