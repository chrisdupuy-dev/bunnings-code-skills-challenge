namespace BunningsCodeSkillsChallenge.UnitTests
{
    using System.Linq;
    using Domain.Services;
    using Xunit;

    public class CsvContextServiceTests
    {
        private const string ValidSuppliersDataLocation = "./TestData/ValidSuppliersData.csv";
        private const string ValidCatalogsDataLocation = "./TestData/ValidCatalogsData.csv";
        private const string ValidBarcodesDataLocation = "./TestData/ValidSupplierProductBarcodesData.csv";

        [Fact]
        public void ReadCompany_WhenGivenValidData_ShouldReadSuccessfully()
        {
            // Arrange
            var csvContextService = new CsvReaderService();

            // Act
            var company = csvContextService.ReadCompany(ValidSuppliersDataLocation, ValidBarcodesDataLocation, ValidCatalogsDataLocation);

            // Assert
            Assert.Equal(3, company.Suppliers.Count());
            Assert.Equal(3, company.SupplierProductBarcodes.Count());
            Assert.Equal(3, company.Catalogs.Count());
        }
    }
}
