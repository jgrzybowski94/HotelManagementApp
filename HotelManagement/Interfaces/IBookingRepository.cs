using HotelManagement.Entities;

namespace HotelManagement.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetBookingsInDateRangeAndRoomTypeAsync(string hotelId, string roomType, DateTime startDate, DateTime endDate);
    }
}
