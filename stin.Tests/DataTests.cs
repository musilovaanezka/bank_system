using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stin.Data;
using stin.Models;
using Xunit;

namespace YourProject.Tests.Data
{
    public class ApplicationDbContextTests
    {
        [Fact]
        public void ApplicationDbContext_Constructor_SetsOptions()
        {
            // Arrange
            var options = new DbContextOptions<ApplicationDbContext>();

            // Act
            var dbContext = new ApplicationDbContext(options);

            // Assert
            Assert.NotNull(dbContext);
        }

        [Fact]
        public void ApplicationDbContext_Ucty_Property_ShouldReturnDbSet()
        {
            // Arrange
            var options = new DbContextOptions<ApplicationDbContext>();
            var dbContext = new ApplicationDbContext(options);

            // Act
            var uctyDbSet = dbContext.Ucty;

            // Assert
            Assert.NotNull(uctyDbSet);
            //Assert.IsType<DbSet<Ucet>>(uctyDbSet);
        }

        [Fact]
        public void ApplicationDbContext_Klienti_Property_ShouldReturnDbSet()
        {
            // Arrange
            var options = new DbContextOptions<ApplicationDbContext>();
            var dbContext = new ApplicationDbContext(options);

            // Act
            var klientiDbSet = dbContext.Klienti;

            // Assert
            Assert.NotNull(klientiDbSet);
            //Assert.IsType<DbSet<stin.Models.Klient>>(klientiDbSet);
        }

        [Fact]
        public void ApplicationDbContext_AutenticationCodes_Property_ShouldReturnDbSet()
        {
            // Arrange
            var options = new DbContextOptions<ApplicationDbContext>();
            var dbContext = new ApplicationDbContext(options);

            // Act
            var autenticationCodesDbSet = dbContext.AutenticationCodes;

            // Assert
            Assert.NotNull(autenticationCodesDbSet);
            //Assert.IsType<DbSet<AutenticationCode>>(autenticationCodesDbSet);
        }
    }
}
