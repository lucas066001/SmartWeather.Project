namespace SmartWeather.UnitTests.Entities.MeasurePoint
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SmartWeather.Entities.MeasurePoint;
    using SmartWeather.Entities.MeasurePoint.Exceptions;

    [TestClass]
    public class MeasurePointTests
    {
        [TestMethod]
        public void Constructor_Should_Create_Valid_MeasurePoint()
        {
            // Arrange
            int localId = 1;
            string name = "Temperature Sensor";
            string color = "#FF5733";
            int unit = (int)MeasureUnit.Celsius;
            int componentId = 2;

            // Act
            var measurePoint = new MeasurePoint(localId, name, color, unit, componentId);

            // Assert
            Assert.AreEqual(localId, measurePoint.LocalId);
            Assert.AreEqual(name, measurePoint.Name);
            Assert.AreEqual(color, measurePoint.Color);
            Assert.AreEqual(MeasureUnit.Celsius, measurePoint.Unit);
            Assert.AreEqual(componentId, measurePoint.ComponentId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMeasurePointLocalIdException))]
        public void Constructor_Should_Throw_When_LocalId_Is_Invalid()
        {
            // Arrange
            int localId = 0;
            string name = "Temperature Sensor";
            string color = "#FF5733";
            int unit = (int)MeasureUnit.Celsius;
            int componentId = 2;

            // Act
            var measurePoint = new MeasurePoint(localId, name, color, unit, componentId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMeasurePointNameException))]
        public void Constructor_Should_Throw_When_Name_Is_Empty()
        {
            // Arrange
            int localId = 1;
            string name = "";
            string color = "#FF5733";
            int unit = (int)MeasureUnit.Celsius;
            int componentId = 2;

            // Act
            var measurePoint = new MeasurePoint(localId, name, color, unit, componentId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMeasurePointColorException))]
        public void Constructor_Should_Throw_When_Color_Is_Invalid()
        {
            // Arrange
            int localId = 1;
            string name = "Temperature Sensor";
            string color = "InvalidColor";
            int unit = (int)MeasureUnit.Celsius;
            int componentId = 2;

            // Act
            var measurePoint = new MeasurePoint(localId, name, color, unit, componentId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMeasurePointUnitException))]
        public void Constructor_Should_Throw_When_Unit_Is_Invalid()
        {
            // Arrange
            int localId = 1;
            string name = "Temperature Sensor";
            string color = "#FF5733";
            int unit = 999; // Invalid unit
            int componentId = 2;

            // Act
            var measurePoint = new MeasurePoint(localId, name, color, unit, componentId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMeasurePointComponentIdException))]
        public void Constructor_Should_Throw_When_ComponentId_Is_Invalid()
        {
            // Arrange
            int localId = 1;
            string name = "Temperature Sensor";
            string color = "#FF5733";
            int unit = (int)MeasureUnit.Celsius;
            int componentId = 0;

            // Act
            var measurePoint = new MeasurePoint(localId, name, color, unit, componentId);
        }
    }
}
