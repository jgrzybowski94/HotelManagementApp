using HotelManagement.Exceptions;
using System.Globalization;

namespace HotelManagement.Helpers
{
    public static class BookingDateConverter
    {
        private const string _dateFormat = "yyyyMMdd";

        public static DateTime ConvertDate(string? date)
        {
            if(DateTime.TryParseExact(date, _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var convertedDate))
            {
                return convertedDate;
            }

            throw new BadDateFormatException();
        }

        public static string ConvertDate(DateTime date)
        {
            return date.ToString(_dateFormat);
        }
    }
}
