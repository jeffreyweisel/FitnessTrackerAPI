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
public class UserProfileController : ControllerBase
{
    private FitnessTrackerDbContext _dbContext;

    public UserProfileController(FitnessTrackerDbContext context)
    {
        _dbContext = context;
    }

    // Get all users with workouts included
    public IActionResult Get()
    {
        return Ok(_dbContext
            .UserProfiles
            .Include(up => up.IdentityUser)
            .Include(up => up.Workouts)
                .ThenInclude(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)  // Include exercises for each workout
            .Select(up => new UserProfileDTO
            {
                Id = up.Id,
                FirstName = up.FirstName,
                LastName = up.LastName,
                IdentityUserId = up.IdentityUserId,
                Email = up.IdentityUser.Email,
                UserName = up.IdentityUser.UserName,
                ProfilePicture = up.ProfilePicture,
                Age = 21,
                Height = "5'9",
                Weight = 184.2M,
                GeneralGoal = "Get stronger and leaner",
                GoalWeightInPounds = 172,
                GoalBenchMaxInPounds = 305,
                GoalDeadliftMaxInPounds = 385,
                GoalSquatMaxInPounds = 315,
                MaxBench = 285,
                MaxDeadlift = 305,
                MaxSquat = 225,
                Workouts = up.Workouts
                    .Select(w => new WorkoutDTO
                    {
                        Id = w.Id,
                        UserProfileId = w.UserProfileId,
                        WorkoutCompletedOn = w.WorkoutCompletedOn,
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
                    .ToList()
            })
            .ToList());
    }

    // Get users with roles
    [HttpGet("withroles")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetWithRoles()
    {
        return Ok(_dbContext.UserProfiles
        .Include(up => up.IdentityUser)
        .Select(up => new UserProfileDTO
        {
            Id = up.Id,
            FirstName = up.FirstName,
            LastName = up.LastName,
            Email = up.IdentityUser.Email,
            UserName = up.IdentityUser.UserName,
            ProfilePicture = up.ProfilePicture,
            IdentityUserId = up.IdentityUserId,
            Roles = _dbContext.UserRoles
            .Where(ur => ur.UserId == up.IdentityUserId)
            .Select(ur => _dbContext.Roles.SingleOrDefault(r => r.Id == ur.RoleId).Name)
            .ToList()
        }));
    }


    //Get by Id with workouts included
    [HttpGet("{id}")]
    // [Authorize]
    public IActionResult GetUserProfileById(int id)
    {
        UserProfile userProfile = _dbContext
            .UserProfiles
            .Include(p => p.IdentityUser)
            .Include(p => p.Workouts)
                .ThenInclude(h => h.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
            .SingleOrDefault(up => up.Id == id);

        if (userProfile == null)
        {
            return NotFound();
        }

        return Ok(new UserProfileDTO
        {
            Id = userProfile.Id,
            FirstName = userProfile.FirstName,
            LastName = userProfile.LastName,
            IdentityUserId = userProfile.IdentityUserId,
            Email = userProfile.IdentityUser.Email,
            UserName = userProfile.IdentityUser.UserName,
            ProfilePicture = userProfile.ProfilePicture,
            Age = userProfile.Age,
            Height = userProfile.Height,
            Weight = userProfile.Weight,
            GeneralGoal = userProfile.GeneralGoal,
            GoalWeightInPounds = userProfile.GoalWeightInPounds,
            GoalBenchMaxInPounds = userProfile.GoalBenchMaxInPounds,
            GoalDeadliftMaxInPounds = userProfile.GoalDeadliftMaxInPounds,
            GoalSquatMaxInPounds = userProfile.GoalSquatMaxInPounds,
            MaxBench = userProfile.MaxBench,
            MaxDeadlift = userProfile.MaxDeadlift,
            MaxSquat = userProfile.MaxSquat,
            Workouts = userProfile.Workouts
                .Select(w => new WorkoutDTO
                {
                    Id = w.Id,
                    UserProfileId = w.UserProfileId,
                    WorkoutCompletedOn = w.WorkoutCompletedOn,
                    WorkoutExercises = w.WorkoutExercises
                        .Where(we => we.Exercise != null)
                        .Select(we => new WorkoutExerciseDTO
                        {
                            Id = we.Id,
                            ExerciseId = we.ExerciseId,
                            Exercise = new ExerciseDTO
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
                                    : null
                            }
                        })
                        .ToList()
                })
                .ToList()
        });
    }


}