using Common;
using Domain.Entities.Auth;
using Domain.Utils;
using FluentValidation;
using Infrastructure;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Register
{
    public class RegisterCommand : IRequest<Result>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string MobileNumber { get; set; }
        public List<AddressList> AddressList { get; set; }

        public class Validator : AbstractValidator<RegisterCommand>
        {
            public Validator()
            {
                RuleFor(r => r.FirstName).NotEmpty()
                   .WithMessage("First Name is Required")
                   .MaximumLength(20)
                   .WithMessage("Maximum Length is 20");

                RuleFor(r => r.MiddleName).NotEmpty()
                    .WithMessage("Midd;e Name is Required")
                    .MaximumLength(40)
                    .WithMessage("Maximum Length is 40");

                RuleFor(r => r.LastName).NotEmpty()
                   .WithMessage("Last Name is Required")
                   .MaximumLength(20)
                   .WithMessage("Maximum Length is 20");

                RuleFor(r => r.Email).NotEmpty()
                   .WithMessage("Email is Required")
                   .EmailAddress()
                   .WithMessage("Invalid Email Format");

                RuleFor(r => r.BirthDate).NotEmpty()
                   .WithMessage("Birth date is required")
                   .Must(BeAtLeastTwentyYearsOld)
                   .WithMessage("Minimum age requirement is 20 years");

                RuleFor(r => r.MobileNumber).NotEmpty()
                   .WithMessage("Mobile is required")
                   .Matches(@"^\+\d{12}$")
                   .WithMessage("Invalid mobile number format. Example: +021006158123");

                RuleFor(r => r.Password).NotEmpty()
                    .WithMessage("Password is required");

                RuleForEach(r => r.AddressList)
                         .ChildRules(address =>
                         {
                             address.RuleFor(a => a.GovernateId)
                                 .NotEmpty().WithMessage("Governate is required");

                             address.RuleFor(a => a.CityId)
                                 .NotEmpty().WithMessage("City is required");

                             address.RuleFor(a => a.Street)
                                 .NotEmpty().WithMessage("Street is required")
                                 .MaximumLength(200).WithMessage("Street cannot exceed 200 characters");

                             address.RuleFor(a => a.BuildNo)
                                 .NotEmpty().WithMessage("Building number is required");

                             address.RuleFor(a => a.FlatNo)
                                 .NotNull().WithMessage("Flat number is required");
                         });
            }
            private bool BeAtLeastTwentyYearsOld(DateTime birthDate)
            {
                int age = DateTime.Today.Year - birthDate.Year;
                if (birthDate.Date > DateTime.Today.AddYears(-age))
                    age--;

                return age >= 20;
            }
        }
        public class Handler : IRequestHandler<RegisterCommand, Result>
        {
            private readonly IIdentityService _identityService;
            private readonly IAppDbContext _context;
            public Handler(IIdentityService identityService, IAppDbContext context)
            {
                _identityService = identityService;
                _context = context;
            }

            public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                //validate Ids
                var allCities = _context.Cities.ToList();
                var invalidCityIds = request.AddressList
                   .Select(a => a.CityId)
                   .Where(cityId => !allCities.Select(i => i.Id).Contains(cityId))
                   .ToList();

                if (invalidCityIds.Any())
                    return Result.Failure(StatusCode.NotFound, $"Invalid City Ids {string.Join(',', invalidCityIds)}");

                var allGovernates = _context.Governates.ToList();
                var invalidGovernateIds = request.AddressList
                   .Select(a => a.GovernateId)
                   .Where(cityId => !allGovernates.Select(i => i.Id).Contains(cityId))
                   .ToList();

                if (invalidGovernateIds.Any())
                    return Result.Failure(StatusCode.NotFound, $"Invalid Governate Ids {string.Join(',', invalidGovernateIds)}");

                var userToAdd = request.Adapt<User>();
                var addressList = request.AddressList.Adapt<List<Address>>();
                userToAdd.Addresses = addressList.ToHashSet();

                var result = await _identityService.RegisterAsync(userToAdd, request.Password);
                if (!result.IsSuccess)
                    return result;

                return Result.Success();
            }
        }
    }
}
