namespace FitnessTracker.Models.DTOs;

public class WorkoutExerciseDTO
{
    public int Id { get; set; }
    public int WorkoutId { get; set; }
    public WorkoutDTO Workout { get; set; }
    public int ExerciseId { get; set; }
    public ExerciseDTO Exercise { get; set; }
}
