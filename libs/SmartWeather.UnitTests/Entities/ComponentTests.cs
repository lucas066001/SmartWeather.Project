namespace SmartWeather.UnitTests.Entities
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SmartWeather.Entities.Component;
    using SmartWeather.Entities.Component.Exceptions;
    using System;

    [TestClass]
    public class ComponentTests
    {
        [TestMethod]
        public void Create_Should_Success()
        {
            // Arrange
            var name = "Temperature Sensor";
            var color = "#FF5733";
            var type = (int)ComponentType.Sensor;
            var stationId = 1;
            var gpioPin = 10;

            // Act
            var component = new Component(name, color, type, stationId, gpioPin);

            // Assert
            Assert.AreEqual(name, component.Name);
            Assert.AreEqual(color, component.Color);
            Assert.AreEqual(ComponentType.Sensor, component.Type);
            Assert.AreEqual(stationId, component.StationId);
            Assert.AreEqual(gpioPin, component.GpioPin);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidComponentNameException))]
        public void Create_Should_Fail_With_Empty_Name()
        {
            // Act
            _ = new Component("", "#FFFFFF", (int)ComponentType.Actuator, 1, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidComponentNameException))]
        public void Create_Should_Fail_With_Null_Name()
        {
            // Act
            _ = new Component(null!, "#FFFFFF", (int)ComponentType.Actuator, 1, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidComponentColorException))]
        public void Create_Should_Fail_With_Invalid_Color_Format()
        {
            // Act
            _ = new Component("Sensor", "NotAColor", (int)ComponentType.Actuator, 1, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidComponentTypeException))]
        public void Create_Should_Fail_With_Invalid_Type()
        {
            // Act
            _ = new Component("Sensor", "#FFFFFF", 999, 1, 10); // 999 is not a valid ComponentType
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidComponentStationIdException))]
        public void Create_Should_Fail_With_Invalid_StationId()
        {
            // Act
            _ = new Component("Sensor", "#FFFFFF", (int)ComponentType.Sensor, 0, 10); // StationId <= 0
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidComponentGpioPinException))]
        public void Create_Should_Fail_With_Invalid_GpioPin_Below_Range()
        {
            // Act
            _ = new Component("Sensor", "#FFFFFF", (int)ComponentType.Sensor, 1, -1); // gpioPin < 0
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidComponentGpioPinException))]
        public void Create_Should_Fail_With_Invalid_GpioPin_Above_Range()
        {
            // Act
            _ = new Component("Sensor", "#FFFFFF", (int)ComponentType.Sensor, 1, 40); // gpioPin > 39
        }

        [TestMethod]
        public void Create_Should_Handle_Edge_Case_Values()
        {
            // Arrange
            var name = "Edge Case Component";
            var color = "#000000"; // Valid minimum hex color
            var type = (int)ComponentType.Unknown;
            var stationId = 1;
            var gpioPin = 0; // Minimum valid GPIO pin

            // Act
            var component = new Component(name, color, type, stationId, gpioPin);

            // Assert
            Assert.AreEqual(name, component.Name);
            Assert.AreEqual(color, component.Color);
            Assert.AreEqual(ComponentType.Unknown, component.Type);
            Assert.AreEqual(stationId, component.StationId);
            Assert.AreEqual(gpioPin, component.GpioPin);
        }
    }
}
