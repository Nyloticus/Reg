//using Application.Notifications;
using Common;
using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Login
{
    public class LoginCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string Password { get; set; }        

        class Handler : IRequestHandler<LoginCommand, Result>
        {
            private readonly IIdentityService _identityService;
            private readonly IAppDbContext _context;            

            public Handler(IIdentityService identityService, IAppDbContext context)
            {
                _identityService = identityService;
                _context = context;                
            }

            public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var result = await _identityService.LoginAsync(request.Email, request.Password);
                return result;
            }
        }
    }
}