using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    public class AppUser
    {
        [Key]
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public ICollection<BabyProfile> Babies { get; set; }
        public ICollection<Pregnancy> Pregnancies { get; set; }
        public ICollection<MedicationPlan> MedicationPlans { get; set; }
    }
}
