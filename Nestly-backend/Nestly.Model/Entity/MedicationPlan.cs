using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class MedicationPlan
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey(nameof(ParentProfile))]
        public long ParentProfileId { get; set; }

        public string MedicineName { get; set; } = default!;
        public string Dose { get; set; } = default!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ParentProfile ParentProfile { get; set; } = default!;

        public ICollection<MedicationScheduleTime> Times { get; set; } = new List<MedicationScheduleTime>();
        public ICollection<MedicationIntakeLog> IntakeLogs { get; set; } = new List<MedicationIntakeLog>();
    }
}
