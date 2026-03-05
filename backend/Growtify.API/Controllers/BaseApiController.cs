using Growtify.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Growtify.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
