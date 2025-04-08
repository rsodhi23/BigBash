using BigBash.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;

namespace BigBash.Controllers
{
    [ApiController]
    [Route("api/workout-plans")]
    public class SurveyController : ControllerBase
    {
        // Inject MemoryCache service
        private readonly IMemoryCache _memoryCache;

        public SurveyController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitWorkoutPlanAsync([FromBody] WorkoutPlanRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.ExperienceLevel) || string.IsNullOrEmpty(request.TrainingType))
            {
                return BadRequest(new { error = "Invalid input data" });
            }

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
                                    text = $"Generate me a workout plan based on the following details: Experience Level - {request.ExperienceLevel}, Training Type - {request.TrainingType}, Training Days Per Week - {request.TrainingDaysPerWeek.Min}-{request.TrainingDaysPerWeek.Max}, Plan Duration - {request.PlanDurationWeeks} weeks. Additional Details: {request.AdditionalDetails}. Return it format matching this structure: {{ \"WorkoutPlanId\": \"string\", \"PlanDurationWeeks\": \"int\", \"TrainingDaysPerWeek\": \"int\", \"Completed\": \"bool\", \"WorkoutPlan\": [{{ \"Week\": \"int\", \"Days\": [{{ \"Day\": \"int\", \"Workouts\": [{{ \"WorkoutId\": \"string\", \"WorkoutName\": \"string\", \"Description\": \"string\", \"Sets\": [{{ \"SetId\": \"string\", \"SetNumber\": \"int\", \"Reps\": \"int\", \"Weight\": \"double\", \"Completed\": \"bool\" }}], \"RestTimeSeconds\": \"int\", \"Completed\": \"bool\" }}] }}] }}] }}."
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
                var geminiResult = JsonConvert.DeserializeObject<GeminiResponseModel>(result);
                var res = geminiResult?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

                if (string.IsNullOrEmpty(res))
                {
                    return StatusCode(500, new { error = "Invalid response from Gemini AI" });
                }

                res = res.Replace("```json", string.Empty).Replace("```", string.Empty);

                var responseModel = JsonConvert.DeserializeObject<ResponseModel>(res);

                if (responseModel == null || string.IsNullOrEmpty(responseModel.WorkoutPlanId))
                {
                    return StatusCode(500, new { error = "Failed to parse workout plan" });
                }

                // Save the workout plan in MemoryCache
                _memoryCache.Set(responseModel.WorkoutPlanId, responseModel);

                return Ok(responseModel.WorkoutPlanId);
            }
        }
    }

    public class WorkoutPlanRequest
    {
        public string ExperienceLevel { get; set; } = string.Empty; // Beginner, Intermediate, Advanced
        public string TrainingType { get; set; } = string.Empty; // General, CrossFit, PowerLifting
        public TrainingDaysPerWeek TrainingDaysPerWeek { get; set; } = new TrainingDaysPerWeek();
        public int PlanDurationWeeks { get; set; } // 4, 6, or 8
        public string AdditionalDetails { get; set; } = string.Empty; // Optional free-text input
    }

    public class TrainingDaysPerWeek
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }
}