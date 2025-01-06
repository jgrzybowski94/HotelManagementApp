using HotelManagement.Entities;
using HotelManagement.Interfaces;
using HotelManagement.Services;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace HotelManagement.Tests
{
    public class HotelServiceTests
    {
        private readonly IHotelRepository _hotelRepositoryMock;
        private readonly IBookingRepository _bookingRepositoryMock;
        private readonly HotelService _hotelService;

        public HotelServiceTests()
        {
            _hotelRepositoryMock = Substitute.For<IHotelRepository>();
            _bookingRepositoryMock = Substitute.For<IBookingRepository>();
            _hotelService = new HotelService(_hotelRepositoryMock, _bookingRepositoryMock);
        }

        [Fact]
        public async Task GetAvailabilityAsync_ShouldThrowException_WhenHotelNotFound()
        {
            _hotelRepositoryMock.GetHotelAsync(Arg.Any<string>()).ReturnsNullForAnyArgs();

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _hotelService.GetAvailabilityAsync("H1", "20240101-20240102", "DBL"));

            Assert.Equal("Hotel H1 not found", ex.Message);
        }

        [Fact]
        public async Task GetAvailabilityAsync_ShouldReturnZero_WhenNoRoomsOfSpecifiedType()
        {
            var hotel = new Hotel(
                "H1",
                "Hotel California",
                [
                new HotelRoomType("SGL", "Single Room", ["WiFi"], ["Non-Smoking"])
                ],
                [
                new HotelRoom("101", "SGL")
                ]
            );

            _hotelRepositoryMock.GetHotelAsync("H1").Returns(hotel);
            _bookingRepositoryMock.GetBookingsInDateRangeAndRoomTypeAsync("H1", "DBL", Arg.Any<DateTime>(), Arg.Any<DateTime>())
                .Returns([]);

            var result = await _hotelService.GetAvailabilityAsync("H1", "20240101-20240102", "DBL");

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task GetAvailabilityAsync_ShouldReturnCorrectAvailability()
        {
            var currentDate = DateTime.Today;

            var hotel = new Hotel(
                "H1",
                "Hotel California",
                [
                new HotelRoomType("DBL", "Double Room", ["WiFi"], ["Non-Smoking"])
                ],
                [
                new HotelRoom("101", "DBL"),
                new HotelRoom("102", "DBL")
                ]
            );

            var bookings = new List<Booking>
            {
                new("H1", currentDate, currentDate.AddDays(3), "DBL", "Standard")
            };

            _hotelRepositoryMock.GetHotelAsync("H1").Returns(hotel);
            _bookingRepositoryMock.GetBookingsInDateRangeAndRoomTypeAsync("H1", "DBL", Arg.Any<DateTime>(), Arg.Any<DateTime>())
                .Returns(bookings);

            var result = await _hotelService.GetAvailabilityAsync("H1", "20240101-20240102", "DBL");

            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetAvailabilityAsync_ShouldReturnNegativeAvailability_WhenOverbooked()
        {
            var currentDate = DateTime.Today;

            var hotel = new Hotel(
                "H1",
                "Hotel California",
                [
                new HotelRoomType("DBL", "Double Room", ["WiFi"], ["Non-Smoking"])
                ],
                [
                new HotelRoom("101", "DBL"),
                new HotelRoom("102", "DBL")
                ]
            );

            var bookings = new List<Booking>
        {
            new("H1", currentDate, currentDate.AddDays(2), "DBL", "Standard"),
            new("H1", currentDate, currentDate.AddDays(2), "DBL", "Standard"),
            new("H1", currentDate, currentDate.AddDays(2), "DBL", "Standard")
        };

            _hotelRepositoryMock.GetHotelAsync("H1").Returns(hotel);
            _bookingRepositoryMock.GetBookingsInDateRangeAndRoomTypeAsync("H1", "DBL", Arg.Any<DateTime>(), Arg.Any<DateTime>())
                .Returns(bookings);

            var result = await _hotelService.GetAvailabilityAsync("H1", "20240102-20240102", "DBL");

            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task SearchAvailabilitiesAsync_ShouldThrowException_WhenHotelNotFound()
        {
            _hotelRepositoryMock.GetHotelAsync(Arg.Any<string>()).ReturnsNullForAnyArgs();

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _hotelService.SearchAvailabilitiesAsync("H1", 10, "DBL"));

            Assert.Equal("Hotel H1 not found", ex.Message);
        }

        [Fact]
        public async Task SearchAvailabilitiesAsync_ShouldReturnEmpty_WhenNoAvailabilitiesInGivenRange()
        {
            var currentDate = DateTime.Today;

            var hotel = new Hotel(
                "H1",
                "Hotel California",
                [
                new HotelRoomType("DBL", "Double Room", ["WiFi"], ["Non-Smoking"])
                ],
                [
                new HotelRoom("101", "DBL"),
                new HotelRoom("102", "DBL")
                ]
            );

            var bookings = new List<Booking>
            {
                new("H1", currentDate, currentDate.AddDays(11), "DBL", "Standard"),
                new("H1", currentDate, currentDate.AddDays(11), "DBL", "Standard"),
            };

            _hotelRepositoryMock.GetHotelAsync("H1").Returns(hotel);
            _bookingRepositoryMock.GetBookingsInDateRangeAndRoomTypeAsync("H1", "DBL", Arg.Any<DateTime>(), Arg.Any<DateTime>())
                .Returns(bookings);

            var result = await _hotelService.SearchAvailabilitiesAsync("H1", 10, "DBL");

            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchAvailabilitiesAsync_ShouldReturnCorrectRanges()
        {

            var currentDate = DateTime.Today;

            var hotel = new Hotel(
                "H1",
                "Hotel California",
                [
                new("DBL", "Double Room", new[] { "WiFi" }, new[] { "Non-Smoking" })
                ],
                [
                new HotelRoom("101", "DBL"),
                new HotelRoom("102", "DBL")
                ]
            );

            var bookings = new List<Booking>
        {
            new("H1", currentDate, currentDate.AddDays(3), "DBL", "Standard")
        };

            _hotelRepositoryMock.GetHotelAsync("H1").Returns(hotel);
            _bookingRepositoryMock.GetBookingsInDateRangeAndRoomTypeAsync("H1", "DBL", Arg.Any<DateTime>(), Arg.Any<DateTime>())
                .Returns(bookings);

            var result = await _hotelService.SearchAvailabilitiesAsync("H1", 5, "DBL");

            var availabilityList = result.ToList();
            Assert.Equal(2, availabilityList.Count);

            Assert.Equal(currentDate, availabilityList[0].DateFrom);
            Assert.Equal(currentDate.AddDays(2), availabilityList[0].DateTo);
            Assert.Equal(1, availabilityList[0].Availability);

            Assert.Equal(currentDate.AddDays(3), availabilityList[1].DateFrom);
            Assert.Equal(currentDate.AddDays(5), availabilityList[1].DateTo);
            Assert.Equal(2, availabilityList[1].Availability);
        }
    }
}