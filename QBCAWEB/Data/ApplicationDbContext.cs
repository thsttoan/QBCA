// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using QBCAWEB.Models; // Ensure this namespace is correct for all your models

namespace QBCAWEB.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Existing DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        // DbSets for Subject, CLO, Question, Difficulty
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<CLO> CLOs { get; set; }
        public DbSet<QuestionDifficulty> QuestionDifficulties { get; set; }
        public DbSet<Question> Questions { get; set; }

        // DbSets for ExamPlan and Assignments
        public DbSet<ExamPlan> ExamPlans { get; set; } // <<< THÊM DÒNG NÀY
        public DbSet<SubjectPlanAssignment> SubjectPlanAssignments { get; set; } // <<< THÊM DÒNG NÀY


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.UserID);
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasOne(d => d.UserRole)
                      .WithMany(r => r.Users)
                      .HasForeignKey(d => d.RoleID)
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // Role Configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(e => e.RoleID);
                entity.HasIndex(e => e.RoleName).IsUnique();
                entity.Property(e => e.RoleName).HasMaxLength(100);
            });

            // Subject Configuration
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subjects");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.SubjectCode)
                      .HasMaxLength(20);

                entity.HasMany(s => s.CLOs)
                      .WithOne(c => c.Subject)
                      .HasForeignKey(c => c.SubjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(s => s.Questions)
                      .WithOne(q => q.Subject)
                      .HasForeignKey(q => q.SubjectId)
                      .OnDelete(DeleteBehavior.Cascade); // Or Restrict if questions should not be deleted with subject

                // One-to-Many: Subject to SubjectPlanAssignments (if Subject model has PlanAssignments collection)
                entity.HasMany(s => s.PlanAssignments) // Assumes Subject.cs has ICollection<SubjectPlanAssignment> PlanAssignments
                      .WithOne(sa => sa.Subject)
                      .HasForeignKey(sa => sa.SubjectId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a subject if it's part of any plan assignment
            });

            // CLO Configuration
            modelBuilder.Entity<CLO>(entity =>
            {
                entity.ToTable("CLOs");
                entity.HasKey(e => e.CLOID);
                entity.Property(e => e.Description)
                      .IsRequired()
                      .HasMaxLength(500);
            });

            // QuestionDifficulty Configuration
            modelBuilder.Entity<QuestionDifficulty>(entity =>
            {
                entity.ToTable("QuestionDifficulties");
                entity.HasKey(e => e.DifficultyID);
                entity.Property(e => e.LevelName)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(e => e.Description)
                      .HasMaxLength(200);
                entity.HasMany(qd => qd.Questions)
                      .WithOne(q => q.Difficulty)
                      .HasForeignKey(q => q.DifficultyID)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Question Configuration
            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Questions");
                entity.HasKey(e => e.QuestionID);
                // Add other configurations for Question properties if needed
            });

            // >>> PHẦN THÊM MỚI CHO EXAM PLAN VÀ ASSIGNMENT <<<
            // ExamPlan Configuration
            modelBuilder.Entity<ExamPlan>(entity =>
            {
                entity.ToTable("ExamPlans");
                entity.HasKey(e => e.PlanID);

                entity.Property(e => e.PlanName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Status)
                      .HasConversion<string>() // Store enum as string in DB
                      .HasMaxLength(20);       // Ensure length matches longest enum string value

                // One-to-Many: ExamPlan to SubjectPlanAssignments
                entity.HasMany(p => p.SubjectAssignments)
                      .WithOne(sa => sa.ExamPlan)
                      .HasForeignKey(sa => sa.ExamPlanID)
                      .OnDelete(DeleteBehavior.Cascade); // If a plan is deleted, its assignments are also deleted
            });

            // SubjectPlanAssignment Configuration
            modelBuilder.Entity<SubjectPlanAssignment>(entity =>
            {
                entity.ToTable("SubjectPlanAssignments");
                entity.HasKey(e => e.AssignmentID);

                // Optional: Composite unique key to prevent assigning the same subject multiple times to the same plan
                // Consider the implications for editing if you enable this.
                // entity.HasIndex(sa => new { sa.ExamPlanID, sa.SubjectId }).IsUnique();

                // Relationships with ExamPlan and Subject are established via ForeignKey attributes in the model
                // and the HasMany configurations on the ExamPlan and Subject entities.
            });
            // >>> KẾT THÚC PHẦN THÊM MỚI <<<
        }
    }
}