namespace SmartWeather.UnitTests.Entities
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SmartWeather.Entities.ComponentData;
    using System;

    [TestClass]
    public class MeasureDataTests
    {
        [TestMethod]
        public void Create_Should_Success()
        {
            // Arrange
            var measurePointId = 1;
            var value = 23.5f;
            var dateTime = DateTime.Now;

            // Act
            var measureData = new MeasureData(measurePointId, value, dateTime);

            // Assert
            Assert.AreEqual(measurePointId, measureData.MeasurePointId);
            Assert.AreEqual(value, measureData.Value);
            Assert.AreEqual(dateTime, measureData.DateTime);
        }

        [TestMethod]
        public void Create_Should_Handle_Edge_Case_Values()
        {
            // Arrange
            var measurePointId = 0; // Minimum valid ID
            var value = float.MinValue; // Minimum float value
            var dateTime = DateTime.MinValue; // Minimum DateTime value

            // Act
            var measureData = new MeasureData(measurePointId, value, dateTime);

            // Assert
            Assert.AreEqual(measurePointId, measureData.MeasurePointId);
            Assert.AreEqual(value, measureData.Value);
            Assert.AreEqual(dateTime, measureData.DateTime);
        }

        [TestMethod]
        public void Create_Should_Handle_Large_Values()
        {
            // Arrange
            var measurePointId = int.MaxValue;
            var value = float.MaxValue; // Maximum float value
            var dateTime = DateTime.MaxValue; // Maximum DateTime value

            // Act
            var measureData = new MeasureData(measurePointId, value, dateTime);

            // Assert
            Assert.AreEqual(measurePointId, measureData.MeasurePointId);
            Assert.AreEqual(value, measureData.Value);
            Assert.AreEqual(dateTime, measureData.DateTime);
        }
    }
}
