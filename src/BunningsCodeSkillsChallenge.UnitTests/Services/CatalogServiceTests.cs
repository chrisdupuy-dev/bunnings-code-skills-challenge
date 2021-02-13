namespace BunningsCodeSkillsChallenge.UnitTests.Services
{
    using System.Collections.Generic;
    using Domain.Interfaces.Models;
    using Domain.Models.Entities;
    using Domain.Services;
    using Moq;
    using Xunit;

    public class CatalogServiceTests
    {
        [Fact]
        public void InsertCatalog_WhenValid_ShouldInsertSuccessfully()
        {
            // Arrange
            var sku = "123-abc-xyz";
            var description = "2x4 Timber";

            var expectedCatalog = new Catalog() { SKU = sku, Description = description };

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.InsertCatalog(It.IsAny<Catalog>())).Returns(expectedCatalog);

            var catalogService = new CatalogService(null);

            // Act
            var catalog = catalogService.InsertCatalog(mockCompany.Object, sku, description);

            // Assert
            Assert.Equal(expectedCatalog, catalog);
            mockCompany.Verify(_ => _.InsertCatalog(It.Is<Catalog>(c => c.SKU == sku && c.Description == description)));
        }

        [Fact]
        public void DeleteCatalog_WhenValid_ShouldRemoveSuccessfully()
        {
            // Arrange
            var sku = "123-abc-789";
            var supplierId = 5;

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.DeleteSupplierProductBarcodes(sku));
            mockCompany.Setup(_ => _.DeleteCatalog(sku));

            var catalogService = new CatalogService(null);

            // Act
            catalogService.DeleteCatalog(mockCompany.Object, sku);

            // Assert
            mockCompany.Verify(_ => _.DeleteSupplierProductBarcodes(sku));
            mockCompany.Verify(_ => _.DeleteCatalog(sku));
        }

        [Fact]
        public void GetCatalog_WhenValid_ShouldReturnSuccessfully()
        {
            // Arrange
            var sku = "123-abc-xyz";

            var existingCatalogs = new List<Catalog>()
            {
                new Catalog() { SKU = sku }
            };

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.Catalogs).Returns(existingCatalogs);

            var catalogService = new CatalogService(null);

            // Act
            var catalog = catalogService.GetCatalog(mockCompany.Object, sku);

            // Assert
            Assert.NotNull(catalog);
            Assert.Equal(sku, catalog.SKU);

            mockCompany.Verify();
        }
    }
}
