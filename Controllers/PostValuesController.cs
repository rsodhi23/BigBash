using Microsoft.AspNetCore.Mvc;

namespace BigBash.Controllers
{
    [ApiController]
    [Route("api/values")]
    public class PostValuesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            // ...existing code for POST logic...
            return Ok();
        }
    }
}
