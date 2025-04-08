using System;
using Microsoft.AspNetCore.Mvc;

namespace BigBash.Controllers
{
    [ApiController]
    [Route("api/workout-plans")]
    public class SurveyController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SubmitWorkoutPlanAsync([FromBody] WorkoutPlanRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.ExperienceLevel) || string.IsNullOrEmpty(request.TrainingType))
            {
                return BadRequest(new { error = "Invalid input data" });
            }

            //
            // Replace with your Gemini AI API key
            string apiKey = "AIzaSyCT99CjWFeLPNO6E257_JfThPWGhOc5K-s";

            using (var httpClient = new HttpClient())
            {
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
                                    text = $"Generate me a workout plan based on the following details: Experience Level - {request.ExperienceLevel}, Training Type - {request.TrainingType}, Training Days Per Week - {request.TrainingDaysPerWeek.Min}-{request.TrainingDaysPerWeek.Max}, Plan Duration - {request.PlanDurationWeeks} weeks. Additional Details: {request.AdditionalDetails}. Return it in a JSON format."
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
                //var responseModel = JsonConvert.DeserializeObject<ResponseModel>(result);
                //return Ok(responseModel);
                 return Ok();
            }
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

    public class ResponseModel
    {
        public string WorkoutPlan { get; set; }
    }
}