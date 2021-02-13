namespace BunningsCodeSkillsChallenge.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
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

        [Fact]
        public void GetSupplierProductBarcodesForProduct_WhenValid_ShouldReturnSuccessfully()
        {
            // Arrange
            var sku = "123-abc-xyz";
            var barcode = "17987128974";
            var supplierID = 5;

            var existingSupplierProductBarcodes = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode()
                {
                    Barcode = barcode,
                    SKU = sku,
                    SupplierID = supplierID
                }
            };

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.SupplierProductBarcodes).Returns(existingSupplierProductBarcodes);

            var catalogService = new CatalogService(null);

            // Act
            var supplierProductBarcodes = catalogService.GetSupplierProductBarcodesForProduct(mockCompany.Object, sku);

            // Assert
            Assert.Single(supplierProductBarcodes);

            var supplierProductBarcode = supplierProductBarcodes.Single();
            Assert.Equal(sku, supplierProductBarcode.SKU);
            Assert.Equal(barcode, supplierProductBarcode.Barcode);
            Assert.Equal(supplierID, supplierProductBarcode.SupplierID);

            mockCompany.Verify();
        }

        [Fact]
        public void InsertSupplierProductBarcodes_WhenValid_ShouldInsertBarcodes()
        {
            // Arrange
            var supplierId = 1;
            var sku = "123-abc-xyz";
            var expectedBarcodes = new []
            {
                "x111111"
            };

            var expectedSupplierProductBarcode = new SupplierProductBarcode()
            {
                SKU = sku,
                SupplierID = supplierId,
                Barcode = expectedBarcodes.First()
            };

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.InsertSupplierProductBarcode(It.Is<SupplierProductBarcode>(b => 
                    b.SKU == sku && 
                    b.Barcode == expectedBarcodes.First() 
                    && b.SupplierID == supplierId)))
                .Returns(expectedSupplierProductBarcode);

            var catalogService = new CatalogService(null);

            // Act
            var actualSupplierProductBarcodes = catalogService.
                InsertSupplierProductBarcodes(mockCompany.Object, supplierId, sku, expectedBarcodes);

            // Assert
            Assert.Single(actualSupplierProductBarcodes);
            Assert.Equal(expectedSupplierProductBarcode, actualSupplierProductBarcodes.First());
            mockCompany.Verify();
        }
    }
}
