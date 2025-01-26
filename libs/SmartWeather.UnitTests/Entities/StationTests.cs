namespace SmartWeather.UnitTests.Entities.Station
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SmartWeather.Entities.Station;
    using SmartWeather.Entities.Station.Exceptions;

    [TestClass]
    public class StationTests
    {
        [TestMethod]
        public void Constructor_Should_Create_Valid_Station()
        {
            // Arrange
            string name = "Weather Station 1";
            string macAddress = "00:1A:2B:3C:4D:5E";
            float latitude = 45.0f;
            float longitude = 90.0f;
            int? userId = 1;
            StationType type = StationType.Private;

            // Act
            var station = new Station(name, macAddress, latitude, longitude, userId, type);

            // Assert
            Assert.AreEqual(name, station.Name);
            Assert.AreEqual(macAddress, station.MacAddress);
            Assert.AreEqual(latitude, station.Latitude);
            Assert.AreEqual(longitude, station.Longitude);
            Assert.AreEqual(userId, station.UserId);
            Assert.AreEqual(type, station.Type);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidStationNameException))]
        public void Constructor_Should_Throw_When_Name_Is_Empty()
        {
            // Arrange
            string name = "";
            string macAddress = "00:1A:2B:3C:4D:5E";
            float latitude = 45.0f;
            float longitude = 90.0f;
            int? userId = 1;

            // Act
            var station = new Station(name, macAddress, latitude, longitude, userId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidStationMacAddressException))]
        public void Constructor_Should_Throw_When_MacAddress_Is_Invalid()
        {
            // Arrange
            string name = "Weather Station 1";
            string macAddress = "InvalidMac";
            float latitude = 45.0f;
            float longitude = 90.0f;
            int? userId = 1;

            // Act
            var station = new Station(name, macAddress, latitude, longitude, userId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidStationCoordinatesException))]
        public void Constructor_Should_Throw_When_Latitude_Is_Out_Of_Range()
        {
            // Arrange
            string name = "Weather Station 1";
            string macAddress = "00:1A:2B:3C:4D:5E";
            float latitude = 100.0f; // Invalid latitude
            float longitude = 90.0f;
            int? userId = 1;

            // Act
            var station = new Station(name, macAddress, latitude, longitude, userId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidStationCoordinatesException))]
        public void Constructor_Should_Throw_When_Longitude_Is_Out_Of_Range()
        {
            // Arrange
            string name = "Weather Station 1";
            string macAddress = "00:1A:2B:3C:4D:5E";
            float latitude = 45.0f;
            float longitude = -200.0f; // Invalid longitude
            int? userId = 1;

            // Act
            var station = new Station(name, macAddress, latitude, longitude, userId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidStationUserIdException))]
        public void Constructor_Should_Throw_When_UserId_Is_Invalid()
        {
            // Arrange
            string name = "Weather Station 1";
            string macAddress = "00:1A:2B:3C:4D:5E";
            float latitude = 45.0f;
            float longitude = 90.0f;
            int? userId = 0; // Invalid userId

            // Act
            var station = new Station(name, macAddress, latitude, longitude, userId);
        }

        [TestMethod]
        public void Constructor_Should_Allow_Mock_MacAddress()
        {
            // Arrange
            string name = "Mock Station";
            string macAddress = "MOCK_STATION";
            float latitude = 45.0f;
            float longitude = 90.0f;
            int? userId = 1;

            // Act
            var station = new Station(name, macAddress, latitude, longitude, userId);

            // Assert
            Assert.AreEqual(macAddress, station.MacAddress);
        }
    }
}
