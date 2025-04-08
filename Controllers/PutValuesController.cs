using Microsoft.AspNetCore.Mvc;

namespace BigBash.Controllers
{
    [ApiController]
    [Route("api/values")]
    public class PutValuesController : ControllerBase
    {
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            // ...existing code for PUT logic...
            return Ok();
        }
    }
}
