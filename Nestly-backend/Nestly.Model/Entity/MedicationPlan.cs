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
        public long UserId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string MedicineName { get; set; }
        public string Dose { get; set; }


        public ParentProfile User { get; set; }
        public ICollection<MedicationScheduleTime> Times { get; set; }
        public ICollection<MedicationIntakeLog> IntakeLogs { get; set; }
    }
}
