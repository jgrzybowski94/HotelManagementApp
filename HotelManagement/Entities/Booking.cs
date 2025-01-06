using HotelManagement.Helpers;
using System.Text.Json.Serialization;

namespace HotelManagement.Entities
{
    [method: JsonConstructor]
    public class Booking(string hotelId, DateTime arrival, DateTime departure, string roomType, string roomRate)
    {
        public string HotelId { get; } = hotelId;
        [JsonConverter(typeof(BookingDateJsonConverter))]
        public DateTime Arrival { get; } = arrival;
        [JsonConverter(typeof(BookingDateJsonConverter))]
        public DateTime Departure { get; } = departure;
        public string RoomType { get; } = roomType;
        public string RoomRate { get; } = roomRate;
    }
}
