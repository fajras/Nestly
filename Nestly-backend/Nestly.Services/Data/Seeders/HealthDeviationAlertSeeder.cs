using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class HealthDeviationAlertSeeder
    {
        public static void SeedData(this EntityTypeBuilder<HealthDeviationAlert> entity)
        {
            entity.HasData(
                new HealthDeviationAlert { Id = 5001, BabyId = 3, ParameterType = PediatricParameterType.Growth, Severity = HealthAlertSeverity.Warning, Title = "Usporen rast težine", Message = "Prirast težine bebe Amar u zadnje dvije sedmice je ispod očekivane krivulje rasta za uzrast.", Recommendation = "Preporučuje se kontrola kod pedijatra i praćenje unosa hrane u narednih 7 dana.", DetectedAt = new DateTime(2026, 9, 2, 9, 0, 0), PeriodFrom = new DateTime(2026, 8, 19), PeriodTo = new DateTime(2026, 9, 2), IsResolved = true, ResolvedAt = new DateTime(2026, 9, 4, 11, 0, 0), DoctorFeedbackIsAccurate = true, DoctorFeedbackComment = "Potvrđeno na pregledu, preporučena prilagodba ishrane.", DoctorFeedbackByDoctorId = 1, DoctorFeedbackAt = new DateTime(2026, 9, 4, 11, 0, 0) },
                new HealthDeviationAlert { Id = 5002, BabyId = 5, ParameterType = PediatricParameterType.Sleep, Severity = HealthAlertSeverity.Info, Title = "Promjena obrasca spavanja", Message = "Beba Hana bilježi kraće noćno spavanje u odnosu na prosjek za njen uzrast tokom zadnjih 5 dana.", Recommendation = "Pratite rutinu prije spavanja i javite se doktoru ako se obrazac ne popravi u narednih sedmicu dana.", DetectedAt = new DateTime(2026, 9, 6, 8, 0, 0), PeriodFrom = new DateTime(2026, 9, 1), PeriodTo = new DateTime(2026, 9, 6), IsResolved = false, ResolvedAt = null, DoctorFeedbackIsAccurate = null, DoctorFeedbackComment = null, DoctorFeedbackByDoctorId = null, DoctorFeedbackAt = null }
            );
        }
    }
}
