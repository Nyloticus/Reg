using Common;
using FluentValidation;
using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth
{
    public class RemoveUserCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public class Validator : AbstractValidator<RemoveUserCommand>
        {
            public Validator()
            {
                RuleFor(r => r.UserId).NotEmpty()
                    .WithMessage("User Id is required");
            }
        }


        public class Handler : IRequestHandler<RemoveUserCommand, Result>
        {
            private readonly IAppDbContext _context;
            private readonly IIdentityService _identityService;
            public Handler(IAppDbContext context, IIdentityService identityService)
            {
                _context = context;
                _identityService = identityService;
            }

            public async Task<Result> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
            {
                var result = await _identityService.RemoveUserAsync(request.UserId);
                return result;
            }
        }
    }


}
