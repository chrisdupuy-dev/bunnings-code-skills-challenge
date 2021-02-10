namespace BunningsCodeSkillsChallenge.UnitTests.Services
{
    using System.Linq;
    using Domain.Services;
    using Xunit;

    public class CsvContextServiceTests
    {
        private const string ValidSuppliersDataLocation = "./TestData/SuppliersA.csv";
        private const string ValidCatalogDataLocation = "./TestData/CatalogA.csv";
        private const string ValidBarcodesDataLocation = "./TestData/BarcodesA.csv";

        [Fact]
        public void ReadCompany_WhenGivenValidData_ShouldReadSuccessfully()
        {
            // Arrange
            var csvContextService = new CsvReaderService();

            // Act
            var company = csvContextService.ReadCompany("A", ValidSuppliersDataLocation, ValidBarcodesDataLocation, ValidCatalogDataLocation);

            // Assert
            Assert.Equal(5, company.Suppliers.Count());
            Assert.Equal(52, company.SupplierProductBarcodes.Count());
            Assert.Equal(5, company.Catalogs.Count());
        }
    }
}
