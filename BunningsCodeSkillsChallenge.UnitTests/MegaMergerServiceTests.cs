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
            var commonCatalog = megaMergerService.GetCommonCatalog(new[] {companyA, companyB});

            // Assert
            AssertCommonCatalogExists(commonCatalog, "647-vyk-317", "Walkers Special Old Whiskey", "A");
            AssertCommonCatalogExists(commonCatalog, "280-oad-768", "Bread - Raisin", "A");
            AssertCommonCatalogExists(commonCatalog, "165-rcy-650", "Tea - Decaf 1 Cup", "A");
            AssertCommonCatalogExists(commonCatalog, "999-eol-949", "Cheese - Grana Padano", "B");
            AssertCommonCatalogExists(commonCatalog, "167-eol-949", "Cheese - Grana Padano", "A");
            AssertCommonCatalogExists(commonCatalog, "999-epd-782", "Carbonated Water - Lemon Lime", "B");
            AssertCommonCatalogExists(commonCatalog, "650-epd-782", "Carbonated Water - Lemon Lime", "A");
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
