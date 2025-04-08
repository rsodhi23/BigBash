using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace BigBash.Controllers
{
    [ApiController]
    [Route("api/values")]
    public class GetValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            // ...existing code for GET logic...
            return Ok(new string[] { "value1", "value2" });
        }

        [HttpGet("workout-plans")]
        public async Task<IActionResult> GetWorkoutPlans([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { error = "User ID is required" });
            }

            // Replace with your Gemini AI API key
            string apiKey = "AIzaSyCT99CjWFeLPNO6E257_JfThPWGhOc5K-s";

            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = "Generate me a workout plan, based on a 4 week plan with 1-3 exercises per week max focusing on building strength. Return it in a json format"
                                }
                            }
                        }
                    }
                };

                var response = await httpClient.PostAsJsonAsync("https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key=AIzaSyBrJLLSmIoo4Ij4ToZeGdQV8FtlXpaeDDA", requestBody);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, new { error = "Failed to fetch workout from Gemini AI" });
                }

                var result = await response.Content.ReadAsStringAsync();
                return Ok(new { workoutPlan = result });
            }
        }

        [HttpGet("workout-plans/{workoutPlanId}")]
        public IActionResult GetWorkoutPlanDetails(string workoutPlanId)
        {
            if (string.IsNullOrEmpty(workoutPlanId))
            {
                return BadRequest(new { error = "Workout Plan ID is required" });
            }

            // Simulate fetching a workout plan based on the provided ID
            var workoutPlan = new
            {
                workoutPlanId = workoutPlanId,
                planDurationWeeks = 8,
                trainingDaysPerWeek = 5,
                completed = false,
                workoutPlan = new[]
                {
                    new
                    {
                        week = 1,
                        days = new[]
                        {
                            new
                            {
                                day = 1,
                                workouts = new[]
                                {
                                    new
                                    {
                                        workoutId = "w1",
                                        workoutName = "Squats",
                                        description = "Leg workout",
                                        sets = new[]
                                        {
                                            new
                                            {
                                                setId = "s1",
                                                setNumber = 1,
                                                reps = 10,
                                                weight = 50.0,
                                                completed = false
                                            }
                                        },
                                        restTimeSeconds = 60,
                                        completed = false
                                    }
                                }
                            }
                        }
                    }
                }
            };

            return Ok(workoutPlan);
        }
    }
}
