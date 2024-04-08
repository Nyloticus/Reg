using Common;
using Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Login
{
    public class RefreshTokenCommand : IRequest<Result>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result>
        {
            private readonly IIdentityService _identityService;

            public RefreshTokenHandler(IIdentityService identityService)
            {
                _identityService = identityService;
            }
            public async Task<Result> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
            {
                return await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);
            }
        }
    }
}
