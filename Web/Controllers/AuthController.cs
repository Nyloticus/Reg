using Application.Auth;
using Application.Auth.Login;
using Application.Auth.Queries;
using Application.Auth.Register;
using Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<Result> Login(
          [FromBody] LoginCommand loginDto)
        {
            return (await Sender.Send(loginDto));
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<Result> RefreshToken([FromBody] RefreshTokenCommand request)
        {
            return (await Sender.Send(request));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<Result> RegisterManager([FromBody] RegisterCommand request)
        {
            return (await Sender.Send(request));
        }


        [HttpGet("get-users")]
        public async Task<Result> GetUsers([FromQuery] GetUsersQuery request)
        {
            return (await Sender.Send(request));
        }


        [HttpPost("activate-user")]
        public async Task<Result> ActivateUser([FromBody] ActivateUserCommand request)
        {
            return (await Sender.Send(request));
        }

        public AuthController(ISender sender) : base(sender)
        {

        }
    }
}