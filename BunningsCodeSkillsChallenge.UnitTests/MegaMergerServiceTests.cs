namespace BunningsCodeSkillsChallenge.UnitTests
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
            var csvContextService = new CsvReaderService();
            var companyA = csvContextService.ReadCompany("A", SuppliersALocation, BarcodesALocation, CatalogALocation);
            var companyB = csvContextService.ReadCompany("B", SuppliersBLocation, BarcodesBLocation, CatalogBLocation);

            var megaMergerService = new MegaMergerService();

            // Act
            var commonCatalogs = megaMergerService.GetCommonCatalog(new[] {companyA, companyB});

            // Assert
            Assert.Equal(7, commonCatalogs.Count());
            AssertCommonCatalogExists(commonCatalogs, "647-vyk-317", "Walkers Special Old Whiskey", "A");
            AssertCommonCatalogExists(commonCatalogs, "280-oad-768", "Bread - Raisin", "A");
            AssertCommonCatalogExists(commonCatalogs, "165-rcy-650", "Tea - Decaf 1 Cup", "A");
            AssertCommonCatalogExists(commonCatalogs, "999-eol-949", "Cheese - Grana Padano", "B");
            AssertCommonCatalogExists(commonCatalogs, "167-eol-949", "Cheese - Grana Padano", "A");
            AssertCommonCatalogExists(commonCatalogs, "999-epd-782", "Carbonated Water - Lemon Lime", "B");
            AssertCommonCatalogExists(commonCatalogs, "650-epd-782", "Carbonated Water - Lemon Lime", "A");
        }

        private void AssertCommonCatalogExists(IEnumerable<CommonCatalog> commonCatalogs, string expectedSku, string expectedDescription, string expectedSource)
        {
            var commonCatalog = commonCatalogs.FirstOrDefault(_ => _.SKU == expectedSku);
            Assert.NotNull(commonCatalog);
            Assert.Equal(expectedDescription, commonCatalog.Description);
            Assert.Equal(expectedSource, commonCatalog.Source);
        }
    }
}
