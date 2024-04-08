using Application.City.Queries;
using Application.Governate.Queries;
using Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LookUpController : BaseController
    {

        [AllowAnonymous]
        [HttpGet("cities-all")]
        public async Task<Result> GetCities([FromQuery] GetAllCityQuery request)
        {
            return (await Sender.Send(request));
        }

        [AllowAnonymous]
        [HttpGet("governate-all")]
        public async Task<Result> GetGovernate([FromQuery] GetAllGovernateQuery request)
        {
            return (await Sender.Send(request));
        }

        public LookUpController(ISender sender) : base(sender)
        {

        }
    }
}
