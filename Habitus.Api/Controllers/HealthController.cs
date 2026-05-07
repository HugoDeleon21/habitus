using Microsoft.AspNetCore.Mvc;

namespace Habitus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "API Habitus funcionando",
                data = DateTime.Now
            });
        }
    }
}