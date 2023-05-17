using stin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stin.Tests
{
    public class ModelsTests
    {
        [Fact]
        public void Ucet_Properties_Should_Set_Correctly()
        {
            // Arrange
            int id = 1;
            string ucetNum = "ABC123";
            string mena = "USD";
            int hodnota = 1000;

            // Act
            var ucet = new Ucet
            {
                Id = id,
                UcetNum = ucetNum,
                Mena = mena,
                hodnota = hodnota
            };

            // Assert
            Assert.Equal(id, ucet.Id);
            Assert.Equal(ucetNum, ucet.UcetNum);
            Assert.Equal(mena, ucet.Mena);
            Assert.Equal(hodnota, ucet.hodnota);
        }

        [Fact]
        public void Klient_Properties_Should_Set_Correctly()
        {
            // Arrange
            string ucetNum = "ABC123";
            string username = "john.doe";
            string password = "password123";

            // Act
            var klient = new stin.Models.Klient
            {
                UcetNum = ucetNum,
                Username = username,
                Password = password
            };

            // Assert
            Assert.Equal(ucetNum, klient.UcetNum);
            Assert.Equal(username, klient.Username);
            Assert.Equal(password, klient.Password);
        }

        [Fact]
        public void Klient_Username_Should_Be_Required()
        {
            // Arrange
            var klient = new stin.Models.Klient();

            // Act & Assert
            Assert.Throws<ValidationException>(() => Validator.ValidateObject(klient, new ValidationContext(klient), true));
        }

        [Fact]
        public void Klient_Password_Should_Have_Minimum_Length()
        {
            // Arrange
            var klient = new stin.Models.Klient
            {
                Password = "short"
            };

            // Act & Assert
            Assert.Throws<ValidationException>(() => Validator.ValidateObject(klient, new ValidationContext(klient), true));
        }

        [Fact]
        public void Currency_Properties_Should_Set_Correctly()
        {
            // Arrange
            string code = "USD";
            int amount = 100;
            float exchangeRate = 1.2f;

            // Act
            var currency = new Currency(code, amount, exchangeRate);

            // Assert
            Assert.Equal(code, currency.Code);
            Assert.Equal(amount, currency.Amount);
            Assert.Equal(exchangeRate, currency.ExchangeRate); // Allow a small precision difference
        }

        [Fact]
        public void AutenticationCode_Properties_Should_Set_Correctly()
        {
            // Arrange
            int id = 1;
            string username = "john.doe";
            string code = "123456";
            DateTime endDateTime = DateTime.Now;

            // Act
            var authCode = new AutenticationCode
            {
                Id = id,
                Username = username,
                Code = code,
                EndDateTime = endDateTime
            };

            // Assert
            Assert.Equal(id, authCode.Id);
            Assert.Equal(username, authCode.Username);
            Assert.Equal(code, authCode.Code);
            Assert.Equal(endDateTime, authCode.EndDateTime);
        }

        [Fact]
        public void AutenticationCode_Username_Should_Be_Required()
        {
            // Arrange
            var authCode = new AutenticationCode();

            // Act & Assert
            Assert.Throws<ValidationException>(() => Validator.ValidateObject(authCode, new ValidationContext(authCode), true));
        }
    }
}
