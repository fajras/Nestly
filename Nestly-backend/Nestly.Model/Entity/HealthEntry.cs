using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class HealthEntry
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(BabyProfile))]
        public long BabyId { get; set; }
        public DateTime EntryDate { get; set; }
        public decimal? TemperatureC { get; set; }
        public string Medicines { get; set; }
        public string DoctorVisit { get; set; }


        public BabyProfile Baby { get; set; }
    }
}
