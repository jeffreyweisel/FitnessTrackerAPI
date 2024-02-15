using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace FitnessTracker.Data;
public class FitnessTrackerDbContext : IdentityDbContext<IdentityUser>
{
    private readonly IConfiguration _configuration;
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<ExerciseLog> ExerciseLogs { get; set; }
    public DbSet<Workout> Workouts { get; set; }
    public DbSet<WorkoutType> WorkoutTypes { get; set; }

    public FitnessTrackerDbContext(DbContextOptions<FitnessTrackerDbContext> context, IConfiguration config) : base(context)
    {
        _configuration = config;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
            Name = "Admin",
            NormalizedName = "admin"
        });

        modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
        {
            Id = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
            UserName = "Administrator",
            Email = "jeffrey@example.com",
            PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, _configuration["AdminPassword"])
        });

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
            UserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f"
        });
        modelBuilder.Entity<UserProfile>().HasData(new UserProfile
        {
            Id = 1,
            IdentityUserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
            FirstName = "Jeffrey",
            LastName = "Weisel",
            Age = 21,
            Height = "5'9",
            Weight = 184.2M,
            GeneralGoal = "Get stronger and leaner",
            GoalWeightInPounds = 172,
            GoalBenchMaxInPounds = 305,
            GoalDeadliftMaxInPounds = 385,
            GoalSquatMaxInPounds = 315,
            ProfilePicture = null, // to be updated later
            MaxBench = 285,
            MaxDeadlift = 305,
            MaxSquat = 225
        });
        modelBuilder.Entity<Exercise>().HasData(new Exercise[]
        {
            new Exercise
            {
                Id = 1,
                Name = "Bench Press",
                WorkoutTypeId = 1, // Chest
            },
            new Exercise
            {
                Id = 2,
                Name = "Incline Dumbbell Bench Press",
                WorkoutTypeId = 1 // Chest
            },
            new Exercise
            {
                Id = 3,
                Name = "Squat",
                WorkoutTypeId = 2 // Legs
            },
            new Exercise
            {
                Id = 4,
                Name = "Calf Raises",
                WorkoutTypeId = 2 // Legs
            },
                new Exercise
            {
                Id = 5,
                Name = "Deadlift",
                WorkoutTypeId = 3 // Back
            },
            new Exercise
            {
                Id = 6,
                Name = "Barbell Row",
                WorkoutTypeId = 3 // Back
            },
            new Exercise
            {
                Id = 7,
                Name = "Barbell Curl",
                WorkoutTypeId = 4 // Arms
            },
            new Exercise
            {
                Id = 8,
                Name = "Tricep Extension",
                WorkoutTypeId = 3 // Arms
            }
        });
        modelBuilder.Entity<ExerciseLog>().HasData(new ExerciseLog[]
        {
             new ExerciseLog
            {
                Id = 1,
                ExerciseId = 1, // Bench Press
                Sets = 3,
                Reps = 15
            },
            new ExerciseLog
            {
                Id = 2,
                ExerciseId = 2, // Incline DB Bench
                Sets = 4,
                Reps = 10
            },
            new ExerciseLog
            {
                Id = 3,
                ExerciseId = 3, // Squat
                Sets = 3,
                Reps = 12
            },
            new ExerciseLog
            {
                Id = 4,
                ExerciseId = 4, // Calf Raises
                Sets = 2,
                Reps = 20
            },
            new ExerciseLog
            {
                Id = 5,
                ExerciseId = 5, // Deadlift
                Sets = 3,
                Reps = 8
            }
        });
        modelBuilder.Entity<Workout>().HasData(new Workout[]
        {
        new Workout
        {
            Id = 1,
            UserProfileId = 1,
            WorkoutCompletedOn = DateTime.Now,
        },
        new Workout
        {
            Id = 2,
            UserProfileId = 1,
            WorkoutCompletedOn = DateTime.Now,
        },

        new Workout
        {
        Id = 3,
        UserProfileId = 1,
        WorkoutCompletedOn = DateTime.Now,
        },

        new Workout
        {
            Id = 4,
            UserProfileId = 1,
            WorkoutCompletedOn = DateTime.Now,
        }
        });
        modelBuilder.Entity<WorkoutType>().HasData(new WorkoutType[]
        {
         new WorkoutType
        {
            Id = 1,
            Name = "Chest"
        },
        
        new WorkoutType
        {
            Id = 2,
            Name = "Legs"
        },

        new WorkoutType
        {       
            Id = 3,
            Name = "Back"
        },

        new WorkoutType
        {
            Id = 4,
            Name = "Arms"
        }
          
        });
        modelBuilder.Entity<WorkoutExercise>().HasData(new WorkoutExercise[]
        {
            // Chest day
            new WorkoutExercise { Id = 1, WorkoutId = 1, ExerciseId = 1 },
            new WorkoutExercise { Id = 2, WorkoutId = 1, ExerciseId = 2 },
            // Leg day
            new WorkoutExercise { Id = 3, WorkoutId = 2, ExerciseId = 3 },
            new WorkoutExercise { Id = 4, WorkoutId = 2, ExerciseId = 4 },
            // Back day
            new WorkoutExercise { Id = 5, WorkoutId = 3, ExerciseId = 5 },
            new WorkoutExercise { Id = 6, WorkoutId = 3, ExerciseId = 6 },
            // Arm day
            new WorkoutExercise { Id = 7, WorkoutId = 4, ExerciseId = 7 },
            new WorkoutExercise { Id = 8, WorkoutId = 4, ExerciseId = 8 },
            
        });


    }
}