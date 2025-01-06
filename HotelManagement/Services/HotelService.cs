using HotelManagement.Dtos;
using HotelManagement.Helpers;
using HotelManagement.Interfaces;

namespace HotelManagement.Services
{
    public class HotelService(IHotelRepository hotelRepository, IBookingRepository bookingRepository)
    {
        public async Task<int> GetAvailabilityAsync(string hotelId, string dateRange, string roomType)
        {
            var dates = dateRange.Split('-');

            var startDate = BookingDateConverter.ConvertDate(dates[0]);
            var endDate = new DateTime();
            if (dates.Length < 2)
            {
                endDate = startDate;
            }
            else
            {
                endDate = BookingDateConverter.ConvertDate(dates[1]);
            }

            var hotel = await hotelRepository.GetHotelAsync(hotelId)
                ?? throw new ArgumentException($"Hotel {hotelId} not found");


            var totalRooms = hotel.Rooms.Where(r => string.Equals(r.RoomType, roomType, StringComparison.OrdinalIgnoreCase)).Count();
            if (totalRooms == 0) return 0;

            var bookings = await bookingRepository.GetBookingsInDateRangeAndRoomTypeAsync(
                hotelId,
                roomType,
                startDate,
                endDate);

            var bookedRooms = bookings.Count();

            return totalRooms - bookedRooms;
        }

        public async Task<IEnumerable<HotelAvailabilityDto>> SearchAvailabilitiesAsync(string hotelId, int numberOfDaysToSearch, string roomType)
        {
            var hotel = await hotelRepository.GetHotelAsync(hotelId)
                ?? throw new ArgumentException($"Hotel {hotelId} not found");

            var results = new List<HotelAvailabilityDto>();
            var startDate = DateTime.Now.Date;
            var endDate = startDate.AddDays(numberOfDaysToSearch);

            var bookings = await bookingRepository.GetBookingsInDateRangeAndRoomTypeAsync(hotelId, roomType, startDate, endDate);
            int totalAvailability = hotel.Rooms.Count(r => r.RoomType == roomType);

            DateTime? currentRangeStart = null;
            int? currentAvailability = null;

            for (var date = startDate; date < endDate; date = date.AddDays(1))
            {
                int dateBookingsCount = bookings.Count(b => b.Arrival <= date && b.Departure > date);
                int availability = totalAvailability - dateBookingsCount;

                if (currentAvailability == null || currentAvailability != availability)
                {
                    if (currentRangeStart is not null && currentAvailability is not null)
                    {
                        results.Add(new HotelAvailabilityDto(currentRangeStart.Value, date.AddDays(-1), currentAvailability.Value));
                    }

                    currentRangeStart = date;
                    currentAvailability = availability;
                }
            }

            if (currentRangeStart != null)
            {
                results.Add(new HotelAvailabilityDto(currentRangeStart.Value, endDate, currentAvailability!.Value));
            }

            return results.Where(a => a.Availability > 0);
        }

    }
}

