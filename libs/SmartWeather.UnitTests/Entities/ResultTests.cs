namespace SmartWeather.UnitTests.Entities.Common
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SmartWeather.Entities.Common;

    [TestClass]
    public class ResultTests
    {
        [TestMethod]
        public void Success_Should_Create_Success_Result()
        {
            // Act
            var result = Result.Success();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(string.Empty, result.ErrorMessage);
        }

        [TestMethod]
        public void Failure_Should_Create_Failure_Result_With_Error_Message()
        {
            // Arrange
            var errorMessage = "An error occurred";

            // Act
            var result = Result.Failure(errorMessage);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }
    }

    [TestClass]
    public class ResultOfTTests
    {
        [TestMethod]
        public void Success_Should_Create_Success_Result_With_Value()
        {
            // Arrange
            var value = "TestValue";

            // Act
            var result = Result<string>.Success(value);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(string.Empty, result.ErrorMessage);
            Assert.AreEqual(value, result.Value);
        }

        [TestMethod]
        public void Failure_Should_Create_Failure_Result_With_Error_Message()
        {
            // Arrange
            var errorMessage = "An error occurred";

            // Act
            var result = Result<string>.Failure(errorMessage);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(errorMessage, result.ErrorMessage);
            Assert.IsNull(result.Value);
        }

        [TestMethod]
        public void IsFailure_Should_Return_True_For_Failure_Result()
        {
            // Arrange
            var result = Result.Failure("An error occurred");

            // Assert
            Assert.IsTrue(result.IsFailure);
        }

        [TestMethod]
        public void IsFailure_Should_Return_False_For_Success_Result()
        {
            // Arrange
            var result = Result.Success();

            // Assert
            Assert.IsFalse(result.IsFailure);
        }
    }
}
