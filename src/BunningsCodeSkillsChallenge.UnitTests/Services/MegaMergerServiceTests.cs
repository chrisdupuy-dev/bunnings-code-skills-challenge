namespace BunningsCodeSkillsChallenge.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Models;
    using Domain.Models.Entities;
    using Domain.Services;
    using Xunit;

    public class MegaMergerServiceTests
    {
        private const string SuppliersALocation = "./TestData/SuppliersA.csv";
        private const string CatalogALocation = "./TestData/CatalogA.csv";
        private const string BarcodesALocation = "./TestData/BarcodesA.csv";
        private const string SuppliersBLocation = "./TestData/SuppliersB.csv";
        private const string CatalogBLocation = "./TestData/CatalogB.csv";
        private const string BarcodesBLocation = "./TestData/BarcodesB.csv";

        [Fact]
        public void GetCommonCatalog_WhenValidCompanies_ShouldReturnCommonCatalogSuccessfully()
        {
            // Arrange
            var csvContextService = new CsvImportExportService();
            var companyA = csvContextService.ImportCompany("A", SuppliersALocation, CatalogALocation, BarcodesALocation);
            var companyB = csvContextService.ImportCompany("B", SuppliersBLocation, CatalogBLocation, BarcodesBLocation);

            var megaMergerService = new MegaMergerService(null);

            // Act
            var commonCatalog = megaMergerService.GetCommonCatalog(new[] {companyA, companyB});

            // Assert
            var commonCatalogItems = commonCatalog.CommonCatalogItems;

            Assert.Equal(7, commonCatalogItems.Count());
            AssertCommonCatalogExists(commonCatalogItems, "647-vyk-317", "Walkers Special Old Whiskey", "A");
            AssertCommonCatalogExists(commonCatalogItems, "280-oad-768", "Bread - Raisin", "A");
            AssertCommonCatalogExists(commonCatalogItems, "165-rcy-650", "Tea - Decaf 1 Cup", "A");
            AssertCommonCatalogExists(commonCatalogItems, "999-eol-949", "Cheese - Grana Padano", "B");
            AssertCommonCatalogExists(commonCatalogItems, "167-eol-949", "Cheese - Grana Padano", "A");
            AssertCommonCatalogExists(commonCatalogItems, "999-epd-782", "Carbonated Water - Lemon Lime", "B");
            AssertCommonCatalogExists(commonCatalogItems, "650-epd-782", "Carbonated Water - Lemon Lime", "A");
        }

        [Fact]
        public void GetCommonCatalog_WhenNoBarcodesForItem_ShouldNotAddToCatalog()
        {
            // Arrange
            var expectedSku = "987-xyz-321";
            var expectedDescription = "4x2 Timber";
            var expectedSource = "B";

            var suppliersA = new List<Supplier>() {new Supplier() {ID = 1, Name = "SupplierA"}};
            var catalogsA = new List<Catalog>() {new Catalog() {SKU = "123-abc-789", Description = "2x4 Timber"}};
            var barcodesA = new List<SupplierProductBarcode>();

            var companyA = new Company("A", catalogsA, barcodesA, suppliersA);

            var suppliersB = new List<Supplier>() { new Supplier() { ID = 2, Name = "SupplierB" } };
            var catalogsB = new List<Catalog>() { new Catalog() { SKU = expectedSku, Description = expectedDescription } };
            var barcodesB = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode() {SupplierID = 2, SKU = expectedSku, Barcode = "X11111"}
            };

            var companyB = new Company(expectedSource, catalogsB, barcodesB, suppliersB);

            var megaMergerService = new MegaMergerService(null);

            // Act
            var commonCatalog = megaMergerService.GetCommonCatalog(new[] {companyA, companyB});

            // Assert
            Assert.Single(commonCatalog.CommonCatalogItems);

            var commonCatalogItem = commonCatalog.CommonCatalogItems.First();
            Assert.Equal(expectedSku, commonCatalogItem.SKU);
            Assert.Equal(expectedDescription, commonCatalogItem.Description);
            Assert.Equal(expectedSource, commonCatalogItem.Source);
        }

        [Fact]
        public void GetCommonCatalog_WhenSkusConflictBetweenCompaniesWithNoMatchingBarcodes_ShouldThrowException()
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

            var companyA = new Company(sourceA, catalogsA, barcodesA, suppliersA);

            var suppliersB = new List<Supplier>() { new Supplier() { ID = 2, Name = "SupplierB" } };
            var catalogsB = new List<Catalog>() { new Catalog() { SKU = conflictingSku, Description = barcodeB } };
            var barcodesB = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode() {SupplierID = 2, SKU = conflictingSku, Barcode = barcodeB}
            };

            var companyB = new Company(sourceB, catalogsB, barcodesB, suppliersB);

            var megaMergerService = new MegaMergerService(null);

            // Act & Assert
            Assert.Throws<Exception>(() => megaMergerService.GetCommonCatalog(new[] { companyA, companyB }));
        }

        [Fact]
        public void GetCommonCatalog_WhenDifferentSkusAndMatchingBarcodes_ShouldAddOnceToCatalog()
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

            var companyA = new Company(expectedSourceA, catalogsA, barcodesA, suppliersA);

            var suppliersB = new List<Supplier>() { new Supplier() { ID = 2, Name = "SupplierB" } };
            var catalogsB = new List<Catalog>() { new Catalog() { SKU = expectedSkuB, Description = expectedDescription } };
            var barcodesB = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode() {SupplierID = 2, SKU = expectedSkuB, Barcode = barcode}
            };

            var companyB = new Company(expectedSourceB, catalogsB, barcodesB, suppliersB);

            var megaMergerService = new MegaMergerService(null);

            // Act
            var commonCatalog = megaMergerService.GetCommonCatalog(new[] { companyA, companyB });

            // Assert
            Assert.Single(commonCatalog.CommonCatalogItems);

            var commonCatalogItem = commonCatalog.CommonCatalogItems.First();
            Assert.Equal(expectedSkuA, commonCatalogItem.SKU);
            Assert.Equal(expectedDescription, commonCatalogItem.Description);
            Assert.Equal(expectedSourceA, commonCatalogItem.Source);
        }

        [Fact]
        public void GetCommonCatalog_WhenMatchingSkuAndBarcode_ShouldAddOnceToCatalog()
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

            var companyA = new Company(expectedSourceA, catalogsA, barcodesA, suppliersA);

            var suppliersB = new List<Supplier>() { new Supplier() { ID = 2, Name = "SupplierB" } };
            var catalogsB = new List<Catalog>() { new Catalog() { SKU = expectedSku, Description = expectedDescription } };
            var barcodesB = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode() {SupplierID = 2, SKU = expectedSku, Barcode = barcode}
            };

            var companyB = new Company(expectedSourceB, catalogsB, barcodesB, suppliersB);

            var megaMergerService = new MegaMergerService(null);

            // Act
            var commonCatalog = megaMergerService.GetCommonCatalog(new[] { companyA, companyB });

            // Assert
            Assert.Single(commonCatalog.CommonCatalogItems);

            var commonCatalogItem = commonCatalog.CommonCatalogItems.First();
            Assert.Equal(expectedSku, commonCatalogItem.SKU);
            Assert.Equal(expectedDescription, commonCatalogItem.Description);
            Assert.Equal(expectedSourceA, commonCatalogItem.Source);
        }

        private void AssertCommonCatalogExists(IEnumerable<CommonCatalogItem> commonCatalogItems, string expectedSku, string expectedDescription, string expectedSource)
        {
            var commonCatalogItem = commonCatalogItems.FirstOrDefault(_ => _.SKU == expectedSku);
            Assert.NotNull(commonCatalogItem);
            Assert.Equal(expectedDescription, commonCatalogItem.Description);
            Assert.Equal(expectedSource, commonCatalogItem.Source);
        }
    }
}
