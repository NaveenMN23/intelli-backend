using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntelliCRMAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthcheckController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Healthcheck()
        {
            return "Gateway Service Working..!!!";
        }

     }
}
