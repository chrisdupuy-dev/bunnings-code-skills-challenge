namespace BunningsCodeSkillsChallenge.UnitTests.Services
{
    using System.Collections.Generic;
    using Domain.Interfaces.Models;
    using Domain.Models.Entities;
    using Domain.Services;
    using Moq;
    using Xunit;

    public class SupplierServiceTests
    {
        [Fact]
        public void InsertSupplier_WhenValid_ShouldInsertSuccessfully()
        {
            // Arrange
            var supplierName = "Bob the Builder";

            var expectedSupplier = new Supplier()
            {
                ID = 1,
                Name = supplierName
            };

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.InsertSupplier(It.Is<Supplier>(s => s.Name == supplierName))).Returns(expectedSupplier);

            var supplierService = new SupplierService(null);
            
            // Act
            var actualSupplier = supplierService.InsertSupplier(mockCompany.Object, supplierName);

            // Assert
            Assert.NotNull(actualSupplier);
            Assert.Equal(expectedSupplier, actualSupplier);

            mockCompany.Verify();
        }

        [Fact]
        public void GetSuppliers_WhenCalled_ShouldReturnAllSuppliers()
        {
            // Arrange
            var expectedSuppliers = new List<Supplier>();

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.Suppliers).Returns(expectedSuppliers);

            var supplierService = new SupplierService(null);

            // Act
            var actualSuppliers = supplierService.GetSuppliers(mockCompany.Object);

            // Assert
            Assert.Equal(expectedSuppliers, actualSuppliers);
        }
    }
}
