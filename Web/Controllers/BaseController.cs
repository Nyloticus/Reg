using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly ISender Sender;

        public BaseController(ISender sender)
        {
            Sender = sender;
        }
    }
}