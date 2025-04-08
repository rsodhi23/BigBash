using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using System.Threading.Tasks;
using BigBash.Models;

namespace BigBash.Controllers
{
    [ApiController]
    [Route("api/values")]
    public class GetValuesController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public GetValuesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Get()
        {
            // ...existing code for GET logic...
            return Ok(new string[] { "value1", "value2" });
        }

        [HttpGet("workout-plans/{workoutPlanId}")]
        public IActionResult GetWorkoutPlanDetails(string workoutPlanId)
        {
            if (string.IsNullOrEmpty(workoutPlanId))
            {
                return BadRequest(new { error = "Workout Plan ID is required" });
            }

            // Retrieve the workout plan from MemoryCache
            if (!_memoryCache.TryGetValue(workoutPlanId, out ResponseModel workoutPlan))
            {
                return NotFound(new { error = "Workout Plan not found" });
            }

            return Ok(workoutPlan);
        }
    }
}
