namespace SmartWeather.UnitTests.Entities
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SmartWeather.Entities.ActivationPlan;
    using SmartWeather.Entities.ActivationPlan.Exceptions;
    using System;

    [TestClass]
    public class ActivationPlanTests
    {
        [TestMethod]
        public void Create_Should_Success()
        {
            // Arrange
            var name = "Test Plan";
            var startingDate = DateTime.Now;
            var endingDate = startingDate.AddDays(10);
            var activationTime = TimeSpan.FromHours(8);
            var duration = TimeSpan.FromHours(2);
            var periodInDay = 2;
            var componentId = 1;

            // Act
            var plan = new ActivationPlan(name, startingDate, endingDate, periodInDay, activationTime, duration, componentId);

            // Assert
            Assert.AreEqual(name, plan.Name);
            Assert.AreEqual(startingDate, plan.StartingDate);
            Assert.AreEqual(endingDate, plan.EndingDate);
            Assert.AreEqual(activationTime, plan.ActivationTime);
            Assert.AreEqual(duration, plan.Duration);
            Assert.AreEqual(periodInDay, plan.PeriodInDay);
            Assert.AreEqual(componentId, plan.ComponentId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidActivationPlanNameException))]
        public void Create_Should_Fail_With_Null_Or_Empty_Name()
        {
            // Act
            _ = new ActivationPlan(null!, DateTime.Now, DateTime.Now.AddDays(1), 1, TimeSpan.FromHours(8), TimeSpan.FromHours(2), 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidActivationPlanNameException))]
        public void Create_Should_Fail_With_Whitespace_Name()
        {
            // Act
            _ = new ActivationPlan(" ", DateTime.Now, DateTime.Now.AddDays(1), 1, TimeSpan.FromHours(8), TimeSpan.FromHours(2), 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidActivationPlanComponentIdException))]
        public void Create_Should_Fail_With_Invalid_ComponentId()
        {
            // Act
            _ = new ActivationPlan("Test", DateTime.Now, DateTime.Now.AddDays(1), 1, TimeSpan.FromHours(8), TimeSpan.FromHours(2), 0);
        }

        [TestMethod]
        public void GetNextActivation_Should_Return_Correct_Time_And_Remaining_Cycles()
        {
            // Arrange
            var startingDate = DateTime.Now.Date.AddDays(-1); // Start yesterday
            var endingDate = startingDate.AddDays(10);
            var activationTime = TimeSpan.FromHours(8);
            var plan = new ActivationPlan("Test", startingDate, endingDate, 2, activationTime, TimeSpan.FromHours(2), 1);

            // Act
            var result = plan.GetNextActivation();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Item1 > TimeSpan.Zero); // Next activation should be in the future
            Assert.IsTrue(result.Item2 > 0); // There should be remaining cycles
        }

        [TestMethod]
        public void GetNextActivation_Should_Return_Null_When_Ended()
        {
            // Arrange
            var startingDate = DateTime.Now.Date.AddDays(-10); // Start far in the past
            var endingDate = startingDate.AddDays(5); // Already ended
            var activationTime = TimeSpan.FromHours(8);
            var plan = new ActivationPlan("Test", startingDate, endingDate, 2, activationTime, TimeSpan.FromHours(2), 1);

            // Act
            var result = plan.GetNextActivation();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetNextActivation_Should_Handle_Single_Activation_Correctly()
        {
            // Arrange
            var startingDate = DateTime.Now.Date; // Start today
            var endingDate = startingDate + TimeSpan.FromDays(1); // Ends tomorrow
            var activationTime = DateTime.Now.TimeOfDay.Add(TimeSpan.FromHours(1)); // Next activation in 1 hour
            var plan = new ActivationPlan("Test", startingDate, endingDate, 1, activationTime, TimeSpan.FromHours(2), 1);

            // Act
            var result = plan.GetNextActivation();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Item2); // Only one activation remaining
        }

        [TestMethod]
        public void Create_Should_Handle_Edge_Case_Dates()
        {
            // Arrange
            var name = "Edge Case Plan";
            var startingDate = DateTime.MinValue.AddYears(1); // Close to DateTime.MinValue
            var endingDate = startingDate.AddDays(10);
            var activationTime = TimeSpan.Zero; // Midnight
            var duration = TimeSpan.FromHours(1);
            var periodInDay = 1;
            var componentId = 1;

            // Act
            var plan = new ActivationPlan(name, startingDate, endingDate, periodInDay, activationTime, duration, componentId);

            // Assert
            Assert.AreEqual(startingDate, plan.StartingDate);
            Assert.AreEqual(endingDate, plan.EndingDate);
        }
    }
}