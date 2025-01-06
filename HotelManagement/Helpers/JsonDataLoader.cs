using System.Text.Json;

namespace HotelManagement.Helpers
{
    public static class JsonDataLoader
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static async Task<T> LoadData<T>(string path)
        {
            var content = await File.ReadAllTextAsync(path);
            var data = JsonSerializer.Deserialize<T>(content, _options) ?? 
                throw new ArgumentException($"File {path} is not correct.");

            return data;
        }
    }
}
