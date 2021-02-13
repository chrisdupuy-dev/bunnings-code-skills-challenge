namespace BunningsCodeSkillsChallenge.IntegrationTests
{
    using System.Linq;
    using Domain;
    using Domain.Interfaces;
    using Domain.Services;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Xunit;

    public class BunningsCodeSkillsChallengeApplicationTests
    {
        private const string SuppliersALocation = "./TestData/SuppliersA.csv";
        private const string CatalogALocation = "./TestData/CatalogA.csv";
        private const string BarcodesALocation = "./TestData/BarcodesA.csv";
        private const string SuppliersBLocation = "./TestData/SuppliersB.csv";
        private const string CatalogBLocation = "./TestData/CatalogB.csv";
        private const string BarcodesBLocation = "./TestData/BarcodesB.csv";

        private IApplication _app;
        public BunningsCodeSkillsChallengeApplicationTests()
        {
            var nullLoggerFactory = new NullLoggerFactory();
            var appLogger = new Logger<BunningsCodeSkillsChallengeApplication>(nullLoggerFactory);
            var companyService = new CompanyManager(new Logger<CompanyManager>(nullLoggerFactory));
            var catalogService = new CatalogService(new Logger<CatalogService>(nullLoggerFactory));
            var supplierService = new SupplierService(new Logger<SupplierService>(nullLoggerFactory));
            var supplierProductBarcodeService = new SupplierProductBarcodeService(new Logger<SupplierProductBarcodeService>(nullLoggerFactory));

            _app = new BunningsCodeSkillsChallengeApplication(appLogger, new CsvImportExportService(), 
                new CommonCatalogService(null), companyService, catalogService, supplierService, supplierProductBarcodeService);
            
            _app.ImportCompany("A", SuppliersALocation, CatalogALocation, BarcodesALocation);
            _app.ImportCompany("B", SuppliersBLocation, CatalogBLocation, BarcodesBLocation);
        }

        [Theory]
        [InlineData("A", "123-abc-789", "2x4 Timber")]
        public void InsertCatalog_WhenProductValid_ShouldInsertCatalogToCompanyAndNotCommonCatalog(string companyName, string sku, string description)
        {
            // Act
            _app.InsertCatalog(companyName, sku, description);

            // Assert
            var commonCatalogItem = _app.GetCommonCatalogs().FirstOrDefault(_ => _.SKU == sku);
            Assert.Null(commonCatalogItem);

            var catalog = _app.GetCatalog(companyName, sku);
            Assert.NotNull(catalog);
            Assert.Equal(sku, catalog.SKU);
            Assert.Equal(description, catalog.Description);
        }

        [Theory]
        [InlineData("A", "123-abc-789", "2x4 Timber", 1, new []{ "X1111", "Y2222", "Z3333" })]
        public void InsertSupplierProductBarcodes_WhenInputValid_ShouldAddToCommonCatalog(string companyName, string sku, string description, int supplierId, string[] barcodes)
        {
            // Arrange
            _app.InsertCatalog(companyName, sku, description);

            // Act
            _app.InsertSupplierProductBarcodes(companyName, sku, supplierId, barcodes);

            // Assert
            var commonCatalogItem = _app.GetCommonCatalogs().FirstOrDefault(_ => _.SKU == sku);
            Assert.NotNull(commonCatalogItem);
            Assert.Equal(description, commonCatalogItem.Description);
            Assert.Equal(companyName, commonCatalogItem.Source);
        }

        [Theory]
        [InlineData("A", "650-epd-782")]
        public void DeleteCatalog_WhenInputValid_ShouldRemoveFromCommonCatalog(string companyName, string sku)
        {
            // Act
            _app.DeleteCatalog(companyName, sku);

            // Assert
            var commonCatalogItem = _app.GetCommonCatalogs().FirstOrDefault(_ => _.SKU == sku);
            Assert.Null(commonCatalogItem);

            var catalog = _app.GetCatalog(companyName, sku);
            Assert.Null(catalog);
        }
    }
}
