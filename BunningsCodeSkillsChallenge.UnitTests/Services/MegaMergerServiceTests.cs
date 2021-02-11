namespace BunningsCodeSkillsChallenge.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Models;
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

            var megaMergerService = new MegaMergerService();

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

        private void AssertCommonCatalogExists(IEnumerable<CommonCatalogItem> commonCatalogItems, string expectedSku, string expectedDescription, string expectedSource)
        {
            var commonCatalogItem = commonCatalogItems.FirstOrDefault(_ => _.SKU == expectedSku);
            Assert.NotNull(commonCatalogItem);
            Assert.Equal(expectedDescription, commonCatalogItem.Description);
            Assert.Equal(expectedSource, commonCatalogItem.Source);
        }
    }
}
