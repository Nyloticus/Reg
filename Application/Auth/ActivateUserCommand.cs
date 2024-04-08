using Common;
using FluentValidation;
using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth
{
    public class ActivateUserCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public bool Activate { get; set; }
        public class Validator : AbstractValidator<ActivateUserCommand>
        {
            public Validator()
            {
                RuleFor(r => r.UserId).NotEmpty()
                    .WithMessage("User Id is required");
            }
        }


        public class Handler : IRequestHandler<ActivateUserCommand, Result>
        {
            private readonly IAppDbContext _context;
            private readonly IIdentityService _identityService;
            public Handler(IAppDbContext context, IIdentityService identityService)
            {
                _context = context;
                _identityService = identityService;
            }

            public async Task<Result> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
            {
                var result = await _identityService.ChangeUserActivationAsync(request.UserId, request.Activate);
                return result;
            }
        }
    }


}
