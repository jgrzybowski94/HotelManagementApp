using HotelManagement.Helpers;

namespace HotelManagement.Dtos
{
    public class HotelAvailabilityDto(DateTime dateFrom, DateTime dateTo, int availability)
    {
        public DateTime DateFrom { get; } = dateFrom;
        public DateTime DateTo { get; } = dateTo;
        public int Availability { get; } = availability;

        public override string ToString()
        {
            return $"({BookingDateConverter.ConvertDate(DateFrom)}-{BookingDateConverter.ConvertDate(DateTo)},{Availability})";
        }
    }
}
