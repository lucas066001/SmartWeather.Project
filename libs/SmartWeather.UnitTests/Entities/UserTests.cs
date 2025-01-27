namespace SmartWeather.UnitTests.Entities
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SmartWeather.Entities.User;
    using SmartWeather.Entities.User.Exceptions;
    using System.Security.Cryptography;
    using System.Text;

    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void Constructor_Should_Create_Valid_User()
        {
            // Arrange
            string username = "JohnDoe";
            string email = "johndoe@example.com";
            string password = "SecurePassword123";
            Role role = Role.Admin;

            // Act
            var user = new User(username, email, password, role);

            // Assert
            Assert.AreEqual(username, user.Username);
            Assert.AreEqual(email, user.Email);
            Assert.AreEqual(User.HashPassword(password), user.PasswordHash);
            Assert.AreEqual(role, user.Role);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserEmailException))]
        public void Constructor_Should_Throw_When_Email_Is_Invalid()
        {
            // Arrange
            string username = "JohnDoe";
            string email = "invalid-email";
            string password = "SecurePassword123";

            // Act
            var user = new User(username, email, password);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserPasswordException))]
        public void Constructor_Should_Throw_When_Password_Is_Empty()
        {
            // Arrange
            string username = "JohnDoe";
            string email = "johndoe@example.com";
            string password = "";

            // Act
            var user = new User(username, email, password);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserNameException))]
        public void Constructor_Should_Throw_When_Username_Is_Empty()
        {
            // Arrange
            string username = "";
            string email = "johndoe@example.com";
            string password = "SecurePassword123";

            // Act
            var user = new User(username, email, password);
        }

        [TestMethod]
        public void HashPassword_Should_Return_Expected_Hash()
        {
            // Arrange
            string password = "SecurePassword123";
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                string expectedHash = Convert.ToHexString(sha256.ComputeHash(passwordBytes));

                // Act
                string actualHash = User.HashPassword(password);

                // Assert
                Assert.AreEqual(expectedHash, actualHash);
            }
        }

        [TestMethod]
        public void Constructor_Should_Set_Default_Role_To_User()
        {
            // Arrange
            string username = "JohnDoe";
            string email = "johndoe@example.com";
            string password = "SecurePassword123";

            // Act
            var user = new User(username, email, password);

            // Assert
            Assert.AreEqual(Role.User, user.Role);
        }
    }
}
