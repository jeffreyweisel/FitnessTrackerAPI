namespace FitnessTracker.Models.DTOs;

public class WorkoutDTO
{
    public int Id { get; set; }
    public int UserProfileId { get; set; }
    public UserProfileDTO UserProfile { get; set; }
    public DateTime WorkoutCompletedOn { get; set; }
    public List<WorkoutExerciseDTO>? WorkoutExercises { get; set; }
}