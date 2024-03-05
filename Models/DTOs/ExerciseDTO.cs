namespace FitnessTracker.Models.DTOs;

public class ExerciseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int WorkoutTypeId { get; set; }
    public WorkoutTypeDTO? WorkoutType { get; set; }
    public List<WorkoutExerciseDTO>? WorkoutExercises { get; set; }
}