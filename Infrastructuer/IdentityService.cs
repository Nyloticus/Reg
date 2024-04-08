using Common;
using Common.Options;
using Domain.Entities.Auth;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly JwtOption _jwtOption;
        private readonly IAppDbContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public IdentityService(UserManager<User> userManager,
          RoleManager<Role> roleManager,
          JwtOption jwtOption,
          IAppDbContext context,
          TokenValidationParameters tokenValidationParameters
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtOption = jwtOption;
            _context = context;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<Result> ValidateUserPassword(string userId, string password)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(e => e.Id == userId);

            if (user == null)
            {
                return Result.Failure(StatusCode.NotFound, "User not found");
            }
            if (!user.Active)
                return Result.Failure(StatusCode.BadRequest, "User isn't active");

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
                return Result.Failure(StatusCode.BadRequest, "Invalid Login");

            return Result.Success();

        }

        public async Task<Result> LoginAsync(string email, string password)
        {
            var user = await _userManager.Users
              .Include(u => u.UserRoles)
              .ThenInclude(u => u.Role)
              .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() || u.UserName.ToLower() == email.ToLower());


            if (user == null)
            {
                return Result.Failure(StatusCode.BadRequest, "Invalid Login");
            }

            if (!user.Active)
                return Result.Failure(StatusCode.BadRequest, "This user has been disabled");

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
                return Result.Failure(StatusCode.BadRequest, "Invalid Login");

            return await GenerateAuthenticationResultForUserAsync(user);
        }


        public async Task<Result> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);
            var storedRefreshToken = await _context.RefreshTokens
              .SingleOrDefaultAsync(e => e.Token == refreshToken);

            if (validatedToken == null)
            {
                storedRefreshToken.Invalidated = true;
                return Result.Failure(StatusCode.Unauthorized, "Invalid Token");
            }

            if (storedRefreshToken == null)
            {
                return Result.Failure(StatusCode.Unauthorized, "Refresh token not exist");
            }

            if (storedRefreshToken.Invalidated)
            {
                Result.Failure(StatusCode.Unauthorized, "Invalid refresh token");
            }
            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                storedRefreshToken.Invalidated = true;
                _context.RefreshTokens.Update(storedRefreshToken);
                Result.Failure(StatusCode.Unauthorized, "Refresh token expired");
            }
            if (storedRefreshToken.Used)
            {
                // error = new ApiException(ApiExeptionType.Unauthorized, "Refresh token used");
            }

            var userId = validatedToken.Claims.Single(e => e.Type == "UserId").Value;
            var user = await _userManager.Users
              .Include(u => u.UserRoles)
              .ThenInclude(u => u.Role)
              .FirstOrDefaultAsync(u => u.Id == userId);


            if (user == null)
            {
                Result.Failure(StatusCode.Unauthorized, "User not found");
            }

            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();


            return await GenerateAuthenticationResultForUserAsync(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                _tokenValidationParameters.ValidateLifetime = false;
                var principals = tokenHandler.ValidateToken(token, _tokenValidationParameters,
                  out var validatedToken);
                _tokenValidationParameters.ValidateLifetime = true;
                return !ISJwtWithValidSecurityAlgorithm(validatedToken) ? null : principals;
            }
            catch
            {
                return null;
            }
        }
        private bool ISJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                     StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<Result> GenerateAuthenticationResultForUserAsync(User user)
        {
            return Result<object>.Success(new
            {
                Token = await GenerateJwtToken(user),
                RefreshToken = (await GenerateRefreshTokenAsync(user)).Token,
            });
        }

        private async Task<RefreshToken> GenerateRefreshTokenAsync(User user)
        {
            var refreshToken = new RefreshToken
            {
                JwtId = Guid.NewGuid().ToString(),
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _context.Set<RefreshToken>().AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        private async Task<string> GenerateJwtToken(User appUser)
        {
            var claims = GenerateUserClaims(appUser);
            var userClaims = await _userManager.GetClaimsAsync(appUser);
            claims.AddRange(userClaims);
            var userRoles = await _userManager.GetRolesAsync(appUser);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Key));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.Add(_jwtOption.TokenLifetime);

            var token = new JwtSecurityToken(
              _jwtOption.Issuer,
              _jwtOption.Issuer,
              claims,
              expires: expires,
              signingCredentials: cred
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static List<Claim> GenerateUserClaims(User appUser)
        {
            return new List<Claim> {
        new Claim(ClaimTypes.Name, appUser.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, appUser.Id),
        new Claim("UserId", appUser.Id),
        new Claim("email", appUser.Email??""),
        new Claim("fullName", appUser.FullName??""),
        new Claim("role", JsonConvert.SerializeObject(appUser.UserRoles.Select(s => s.Role.Name))),
        new Claim("isActive", appUser.Active.ToString()),
        new Claim("isPhoneNumberConfirmed", appUser.PhoneNumberConfirmed.ToString()),
        new Claim("PhoneNumber", appUser.PhoneNumber??string.Empty),
      };
        }



        public async Task<Result> RegisterAsync(User userToAdd, string password)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == userToAdd.Email.ToLower());
            if (user != null)
                return Result.Failure(StatusCode.BadRequest, "Email already exists");
                        
            var Password = _userManager.PasswordHasher.HashPassword(user, password);
            userToAdd.PasswordHash = Password;
            userToAdd.UserName = userToAdd.Email;
            var result = await _userManager.CreateAsync(userToAdd);
            if (result.Errors.Count() > 0)
                return Result<string>.Failure(StatusCode.BadRequest, string.Join(',', result.Errors));

            return Result<string>.Success("User created successfully");
        }


        public List<string> GetUserImages(string userId)
        {
            List<string> userimages = new List<string>();
            var urls = _context.UserImages.Where(x => x.UserId == userId).Select(x => x.Url).ToList();
            if (urls.Count() != 0)
            {
                userimages.AddRange(urls);
            }
            return userimages;
        }
        public async Task<Result> RemoveUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure(StatusCode.NotFound, "User not found");

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any(r => r.ToLower().Contains("superadmin")))
                return Result.Failure(StatusCode.BadRequest, "You can't remove an superadmin user");

            var result = await _userManager.DeleteAsync(user);
            user.Active = false;
            return result.Succeeded ? Result.Success() : Result.Failure(StatusCode.NotFound, "User not found");
        }

        public async Task<Result> ChangeUserActivationAsync(string userId, bool activate)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure(StatusCode.NotFound, "User not found");

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any(r => r.ToLower().Contains("superadmin")))
                return Result.Success();
            if (activate) { user.Active = true; } else { user.Active = false; }
            return Result.Success();
        }
    }
}