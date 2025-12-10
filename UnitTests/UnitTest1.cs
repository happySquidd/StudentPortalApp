using StudentPortalApp.Models;
using StudentPortalApp.Views;

namespace UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task LoginFieldEmpty()
        {
            // Arrange
            string username = "";

            // Act
            User user = await Login.VerifyUsername(username);

            // Assert
            Assert.Null(user);
        }
        
        [Fact]
        public async Task IncorrectPassword()
        {
            // Arrange
            string username = "user";
            string password = "";

            // Act
            User user = await Login.VerifyUsername(username);
            bool incorrectPassword = await Login.VerifyPassword(user, password);

            // Assert 
            Assert.False(incorrectPassword);
        }
    }
}
