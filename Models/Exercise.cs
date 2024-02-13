namespace FitnessTracker.Models;

public class Exercise 
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int WorkoutTypeId { get; set; }
    public WorkoutType WorkoutType { get; set; }
}