namespace BunningsCodeSkillsChallenge.UnitTests.Services
{
    using System.Linq;
    using Domain.Services;
    using Xunit;

    public class CsvContextServiceTests
    {
        private const string ValidSuppliersDataLocation = @".\TestData\SuppliersA.csv";
        private const string ValidCatalogDataLocation = @".\TestData\CatalogA.csv";
        private const string ValidBarcodesDataLocation = @".\TestData\BarcodesA.csv";

        [Fact]
        public void ImportCompany_WhenGivenValidData_ShouldReadSuccessfully()
        {
            // Arrange
            var csvContextService = new CsvImportExportService();

            // Act
            var company = csvContextService.ImportCompany("A", ValidSuppliersDataLocation, ValidCatalogDataLocation, 
                ValidBarcodesDataLocation);

            // Assert
            Assert.Equal(5, company.Suppliers.Count());
            Assert.Equal(52, company.SupplierProductBarcodes.Count());
            Assert.Equal(5, company.Catalogs.Count());
        }
    }
}
