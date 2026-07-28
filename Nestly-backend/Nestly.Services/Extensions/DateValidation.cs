namespace Nestly.Services.Extensions
{
    // Requests only ever carry a plain date/time with no client timezone
    // attached, so "future" is checked against UTC. Bosnia (and most of the
    // app's target audience) is UTC+1/+2, so right after local midnight the
    // user's "today" is still UTC's "yesterday" - without slack, every log
    // entry dated "today" gets incorrectly rejected as being in the future
    // for a few hours every single day. One day of tolerance covers any
    // timezone ahead of UTC.
    public static class DateValidation
    {
        public static bool IsFutureDate(DateTime date)
        {
            return date.Date > DateTime.UtcNow.Date.AddDays(1);
        }

        public static bool IsFutureDateTime(DateTime dateTime)
        {
            return dateTime > DateTime.UtcNow.AddDays(1);
        }
    }
}
