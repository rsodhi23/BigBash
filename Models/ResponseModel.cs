namespace BigBash.Models
{
    public class ResponseModel
    {
        public string WorkoutPlanId { get; set; }
        public int PlanDurationWeeks { get; set; }
        public int TrainingDaysPerWeek { get; set; }
        public bool Completed { get; set; }
        public List<WorkoutWeek> WorkoutPlan { get; set; }
    }

    public class WorkoutWeek
    {
        public int Week { get; set; }
        public List<WorkoutDay> Days { get; set; }
    }

    public class WorkoutDay
    {
        public int Day { get; set; }
        public List<Workout> Workouts { get; set; }
    }

    public class Workout
    {
        public string WorkoutId { get; set; }
        public string WorkoutName { get; set; }
        public string Description { get; set; }
        public List<WorkoutSet> Sets { get; set; }
        public int RestTimeSeconds { get; set; }
        public bool Completed { get; set; }
    }

    public class WorkoutSet
    {
        public string SetId { get; set; }
        public int SetNumber { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
        public bool Completed { get; set; }
    }
}