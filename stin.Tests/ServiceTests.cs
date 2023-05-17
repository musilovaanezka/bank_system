using stin.Models;
using stin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace stin.Tests
{
    public class ServiceTests
    {
        [Fact]
        public void GetAuthenticationCode_Should_Return_AuthenticationCode()
        {
            // Arrange
            string username = "john.doe";
            DateTime expectedEndDateTime = DateTime.Now.AddDays(1);

            var service = new AuthenticationCodeService();

            // Act
            var authenticationCode = service.getAuthenticationCode(username);

            // Assert
            Assert.Equal(username, authenticationCode.Username);
            Assert.Equal(expectedEndDateTime.Date, authenticationCode.EndDateTime.Date);
            Assert.NotNull(authenticationCode.Code);
        }

    }
}
