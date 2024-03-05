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
public class WorkoutController : ControllerBase
{
    private FitnessTrackerDbContext _dbContext;

    public WorkoutController(FitnessTrackerDbContext context)
    {
        _dbContext = context;
    }

// Get all workouts with user and all related info included
public IActionResult Get()
{
    var workouts = _dbContext
        .Workouts
        .Include(w => w.UserProfile)
        .Include(w => w.WorkoutExercises)
            .ThenInclude(we => we.Exercise)
        .Select(w => new WorkoutDTO
        {
            Id = w.Id,
            WorkoutCompletedOn = w.WorkoutCompletedOn,
            UserProfileId = w.UserProfileId,
            UserProfile = new UserProfileDTO
            {
                Id = w.UserProfile.Id,
                FirstName = w.UserProfile.FirstName,
                LastName = w.UserProfile.LastName,
                Email = w.UserProfile.IdentityUser.Email,
                ProfilePicture = w.UserProfile.ProfilePicture,
                Age = w.UserProfile.Age,
                Height = w.UserProfile.Height,
                Weight = w.UserProfile.Weight,
                GeneralGoal = w.UserProfile.GeneralGoal,
                GoalWeightInPounds = w.UserProfile.GoalWeightInPounds,
                GoalBenchMaxInPounds = w.UserProfile.GoalBenchMaxInPounds,
                GoalDeadliftMaxInPounds = w.UserProfile.GoalDeadliftMaxInPounds,
                GoalSquatMaxInPounds = w.UserProfile.GoalSquatMaxInPounds,
                MaxBench = w.UserProfile.MaxBench,
                MaxDeadlift = w.UserProfile.MaxDeadlift,
                MaxSquat = w.UserProfile.MaxSquat,
            },
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

    return Ok(workouts);
}

// Get a workout by Id
[HttpGet("{id}")]
public IActionResult GetWorkoutById(int id)
{
    Workout workout = _dbContext
        .Workouts
        .Include(w => w.UserProfile)
        .Include(w => w.WorkoutExercises)
            .ThenInclude(we => we.Exercise)
        .SingleOrDefault(w => w.Id == id);

    if (workout == null)
    {
        return NotFound();
    }

    return Ok(new WorkoutDTO
    {
        Id = workout.Id,
        WorkoutCompletedOn = workout.WorkoutCompletedOn,
        UserProfileId = workout.UserProfileId,
        UserProfile = workout.UserProfile != null
            ? new UserProfileDTO
            {
                Id = workout.UserProfile.Id,
                FirstName = workout.UserProfile.FirstName,
                LastName = workout.UserProfile.LastName,
                Email = workout.UserProfile.IdentityUser?.Email,  // Add null check for IdentityUser
                ProfilePicture = workout.UserProfile.ProfilePicture,
                Age = workout.UserProfile.Age,
                Height = workout.UserProfile.Height,
                Weight = workout.UserProfile.Weight,
                GeneralGoal = workout.UserProfile.GeneralGoal,
                GoalWeightInPounds = workout.UserProfile.GoalWeightInPounds,
                GoalBenchMaxInPounds = workout.UserProfile.GoalBenchMaxInPounds,
                GoalDeadliftMaxInPounds = workout.UserProfile.GoalDeadliftMaxInPounds,
                GoalSquatMaxInPounds = workout.UserProfile.GoalSquatMaxInPounds,
                MaxBench = workout.UserProfile.MaxBench,
                MaxDeadlift = workout.UserProfile.MaxDeadlift,
                MaxSquat = workout.UserProfile.MaxSquat,
            }
            : null,  // Handle potential null UserProfile
        WorkoutExercises = workout.WorkoutExercises
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

// Create new workout

// {
//     "userProfileId": 1,
//     "workoutCompletedOn": "2024-03-05T12:02:51.133955",
//     "workoutExercises": [
//         {
//             "exerciseId": 1
//         },
//         {
//             "exerciseId": 2
//         }
//     ]
// }

    [HttpPost]
    // [Authorize]
    public IActionResult CreateWorkout(Workout workout)
    {
        try
        {
            _dbContext.Workouts.Add(workout);
            _dbContext.SaveChanges();

            return Created($"/api/workout/{workout.Id}", workout);
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