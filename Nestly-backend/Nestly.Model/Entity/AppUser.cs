using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nestly.Model.Entity
{
    public class AppUser
    {
        [Key] public long Id { get; set; }

        [Required, MaxLength(64)]
        public string IdentityUserId { get; set; } = default!;

        [Required, MaxLength(255)]
        public string Email { get; set; } = default!;

        [MaxLength(150)]
        public string FirstName { get; set; } = default!;

        [MaxLength(150)]
        public string LastName { get; set; } = default!;

        [MaxLength(50)]
        public string PhoneNumber { get; set; } = default!;

        public DateTime DateOfBirth { get; set; }

        [MaxLength(20)]
        public string Gender { get; set; } = default!;

        [ForeignKey(nameof(Role))]
        public long RoleId { get; set; }
        public Role Role { get; set; } = default!;

        public ParentProfile? ParentProfile { get; set; }
        public DoctorProfile? DoctorProfile { get; set; }
    }

    public class ParentProfile
    {
        [Key] public long Id { get; set; }
        public DateTime ConceptionDate { get; set; }

        [ForeignKey(nameof(AppUser))]
        public long UserId { get; set; }

        [JsonIgnore] public AppUser User { get; set; } = default!;
        public ICollection<BabyProfile>? Babies { get; set; } = new List<BabyProfile>();
        public ICollection<Pregnancy>? Pregnancies { get; set; } = new List<Pregnancy>();
        public ICollection<MedicationPlan>? MedicationPlans { get; set; } = new List<MedicationPlan>();
        public ICollection<QaQuestion>? QuestionsAsked { get; set; } = new List<QaQuestion>();
        public ICollection<CalendarEvent>? CalendarEvents { get; set; } = new List<CalendarEvent>();
        public ICollection<ChatRoom>? ChatRooms { get; set; } = new List<ChatRoom>();
    }

    public class DoctorProfile
    {
        [Key] public long Id { get; set; }

        [ForeignKey(nameof(AppUser))]
        public long UserId { get; set; }

        [JsonIgnore] public AppUser User { get; set; } = default!;
        public ICollection<QaAnswer>? QaAnswers { get; set; } = new List<QaAnswer>();
        public ICollection<BlogPost>? BlogPosts { get; set; } = new List<BlogPost>();
    }
}
