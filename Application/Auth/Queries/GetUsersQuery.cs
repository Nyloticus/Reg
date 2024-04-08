using Application.Auth.Dto;
using Common;
using Infrastructure;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Queries
{
    public class GetUsersQuery : IRequest<Result>
    {        
        public class Handler : IRequestHandler<GetUsersQuery, Result>
        {
            private readonly IAppDbContext _context;
            private readonly IIdentityService _identityService;
            public Handler(IAppDbContext context, IIdentityService identityService)
            {
                _context = context;
                _identityService = identityService;
            }

            public async Task<Result> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            {
                var users = await _context.Users.ToListAsync();

                return Result<List<UserDto>>.Success(users.Adapt<List<UserDto>>().ToList());
            }
        }
    }


}
