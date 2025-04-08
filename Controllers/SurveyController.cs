using System;
using Microsoft.AspNetCore.Mvc;

namespace BigBash.Controllers
{
    [ApiController]
    [Route("api/workout-plans")]
    public class SurveyController : ControllerBase
    {
        [HttpPost]
        public IActionResult SubmitWorkoutPlan([FromBody] WorkoutPlanRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.ExperienceLevel) || string.IsNullOrEmpty(request.TrainingType))
            {
                return BadRequest(new { error = "Invalid input data" });
            }

            // Simulate creating a workout plan (e.g., save to database, generate plan ID)
            var response = new
            {
                workoutPlanId = Guid.NewGuid().ToString(),
                message = "Workout plan successfully created"
            };

            return Ok(response);
        }
    }

    public class WorkoutPlanRequest
    {
        public string ExperienceLevel { get; set; } // Beginner, Intermediate, Advanced
        public string TrainingType { get; set; } // General, CrossFit, PowerLifting
        public TrainingDaysPerWeek TrainingDaysPerWeek { get; set; }
        public int PlanDurationWeeks { get; set; } // 4, 6, or 8
        public string AdditionalDetails { get; set; } // Optional free-text input
    }

    public class TrainingDaysPerWeek
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }
}