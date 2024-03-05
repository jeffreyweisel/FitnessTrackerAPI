using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Data;
using FitnessTracker.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace FitnessTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExerciseController : ControllerBase
{
    private FitnessTrackerDbContext _dbContext;

    public ExerciseController(FitnessTrackerDbContext context)
    {
        _dbContext = context;
    }

// Get all exercises with user and all related info included
public IActionResult Get()
{
    var exercises = _dbContext
        .Exercises
        .Include(e => e.WorkoutType)
        .Include(w => w.WorkoutExercises)
        .Select(w => new ExerciseDTO
        {
            Id = w.Id,
            Name = w.Name,
            WorkoutTypeId = w.WorkoutTypeId,
            WorkoutExercises = w.WorkoutExercises
                .Select(we => new WorkoutExerciseDTO
                {
                    Id = we.Id,
                    ExerciseId = we.ExerciseId,
                    Exercise = new ExerciseDTO
                    {
                        Id = we.Exercise.Id,
                        Name = we.Exercise.Name,
                        WorkoutTypeId = we.Exercise.WorkoutTypeId,
                        WorkoutType = new WorkoutTypeDTO
                        {
                            Id = we.Exercise.WorkoutType.Id,
                            Name = we.Exercise.WorkoutType.Name
                        }
                    }
                })
                .ToList()
        })
        .ToList();

    return Ok(exercises);
}

// Get a exercise by Id
[HttpGet("{id}")]
public IActionResult GetExerciseById(int id)
{
    Exercise exercise = _dbContext
        .Exercises
        .Include(w => w.WorkoutType)
        .Include(w => w.WorkoutExercises)
        .SingleOrDefault(w => w.Id == id);

    if (exercise == null)
    {
        return NotFound();
    }

    return Ok(new ExerciseDTO
    {
        Id = exercise.Id,
        Name = exercise.Name,
        WorkoutTypeId = exercise.WorkoutTypeId,
        WorkoutExercises = exercise.WorkoutExercises
            .Select(we => new WorkoutExerciseDTO
            {
                Id = we.Id,
                ExerciseId = we.ExerciseId,
                Exercise = we.Exercise != null
                    ? new ExerciseDTO
                    {
                        Id = we.Exercise.Id,
                        Name = we.Exercise.Name,
                        WorkoutTypeId = we.Exercise.WorkoutTypeId,
                        WorkoutType = we.Exercise.WorkoutType != null
                            ? new WorkoutTypeDTO
                            {
                                Id = we.Exercise.WorkoutType.Id,
                                Name = we.Exercise.WorkoutType.Name
                            }
                            : null,  // Handle potential null WorkoutType
                    }
                    : null,  // Handle potential null Exercise
            })
            .ToList()
    });
}

// Create new exercise

    [HttpPost]
    // [Authorize]
    public IActionResult CreateExercise(Exercise exercise)
    {
        try
        {
            _dbContext.Exercises.Add(exercise);
            _dbContext.SaveChanges();

            return Created($"/api/exercise/{exercise.Id}", exercise);
        }
        catch (DbUpdateException ex)
        {
            //  exception details
            Console.WriteLine("DbUpdateException Message: " + ex.Message);
            Console.WriteLine("Inner Exception Message: " + ex.InnerException?.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);

            return StatusCode(500, "Error creating home listing");
        }
        catch (Exception ex)
        {
            // Log any other exceptions
            Console.WriteLine("Exception Message: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);

            return StatusCode(500, "Error creating home listing");
        }
    }
}