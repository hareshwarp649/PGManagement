using TimeZoneConverter;

namespace bca.api.Helpers
{
    public class DateTimeHelper
    {
        public static int CalculateAge(DateTime dob)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dob.Year;

            if (dob > today.AddYears(-age))
            {
                age--; // Adjust if the birthday has not occurred this year
            }

            return age;
        }

        public static DateTime UtcToIst(DateTime dateTime)
        {
            // Mark input as UTC if not already
            DateTime utcDate = dateTime.Kind == DateTimeKind.Utc
                ? dateTime
                : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

            // Get correct timezone ID for current platform
            string istTimeZoneId = TZConvert.WindowsToIana("India Standard Time"); // converts to "Asia/Kolkata" on Linux/macOS

            // Get TimeZoneInfo for IST
            TimeZoneInfo istZone = TZConvert.GetTimeZoneInfo(istTimeZoneId);

            // Convert UTC to IST
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, istZone);

            return istDate;
        }
    }
}
