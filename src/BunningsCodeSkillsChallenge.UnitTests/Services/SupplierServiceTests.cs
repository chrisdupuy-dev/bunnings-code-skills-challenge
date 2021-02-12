namespace BunningsCodeSkillsChallenge.UnitTests.Services
{
    using System.Collections.Generic;
    using Domain.Models;
    using Domain.Models.Entities;
    using Domain.Services;
    using Xunit;

    public class SupplierServiceTests
    {
        [Fact]
        public void CreateSupplier_WhenValid_ShouldInsertSuccessfully()
        {
            // Arrange
            var supplierName = "Bob the Builder";
            var company = new Company("A", new List<Catalog>(), new List<SupplierProductBarcode>(), new List<Supplier>());
            
            var supplierService = new SupplierService(null);
            
            // Act
            var insertedSupplier = supplierService.CreateSupplier(company, supplierName);

            // Assert
            Assert.NotNull(insertedSupplier);
            Assert.Equal(1, insertedSupplier.ID);
            Assert.Equal(supplierName, insertedSupplier.Name);

            Assert.Contains(insertedSupplier, company.Suppliers);
        }

        [Fact]
        public void GetSuppliers_WhenCalled_ShouldReturnAllSuppliers()
        {
            // Arrange
            var expectedSuppliers = new List<Supplier>();
            var company = new Company("A", new List<Catalog>(), new List<SupplierProductBarcode>(), expectedSuppliers);

            var supplierService = new SupplierService(null);

            // Act
            var actualSuppliers = supplierService.GetSuppliers(company);

            // Assert
            Assert.Equal(expectedSuppliers, actualSuppliers);
        }
    }
}
