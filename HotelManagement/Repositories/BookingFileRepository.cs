using HotelManagement.Entities;
using HotelManagement.Helpers;
using HotelManagement.Interfaces;

namespace HotelManagement.Repositories
{
    public class BookingFileRepository(string filePath) : IBookingRepository
    {
        public async Task<IEnumerable<Booking>> GetBookingsInDateRangeAndRoomTypeAsync(string hotelId, string roomType, DateTime startDate, DateTime endDate)
        {
            var bookings = await GetBookingsForHotelAsync(hotelId);
            return bookings.Where(b => b.Arrival <= endDate && b.Departure > startDate && b.RoomType == roomType);
        }

        private async Task<IEnumerable<Booking>> GetBookingsAsync()
        {
            var bookings = await JsonDataLoader.LoadData<IEnumerable<Booking>>(filePath);
            return bookings;
        }

        private async Task<IEnumerable<Booking>> GetBookingsForHotelAsync(string hotelId)
        {
            var bookings = await GetBookingsAsync();
            return bookings.Where(b => b.HotelId == hotelId);
        }
    }
}
