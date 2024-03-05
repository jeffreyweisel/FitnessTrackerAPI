using Microsoft.AspNetCore.Identity;

namespace FitnessTracker.Models.DTOs;

public class UserProfileDTO   
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public List<string> Roles { get; set; }
    public string IdentityUserId { get; set; }
    public IdentityUser IdentityUser { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public int Age { get; set; }
    public decimal Weight { get; set; }
    public string Height { get; set; }
    public string GeneralGoal { get; set; }
    public int GoalWeightInPounds { get; set; }
    public int GoalBenchMaxInPounds { get; set; }
    public int GoalSquatMaxInPounds { get; set; }
    public int GoalDeadliftMaxInPounds { get; set; }
    // Pounds until goal body weight calculated property
    public decimal? PoundsUntilGoalWeightReached
    {
        get
        {
            return Weight - GoalWeightInPounds;
        }
    }
    public int MaxBench { get; set; }
    // Pounds until goal max bench calculated property
     public decimal? PoundsUntilGoalBench
    {
        get
        {
            return GoalBenchMaxInPounds - MaxBench;
        }
    }
    public int MaxSquat { get; set; }
    // Pounds until goal max squat calculated property
     public decimal? PoundsUntilGoalSquat
    {
        get
        {
            return GoalSquatMaxInPounds - MaxSquat;
        }
    }
    public int MaxDeadlift { get; set; }
    // Pounds until goal max deadlift calculated property
     public decimal? PoundsUntilGoalDeadlift
    {
        get
        {
            return GoalDeadliftMaxInPounds - MaxDeadlift;
        }
    }
      public List<WorkoutDTO> Workouts { get; set; }
    
}