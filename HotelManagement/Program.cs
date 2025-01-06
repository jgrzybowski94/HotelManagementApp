using HotelManagement.Constants;
using HotelManagement.Repositories;
using HotelManagement.Services;
using HotelManagement.Validators;
using System.Text;

var arguments = Environment.GetCommandLineArgs();

var configurationProvider = new HotelManagementConfigurationProvider();
var configuration = configurationProvider.ParseConfiguration(arguments);

var hotelRepository = new HotelFileRepository(configuration.HotelsFilePath);
var bookingsRepository = new BookingFileRepository(configuration.BookingsFilePath);

var hotelService = new HotelService(hotelRepository, bookingsRepository);

Console.WriteLine("Input command. ");
Console.WriteLine("Example commands: ");
Console.WriteLine("Availability(H1, 20240901, SGL)");
Console.WriteLine("Search(H1, 365, SGL)");

while (true)
{

    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) break;

    var command = input.Split('(')[0].Trim();
    var parameters = input.Split('(')[1].Trim(')').Split(',');

    if (command == Commands.Availability)
    {
        var hotelId = parameters[0].Trim();
        var dateRange = parameters[1].Trim();
        var roomType = parameters[2].Trim();

        var availability = await hotelService.GetAvailabilityAsync(hotelId, dateRange, roomType);

        Console.WriteLine($"Hotel {hotelId} has {availability} {roomType} rooms available in {dateRange}");
    }

    if(command == Commands.Search)
    {
        var hotelId = parameters[0].Trim();
        var dateRange = int.Parse(parameters[1].Trim());
        var roomType = parameters[2].Trim();

        var availabilities = await hotelService.SearchAvailabilitiesAsync(hotelId, dateRange, roomType);
        if (!availabilities.Any())
        {
            Console.WriteLine();
        }
        else
        {
            var sb = new StringBuilder();
            foreach (var availability in availabilities)
            {
                sb.Append(availability.ToString());
                sb.Append(',');
            }
            sb.Remove(sb.Length - 1, 1);

            Console.WriteLine(sb.ToString());
        }
    }

}
