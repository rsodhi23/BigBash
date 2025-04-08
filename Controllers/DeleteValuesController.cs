using Microsoft.AspNetCore.Mvc;

namespace BigBash.Controllers
{
    [ApiController]
    [Route("api/values")]
    public class DeleteValuesController : ControllerBase
    {
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // ...existing code for DELETE logic...
            return Ok();
        }
    }
}
