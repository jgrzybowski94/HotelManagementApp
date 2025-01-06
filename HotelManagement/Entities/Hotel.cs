using System.Text.Json.Serialization;

namespace HotelManagement.Entities
{
    [method: JsonConstructor]
    public class Hotel(string id, string name, IEnumerable<HotelRoomType> roomTypes, IEnumerable<HotelRoom> rooms)
    {
        public string Id { get; } = id;
        public string Name { get; } = name;
        public IEnumerable<HotelRoomType> RoomTypes { get; } = roomTypes;
        public IEnumerable<HotelRoom> Rooms { get; } = rooms;

    }

    [method: JsonConstructor]
    public class HotelRoom(string roomId, string roomType)
    {
        public string RoomId { get; } = roomId;
        public string RoomType { get; } = roomType;
    }

    [method: JsonConstructor]
    public class HotelRoomType(string code, string description, IEnumerable<string> amenities, IEnumerable<string> features)
    {
        public string Code { get; } = code;
        public string Description { get; } = description;
        public IEnumerable<string> Amenities { get; } = amenities;
        public IEnumerable<string> Features { get; } = features;
    }


}
