namespace FitnessTracker.Models.DTOs;

public class ExerciseLogDTO
{
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public ExerciseDTO Exercise { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
}