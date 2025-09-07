using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;

namespace Nestly.Services.Data
{
    public class NestlyDbContext : DbContext
    {
        public NestlyDbContext(DbContextOptions<NestlyDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<BabyProfile> BabyProfiles { get; set; }
        public DbSet<BabyGrowth> BabyGrowths { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogPostCategory> BlogPostCategories { get; set; }
        public DbSet<CalendarEvent> CalendarEvents { get; set; }
        public DbSet<ChatMember> ChatMembers { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<DiaperLog> DiaperLogs { get; set; }
        public DbSet<FeedingLog> FeedingLogs { get; set; }
        public DbSet<FetalDevelopmentWeek> FetalDevelopmentWeeks { get; set; }
        public DbSet<FoodType> FoodTypes { get; set; }
        public DbSet<HealthEntry> HealthEntries { get; set; }
        public DbSet<MealPlan> MealPlans { get; set; }
        public DbSet<MedicationIntakeLog> MedicationIntakeLogs { get; set; }
        public DbSet<MedicationPlan> MedicationPlans { get; set; }
        public DbSet<MedicationScheduleTime> MedicationScheduleTimes { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<Pregnancy> Pregnancies { get; set; }
        public DbSet<QaAnswer> QaAnswers { get; set; }
        public DbSet<QaQuestion> QaQuestions { get; set; }
        public DbSet<SleepLog> SleepLogs { get; set; }
        public DbSet<WeeklyAdvice> WeeklyAdvices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureAppUser(modelBuilder);
            ConfigureBabyProfile(modelBuilder);
            ConfigureDiaperLog(modelBuilder);
            ConfigureSleepLog(modelBuilder);
            ConfigureHealthEntry(modelBuilder);
            ConfigureMilestone(modelBuilder);
            ConfigureBabyGrowth(modelBuilder);
            ConfigureMealPlan(modelBuilder);
            ConfigureCalendarEvent(modelBuilder);
            ConfigurePregnancy(modelBuilder);
            ConfigureFetalDevelopmentWeek(modelBuilder);
            ConfigureWeeklyAdvice(modelBuilder);
            ConfigureBlogPost(modelBuilder);
            ConfigureQa(modelBuilder);
            ConfigureMedicationPlan(modelBuilder);
        }

        private void ConfigureAppUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
            });
        }

        private void ConfigureBabyProfile(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BabyProfile>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.BabyName).HasMaxLength(150);
                entity.Property(e => e.Gender).HasMaxLength(20);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Babies)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureDiaperLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiaperLog>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.DiaperState).HasMaxLength(30);
                entity.Property(e => e.Notes).HasMaxLength(1000);

                entity.Property(e => e.ChangeTime).HasConversion<TimeSpan>();

                entity.HasOne(e => e.Baby)
                    .WithMany(b => b.DiaperLogs)
                    .HasForeignKey(e => e.BabyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.BabyId, e.ChangeDate });
            });
        }

        private void ConfigureSleepLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SleepLog>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Notes).HasMaxLength(1000);

                entity.HasOne(e => e.Baby)
                    .WithMany(b => b.SleepLogs)
                    .HasForeignKey(e => e.BabyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.BabyId, e.SleepDate });
            });
        }
        private void ConfigureHealthEntry(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HealthEntry>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Medicines).HasMaxLength(1000);
                entity.Property(e => e.DoctorVisit).HasMaxLength(500);

                entity.Property(e => e.TemperatureC).HasPrecision(4, 2);

                entity.HasOne(e => e.Baby)
                    .WithMany(b => b.HealthEntries)
                    .HasForeignKey(e => e.BabyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.BabyId, e.EntryDate });
            });
        }
        private void ConfigureMilestone(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Milestone>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title).HasMaxLength(200);
                entity.Property(e => e.Notes).HasMaxLength(1000);

                entity.HasOne(e => e.Baby)
                    .WithMany(b => b.Milestones)
                    .HasForeignKey(e => e.BabyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.BabyId, e.AchievedDate });
            });
        }
        private void ConfigureBabyGrowth(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BabyGrowth>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.WeekNumber).IsRequired();

                entity.Property(e => e.WeightKg).HasPrecision(5, 2);
                entity.Property(e => e.HeightCm).HasPrecision(5, 2);
                entity.Property(e => e.HeadCircumferenceCm).HasPrecision(5, 2);

                entity.HasOne(e => e.Baby)
                    .WithMany(b => b.Growths)
                    .HasForeignKey(e => e.BabyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.BabyId, e.WeekNumber }).IsUnique();
            });
        }
        private void ConfigureMealPlan(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MealPlan>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FoodItem).HasMaxLength(200);
                entity.Property(e => e.FoodRating).HasDefaultValue(null);

                entity.HasOne(e => e.Baby)
                    .WithMany(b => b.MealPlans)
                    .HasForeignKey(e => e.BabyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.BabyId, e.WeekNumber });
            });
        }
        private void ConfigureCalendarEvent(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalendarEvent>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title).HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.HasOne(e => e.Baby)
                    .WithMany(b => b.CalendarEvents)
                    .HasForeignKey(e => e.BabyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.BabyId, e.StartAt });
            });
        }

        private void ConfigurePregnancy(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pregnancy>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Pregnancies)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.LmpDate).IsRequired(false);
                entity.Property(e => e.DueDate).IsRequired(false);

                entity.HasIndex(e => new { e.UserId, e.CreatedAt });
            });
        }
        private void ConfigureFetalDevelopmentWeek(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FetalDevelopmentWeek>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.WeekNumber).IsRequired();
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                entity.Property(e => e.BabyDevelopment).HasMaxLength(5000);
                entity.Property(e => e.MotherChanges).HasMaxLength(5000);

                entity.HasIndex(e => e.WeekNumber).IsUnique();
            });
        }
        private void ConfigureWeeklyAdvice(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeeklyAdvice>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.AdviceText).HasMaxLength(4000);

                entity.HasIndex(e => e.WeekNumber).IsUnique();
            });
        }
        private void ConfigureBlogPost(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPostCategory>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.CategoryId });

                entity.HasOne(e => e.Post)
                      .WithMany(p => p.BlogPostCategories)
                      .HasForeignKey(e => e.PostId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.BlogPostCategories)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        }
        private void ConfigureQa(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QaQuestion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.QuestionText).IsRequired().HasMaxLength(2000);

                entity.HasIndex(e => new { e.AskedByUserId, e.CreatedAt });
            });

            modelBuilder.Entity<QaAnswer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AnswerText).IsRequired().HasMaxLength(4000);

                entity.HasOne(e => e.Question)
                      .WithMany(q => q.Answers)
                      .HasForeignKey(e => e.QuestionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.QuestionId, e.CreatedAt });
            });
        }
        private void ConfigureMedicationPlan(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicationPlan>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.MedicineName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Dose).IsRequired().HasMaxLength(100);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.MedicationPlans)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Times)
                      .WithOne(t => t.Plan)
                      .HasForeignKey(t => t.PlanId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.IntakeLogs)
                      .WithOne(l => l.Plan)
                      .HasForeignKey(l => l.PlanId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserId, e.MedicineName });
            });
        }



    }
}
