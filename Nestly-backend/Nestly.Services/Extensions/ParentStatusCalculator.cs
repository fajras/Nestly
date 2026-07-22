namespace Nestly.Services.Extensions
{
    // Shared "is this parent currently a parent, pregnant, or unknown"
    // calculation, previously duplicated independently in AppUserService
    // and ChatService.
    public static class ParentStatusCalculator
    {
        public const int TotalGestationWeeks = 40;

        public static int CalculateBabyAgeInMonths(DateTime birthDate)
        {
            var now = DateTime.UtcNow;

            var months = (now.Year - birthDate.Year) * 12 +
                   now.Month - birthDate.Month;

            // Don't count the current month until the birth day-of-month
            // has actually passed (otherwise age can be off by up to ~4
            // weeks, e.g. a baby born on the 28th shows a month older on
            // the 1st).
            if (now.Day < birthDate.Day)
            {
                months--;
            }

            return Math.Max(0, months);
        }

        public static int CalculatePregnancyTrimester(DateTime dueDate)
        {
            var weeksLeft = (dueDate - DateTime.UtcNow).Days / 7;
            var currentWeek = TotalGestationWeeks - weeksLeft;

            if (currentWeek <= 13)
            {
                return 1;
            }

            if (currentWeek <= 27)
            {
                return 2;
            }

            return 3;
        }

        public static (string Status, int? BabyAgeMonths, int? PregnancyTrimester) Resolve(
            DateTime? latestBabyBirthDate,
            DateTime? latestActivePregnancyDueDate)
        {
            if (latestBabyBirthDate.HasValue)
            {
                return ("PARENT", CalculateBabyAgeInMonths(latestBabyBirthDate.Value), null);
            }

            if (latestActivePregnancyDueDate.HasValue)
            {
                return ("PREGNANT", null, CalculatePregnancyTrimester(latestActivePregnancyDueDate.Value));
            }

            return ("UNKNOWN", null, null);
        }
    }
}
