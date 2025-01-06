using HotelManagement.Entities;

namespace HotelManagement.Interfaces
{
    public interface IHotelRepository
    {
        Task<Hotel?> GetHotelAsync(string hotelId);
    }
}
