using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetWorkoutPlans([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { error = "User ID is required" });
            }

            // Simulate fetching workout plans for the user (e.g., from a database)
            var workoutPlans = new[]
            {
                new { PlanId = "1", Name = "Beginner Plan", DurationWeeks = 4 },
                new { PlanId = "2", Name = "Advanced Plan", DurationWeeks = 8 }
            };

            return Ok(workoutPlans);
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
