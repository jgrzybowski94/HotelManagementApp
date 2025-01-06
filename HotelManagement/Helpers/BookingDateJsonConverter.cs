using System.Text.Json;
using System.Text.Json.Serialization;

namespace HotelManagement.Helpers
{
    public class BookingDateJsonConverter : JsonConverter<DateTime>
    {

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateString = reader.GetString();
            return BookingDateConverter.ConvertDate(dateString);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(BookingDateConverter.ConvertDate(value));
        }
    }
}
