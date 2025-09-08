using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;

namespace Nestly.Services.Data
{
    public class NestlyDbContext : DbContext
    {
        public NestlyDbContext(DbContextOptions<NestlyDbContext> options) : base(options) { }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<ParentProfile> ParentProfiles { get; set; }
        public DbSet<DoctorProfile> DoctorProfiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<BabyProfile> BabyProfiles { get; set; }
        public DbSet<BabyGrowth> BabyGrowths { get; set; }

        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogPostCategory> BlogPostCategories { get; set; }

        public DbSet<CalendarEvent> CalendarEvents { get; set; }

        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatMember> ChatMembers { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<DiaperLog> DiaperLogs { get; set; }
        public DbSet<FeedingLog> FeedingLogs { get; set; }
        public DbSet<HealthEntry> HealthEntries { get; set; }
        public DbSet<MealPlan> MealPlans { get; set; }
        public DbSet<SleepLog> SleepLogs { get; set; }
        public DbSet<Milestone> Milestones { get; set; }

        public DbSet<FetalDevelopmentWeek> FetalDevelopmentWeeks { get; set; }
        public DbSet<FoodType> FoodTypes { get; set; }
        public DbSet<WeeklyAdvice> WeeklyAdvices { get; set; }

        public DbSet<MedicationPlan> MedicationPlans { get; set; }
        public DbSet<MedicationScheduleTime> MedicationScheduleTimes { get; set; }
        public DbSet<MedicationIntakeLog> MedicationIntakeLogs { get; set; }

        public DbSet<Pregnancy> Pregnancies { get; set; }

        public DbSet<QaQuestion> QaQuestions { get; set; }
        public DbSet<QaAnswer> QaAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUsersAndProfiles(modelBuilder);
            ConfigureRoles(modelBuilder);

            ConfigureBabyProfileAndChildren(modelBuilder);
            ConfigurePregnancy(modelBuilder);

            ConfigureBlog(modelBuilder);

            ConfigureCalendar(modelBuilder);

            ConfigureChat(modelBuilder);

            ConfigureMedication(modelBuilder);

            ConfigureStaticLookups(modelBuilder);

            ConfigureQA(modelBuilder);
        }


        private static void ConfigureUsersAndProfiles(ModelBuilder model)
        {
            model.Entity<AppUser>(e =>
            {
                e.HasKey(x => x.Id);

                e.Property(x => x.Email).IsRequired().HasMaxLength(255);
                e.Property(x => x.Username).IsRequired().HasMaxLength(100);
                e.Property(x => x.FirstName).HasMaxLength(150);
                e.Property(x => x.LastName).HasMaxLength(150);
                e.Property(x => x.PhoneNumber).HasMaxLength(50);
                e.Property(x => x.Gender).HasMaxLength(20);
                e.Property(x => x.Password).IsRequired();

                e.HasIndex(x => x.Email).IsUnique();
                e.HasIndex(x => x.Username).IsUnique();

                e.HasOne(x => x.ParentProfile)
                 .WithOne(p => p.User)
                 .HasForeignKey<ParentProfile>(p => p.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.DoctorProfile)
                 .WithOne(d => d.User)
                 .HasForeignKey<DoctorProfile>(d => d.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            model.Entity<ParentProfile>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => x.UserId).IsUnique();
            });

            model.Entity<DoctorProfile>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => x.UserId).IsUnique();
            });
        }

        private static void ConfigureRoles(ModelBuilder model)
        {
            model.Entity<Role>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(80);
                e.HasIndex(x => x.Name).IsUnique();
            });

            model.Entity<UserRole>(e =>
            {
                e.HasKey(x => new { x.UserId, x.RoleId });
                e.HasOne(x => x.User)
                 .WithMany(u => u.UserRoles)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Role)
                 .WithMany(r => r.UserRoles)
                 .HasForeignKey(x => x.RoleId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }


        private static void ConfigureBabyProfileAndChildren(ModelBuilder model)
        {
            model.Entity<BabyProfile>(e =>
            {
                e.HasKey(x => x.Id);

                e.Property(x => x.BabyName).HasMaxLength(150);
                e.Property(x => x.Gender).HasMaxLength(20);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                e.HasOne(x => x.ParentProfile)
                 .WithMany(p => p.Babies)
                 .HasForeignKey(x => x.ParentProfileId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Pregnancy)
                 .WithMany()
                 .HasForeignKey(x => x.PregnancyId)
                 .OnDelete(DeleteBehavior.SetNull);

                e.HasIndex(x => new { x.ParentProfileId, x.BirthDate });
            });

            model.Entity<BabyGrowth>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.WeekNumber).IsRequired();
                e.Property(x => x.WeightKg).HasPrecision(5, 2);
                e.Property(x => x.HeightCm).HasPrecision(5, 2);
                e.Property(x => x.HeadCircumferenceCm).HasPrecision(5, 2);

                e.HasOne(x => x.Baby)
                 .WithMany(b => b.Growths)
                 .HasForeignKey(x => x.BabyId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(x => new { x.BabyId, x.WeekNumber }).IsUnique();
            });

            model.Entity<SleepLog>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Notes).HasMaxLength(1000);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                e.Ignore(x => x.DurationMinutes);

                e.HasOne(x => x.Baby)
                 .WithMany(b => b.SleepLogs)
                 .HasForeignKey(x => x.BabyId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(x => new { x.BabyId, x.SleepDate });
            });

            model.Entity<DiaperLog>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.DiaperState).HasMaxLength(30);
                e.Property(x => x.Notes).HasMaxLength(1000);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                e.HasOne(x => x.Baby)
                 .WithMany(b => b.DiaperLogs)
                 .HasForeignKey(x => x.BabyId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(x => new { x.BabyId, x.ChangeDate });
            });

            model.Entity<HealthEntry>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Medicines).HasMaxLength(1000);
                e.Property(x => x.DoctorVisit).HasMaxLength(500);
                e.Property(x => x.TemperatureC).HasPrecision(4, 2);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                e.HasOne(x => x.Baby)
                 .WithMany(b => b.HealthEntries)
                 .HasForeignKey(x => x.BabyId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(x => new { x.BabyId, x.EntryDate });
            });

            model.Entity<MealPlan>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.FoodItem).HasMaxLength(200);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                e.HasOne(x => x.Baby)
                 .WithMany(b => b.MealPlans)
                 .HasForeignKey(x => x.BabyId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(x => new { x.BabyId, x.WeekNumber });
            });

            model.Entity<FeedingLog>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.AmountMl).HasPrecision(10, 2);
                e.Property(x => x.Notes).HasMaxLength(1000);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                e.HasOne(x => x.Baby)
                 .WithMany(b => b.FeedingLogs)
                 .HasForeignKey(x => x.BabyId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.FoodType)
                 .WithMany()
                 .HasForeignKey(x => x.FoodTypeId)
                 .OnDelete(DeleteBehavior.SetNull);
            });
        }

        private static void ConfigurePregnancy(ModelBuilder model)
        {
            model.Entity<Pregnancy>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                e.HasOne(x => x.User)
                 .WithMany(p => p.Pregnancies)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(x => new { x.UserId, x.CreatedAt });
            });
        }


        private static void ConfigureBlog(ModelBuilder model)
        {
            model.Entity<BlogCategory>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(150);
                e.HasIndex(x => x.Name).IsUnique();
            });

            model.Entity<BlogPost>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Title).IsRequired().HasMaxLength(200);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                e.HasOne(x => x.Author)
                 .WithMany(d => d.BlogPosts)
                 .HasForeignKey(x => x.AuthorId)
                 .OnDelete(DeleteBehavior.SetNull);

                e.HasIndex(x => x.CreatedAt);
            });

            model.Entity<BlogPostCategory>(e =>
            {
                e.HasKey(x => new { x.PostId, x.CategoryId });

                e.HasOne(x => x.Post)
                 .WithMany(p => p.BlogPostCategories)
                 .HasForeignKey(x => x.PostId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Category)
                 .WithMany(c => c.BlogPostCategories)
                 .HasForeignKey(x => x.CategoryId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }


        private static void ConfigureCalendar(ModelBuilder model)
        {
            model.Entity<CalendarEvent>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Title).HasMaxLength(200);
                e.Property(x => x.Description).HasMaxLength(2000);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                e.HasOne(x => x.Baby)
                 .WithMany(b => b.CalendarEvents)
                 .HasForeignKey(x => x.BabyId)
                 .OnDelete(DeleteBehavior.Cascade);

                // UserId -> ParentProfile (nullable)
                e.HasOne(x => x.User)
                 .WithMany(p => p.CalendarEvents)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.SetNull);

                e.HasIndex(x => new { x.BabyId, x.StartAt });
            });
        }


        private static void ConfigureChat(ModelBuilder model)
        {
            model.Entity<ChatRoom>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).HasMaxLength(150);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                e.Property(x => x.IsPrivate).HasDefaultValue(false);
            });

            model.Entity<ChatMember>(e =>
            {
                e.HasKey(x => new { x.RoomId, x.UserId });
                e.Property(x => x.JoinedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                e.HasOne(x => x.Room)
                 .WithMany(r => r.Members)
                 .HasForeignKey(x => x.RoomId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            model.Entity<ChatMessage>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Content).IsRequired();
                e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                e.HasOne(x => x.Room)
                 .WithMany(r => r.Messages)
                 .HasForeignKey(x => x.RoomId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(x => new { x.RoomId, x.CreatedAt });
            });
        }


        private static void ConfigureMedication(ModelBuilder model)
        {
            model.Entity<MedicationPlan>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.MedicineName).IsRequired().HasMaxLength(200);
                e.Property(x => x.Dose).IsRequired().HasMaxLength(100);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                e.HasOne(x => x.User)
                 .WithMany(p => p.MedicationPlans)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(x => x.Times)
                 .WithOne(t => t.Plan)
                 .HasForeignKey(t => t.PlanId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(x => x.IntakeLogs)
                 .WithOne(l => l.Plan)
                 .HasForeignKey(l => l.PlanId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(x => new { x.UserId, x.MedicineName });
            });

            model.Entity<MedicationScheduleTime>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.IntakeTime).IsRequired();
            });

            model.Entity<MedicationIntakeLog>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.ScheduledDate).IsRequired();
                e.Property(x => x.IntakeTime).IsRequired();
            });
        }


        private static void ConfigureStaticLookups(ModelBuilder model)
        {
            model.Entity<FetalDevelopmentWeek>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.WeekNumber).IsRequired();
                e.Property(x => x.ImageUrl).HasMaxLength(500);
                e.Property(x => x.BabyDevelopment).HasMaxLength(5000);
                e.Property(x => x.MotherChanges).HasMaxLength(5000);
                e.HasIndex(x => x.WeekNumber).IsUnique();
            });

            model.Entity<FoodType>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(120);
                e.HasIndex(x => x.Name).IsUnique();
            });

            model.Entity<WeeklyAdvice>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.AdviceText).HasMaxLength(4000);
                e.HasIndex(x => x.WeekNumber).IsUnique();
            });
        }


        private static void ConfigureQA(ModelBuilder model)
        {
            model.Entity<QaQuestion>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.QuestionText).IsRequired();
                e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                e.HasOne(x => x.AskedBy)
                 .WithMany(p => p.QuestionsAsked)
                 .HasForeignKey(x => x.AskedById)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            model.Entity<QaAnswer>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.AnswerText).IsRequired();
                e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                e.HasOne(x => x.Question)
                 .WithMany(q => q.Answers)
                 .HasForeignKey(x => x.QuestionId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.AnsweredBy)
                 .WithMany(d => d.QaAnswers)
                 .HasForeignKey(x => x.AnsweredById)
                 .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
