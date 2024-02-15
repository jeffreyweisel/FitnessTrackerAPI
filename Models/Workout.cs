namespace FitnessTracker.Models;

public class Workout
{
    public int Id { get; set; }
    public int UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; }
    public DateTime WorkoutCompletedOn { get; set; }
    public List<WorkoutExercise>? WorkoutExercises { get; set; }
}
