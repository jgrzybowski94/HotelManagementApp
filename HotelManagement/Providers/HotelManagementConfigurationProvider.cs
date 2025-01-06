using HotelManagement.Dtos;

namespace HotelManagement.Validators
{
    public class HotelManagementConfigurationProvider
    {
        public HotelManagementConfigurationDto ParseConfiguration(string[] arguments)
        {
            if (arguments.Length < 4)
            {
                throw new ArgumentException("Incorrect number of arguments. Please provide paths for bookings.json and hotels.json");
            }

            int hotelsArgumentIndex = Array.IndexOf(arguments, "--hotels");
            int bookingsArgumentIndex = Array.IndexOf(arguments, "--bookings");


            if (hotelsArgumentIndex == -1 || bookingsArgumentIndex == -1)
            {
                throw new ArgumentException("Please provide arguments for hotels.json and bookings.json paths. Sample input: HotelManagement --hotels hotels.json --bookings bookings.json");
            }
            if (!File.Exists(arguments[hotelsArgumentIndex+1]))
            {
                throw new ArgumentException($"Hotels file not found: {arguments[1]}");
            }
            if (!File.Exists(arguments[bookingsArgumentIndex+1]))
            {
                throw new ArgumentException($"Bookings file not found: {arguments[3]}");
            }
            else
            {
                return new HotelManagementConfigurationDto(arguments[hotelsArgumentIndex + 1], arguments[bookingsArgumentIndex + 1]);
            }
        }
    }
}
