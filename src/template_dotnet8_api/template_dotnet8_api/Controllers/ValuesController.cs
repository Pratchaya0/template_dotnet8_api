using Microsoft.AspNetCore.Mvc;

namespace template_dotnet8_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        [HttpGet]
        public bool Get()
        {
            return true;
        }
    }
}
