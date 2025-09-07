using System;

namespace Nestly.Model.SearchObjects
{
    public class MedicationPlanSearchObject
    {
        public long? UserId { get; set; }
        public string? MedicineName { get; set; }
        public DateTime? ActiveOn { get; set; }
    }
}
