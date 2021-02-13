namespace BunningsCodeSkillsChallenge.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Interfaces.Models;
    using Domain.Models.Entities;
    using Domain.Services;
    using Moq;
    using Xunit;

    public class CommonCatalogServiceTests
    {
        [Fact]
        public void GetCommonCatalogs_WhenNoBarcodesForItem_ShouldNotAddToCatalog()
        {
            // Arrange
            var expectedSku = "987-xyz-321";
            var expectedDescription = "4x2 Timber";
            var expectedSource = "B";

            var suppliersA = new List<Supplier>() {new Supplier() {ID = 1, Name = "SupplierA"}};
            var catalogsA = new List<Catalog>() {new Catalog() {SKU = "123-abc-789", Description = "2x4 Timber"}};
            var barcodesA = new List<SupplierProductBarcode>();

            var mockCompanyA = new Mock<ICompany>();
            mockCompanyA.Setup(_ => _.Name).Returns("A");
            mockCompanyA.Setup(_ => _.Suppliers).Returns(suppliersA);
            mockCompanyA.Setup(_ => _.Catalogs).Returns(catalogsA);
            mockCompanyA.Setup(_ => _.SupplierProductBarcodes).Returns(barcodesA);

            var suppliersB = new List<Supplier>() { new Supplier() { ID = 2, Name = "SupplierB" } };
            var catalogsB = new List<Catalog>() { new Catalog() { SKU = expectedSku, Description = expectedDescription } };
            var barcodesB = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode() {SupplierID = 2, SKU = expectedSku, Barcode = "X11111"}
            };

            var mockCompanyB = new Mock<ICompany>();
            mockCompanyB.Setup(_ => _.Name).Returns(expectedSource);
            mockCompanyB.Setup(_ => _.Suppliers).Returns(suppliersB);
            mockCompanyB.Setup(_ => _.Catalogs).Returns(catalogsB);
            mockCompanyB.Setup(_ => _.SupplierProductBarcodes).Returns(barcodesB);

            var commonCatalogService = new CommonCatalogService(null);

            // Act
            var commonCatalogs = commonCatalogService.GetCommonCatalogs(new[] { mockCompanyA.Object, mockCompanyB .Object });

            // Assert
            Assert.Single(commonCatalogs);

            var commonCatalog = commonCatalogs.First();
            Assert.Equal(expectedSku, commonCatalog.SKU);
            Assert.Equal(expectedDescription, commonCatalog.Description);
            Assert.Equal(expectedSource, commonCatalog.Source);

            mockCompanyA.Verify();
            mockCompanyB.Verify();
        }

        [Fact]
        public void GetCommonCatalogs_WhenSkusConflictBetweenCompaniesWithNoMatchingBarcodes_ShouldThrowException()
        {
            // Arrange
            var conflictingSku = "123-abc-789";
            var description = "2x4 Timber";
            var sourceA = "A";
            var sourceB = "B";
            var barcodeA = "X11111";
            var barcodeB = "Y11111";

            var suppliersA = new List<Supplier>() { new Supplier() { ID = 1, Name = "SupplierA" } };
            var catalogsA = new List<Catalog>() { new Catalog() { SKU = conflictingSku, Description = barcodeA } };
            var barcodesA = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode() { SupplierID = 1, SKU = conflictingSku, Barcode = barcodeA }
            };

            var mockCompanyA = new Mock<ICompany>();
            mockCompanyA.Setup(_ => _.Name).Returns(sourceA);
            mockCompanyA.Setup(_ => _.Suppliers).Returns(suppliersA);
            mockCompanyA.Setup(_ => _.Catalogs).Returns(catalogsA);
            mockCompanyA.Setup(_ => _.SupplierProductBarcodes).Returns(barcodesA);

            var suppliersB = new List<Supplier>() { new Supplier() { ID = 2, Name = "SupplierB" } };
            var catalogsB = new List<Catalog>() { new Catalog() { SKU = conflictingSku, Description = barcodeB } };
            var barcodesB = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode() {SupplierID = 2, SKU = conflictingSku, Barcode = barcodeB}
            };

            var mockCompanyB = new Mock<ICompany>();
            mockCompanyB.Setup(_ => _.Name).Returns(sourceB);
            mockCompanyB.Setup(_ => _.Suppliers).Returns(suppliersB);
            mockCompanyB.Setup(_ => _.Catalogs).Returns(catalogsB);
            mockCompanyB.Setup(_ => _.SupplierProductBarcodes).Returns(barcodesB);

            var commonCatalogService = new CommonCatalogService(null);

            // Act & Assert
            Assert.Throws<Exception>(() => commonCatalogService.GetCommonCatalogs(new[] { mockCompanyA.Object, mockCompanyB.Object }));
        }

        [Fact]
        public void GetCommonCatalogs_WhenDifferentSkusAndMatchingBarcodes_ShouldAddOnceToCatalog()
        {
            // Arrange
            var expectedSkuA = "123-abc-789";
            var expectedSkuB = "987-xyz-321";
            var expectedDescription = "2x4 Timber";
            var expectedSourceA = "A";
            var expectedSourceB = "B";
            var barcode = "X11111";

            var suppliersA = new List<Supplier>() { new Supplier() { ID = 1, Name = "SupplierA" } };
            var catalogsA = new List<Catalog>() { new Catalog() { SKU = expectedSkuA, Description = expectedDescription } };
            var barcodesA = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode() { SupplierID = 1, SKU = expectedSkuA, Barcode = barcode }
            };

            var mockCompanyA = new Mock<ICompany>();
            mockCompanyA.Setup(_ => _.Name).Returns(expectedSourceA);
            mockCompanyA.Setup(_ => _.Suppliers).Returns(suppliersA);
            mockCompanyA.Setup(_ => _.Catalogs).Returns(catalogsA);
            mockCompanyA.Setup(_ => _.SupplierProductBarcodes).Returns(barcodesA);

            var suppliersB = new List<Supplier>() { new Supplier() { ID = 2, Name = "SupplierB" } };
            var catalogsB = new List<Catalog>() { new Catalog() { SKU = expectedSkuB, Description = expectedDescription } };
            var barcodesB = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode() {SupplierID = 2, SKU = expectedSkuB, Barcode = barcode}
            };

            var mockCompanyB = new Mock<ICompany>();
            mockCompanyB.Setup(_ => _.Name).Returns(expectedSourceB);
            mockCompanyB.Setup(_ => _.Suppliers).Returns(suppliersB);
            mockCompanyB.Setup(_ => _.Catalogs).Returns(catalogsB);
            mockCompanyB.Setup(_ => _.SupplierProductBarcodes).Returns(barcodesB);

            var commonCatalogService = new CommonCatalogService(null);

            // Act
            var commonCatalogs = commonCatalogService.GetCommonCatalogs(new[] { mockCompanyA.Object, mockCompanyB.Object });

            // Assert
            Assert.Single(commonCatalogs);

            var commonCatalog = commonCatalogs.First();
            Assert.Equal(expectedSkuA, commonCatalog.SKU);
            Assert.Equal(expectedDescription, commonCatalog.Description);
            Assert.Equal(expectedSourceA, commonCatalog.Source);
        }

        [Fact]
        public void GetCommonCatalogs_WhenMatchingSkuAndBarcode_ShouldAddOnceToCatalog()
        {
            // Arrange
            var expectedSku = "123-abc-789";
            var expectedDescription = "2x4 Timber";
            var expectedSourceA = "A";
            var expectedSourceB = "B";
            var barcode = "X11111";

            var suppliersA = new List<Supplier>() { new Supplier() { ID = 1, Name = "SupplierA" } };
            var catalogsA = new List<Catalog>() { new Catalog() { SKU = expectedSku, Description = expectedDescription } };
            var barcodesA = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode() { SupplierID = 1, SKU = expectedSku, Barcode = barcode }
            };

            var mockCompanyA = new Mock<ICompany>();
            mockCompanyA.Setup(_ => _.Name).Returns(expectedSourceA);
            mockCompanyA.Setup(_ => _.Suppliers).Returns(suppliersA);
            mockCompanyA.Setup(_ => _.Catalogs).Returns(catalogsA);
            mockCompanyA.Setup(_ => _.SupplierProductBarcodes).Returns(barcodesA);

            var suppliersB = new List<Supplier>() { new Supplier() { ID = 2, Name = "SupplierB" } };
            var catalogsB = new List<Catalog>() { new Catalog() { SKU = expectedSku, Description = expectedDescription } };
            var barcodesB = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode() {SupplierID = 2, SKU = expectedSku, Barcode = barcode}
            };

            var mockCompanyB = new Mock<ICompany>();
            mockCompanyB.Setup(_ => _.Name).Returns(expectedSourceB);
            mockCompanyB.Setup(_ => _.Suppliers).Returns(suppliersB);
            mockCompanyB.Setup(_ => _.Catalogs).Returns(catalogsB);
            mockCompanyB.Setup(_ => _.SupplierProductBarcodes).Returns(barcodesB);

            var commonCatalogService = new CommonCatalogService(null);

            // Act
            var commonCatalogs = commonCatalogService.GetCommonCatalogs(new[] { mockCompanyA.Object, mockCompanyB.Object });

            // Assert
            Assert.Single(commonCatalogs);

            var commonCatalog = commonCatalogs.First();
            Assert.Equal(expectedSku, commonCatalog.SKU);
            Assert.Equal(expectedDescription, commonCatalog.Description);
            Assert.Equal(expectedSourceA, commonCatalog.Source);
        }
    }
}
