using Common;
using Domain.Entities.Auth;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IIdentityService
    {
        Task<Result> RegisterAsync(User userToAdd, string password);
        Task<Result> LoginAsync(string email, string password);
        Task<Result> ValidateUserPassword(string userId, string password);
        Task<Result> RefreshTokenAsync(string token, string refreshToken);
        Task<Result> GenerateAuthenticationResultForUserAsync(User user);
        Task<Result> RemoveUserAsync(string userId);
        Task<Result> ChangeUserActivationAsync(string userId, bool activate);
    }
}