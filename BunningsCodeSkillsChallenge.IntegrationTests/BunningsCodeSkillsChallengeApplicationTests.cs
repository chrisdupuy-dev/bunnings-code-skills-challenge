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
            var productService = new ProductService(new Logger<ProductService>(nullLoggerFactory));
            var supplierService = new SupplierService(new Logger<SupplierService>(nullLoggerFactory));

            _app = new BunningsCodeSkillsChallengeApplication(appLogger, new CsvImportExportService(), 
                new MegaMergerService(), productService, supplierService);
            
            _app.ImportCompany("A", SuppliersALocation, CatalogALocation, BarcodesALocation);
            _app.ImportCompany("B", SuppliersBLocation, CatalogBLocation, BarcodesBLocation);
        }

        [Theory]
        [InlineData("A", "123-abc-789", "2x4 Timber")]
        public void AddNewProduct_WhenProductValid_ShouldAddProductToCompanyAndNotCommonCatalog(string companyName, string sku, string description)
        {
            // Act
            _app.AddNewProduct(companyName, sku, description);

            // Assert
            var commonCatalog = _app.GetCommonCatalog();
            var commonCatalogItem = commonCatalog.CommonCatalogItems.FirstOrDefault(_ => _.SKU == sku);

            Assert.Null(commonCatalogItem);

            var catalog = _app.GetProduct(companyName, sku);
            Assert.NotNull(catalog);
            Assert.Equal(sku, catalog.SKU);
            Assert.Equal(description, catalog.Description);
        }

        [Theory]
        [InlineData("A", "123-abc-789", "2x4 Timber", 1, new []{ "X1111", "Y2222", "Z3333" })]
        public void AddProductBarcodes_WhenInputValid_ShouldAddToCommonCatalog(string companyName, string sku, string description, int supplierId, string[] barcodes)
        {
            // Arrange
            _app.AddNewProduct(companyName, sku, description);

            // Act
            _app.AddProductBarcodes(companyName, sku, supplierId, barcodes);

            // Assert
            var commonCatalog = _app.GetCommonCatalog();
            var commonCatalogItem = commonCatalog.CommonCatalogItems.FirstOrDefault(_ => _.SKU == sku);

            Assert.NotNull(commonCatalogItem);
            Assert.Equal(description, commonCatalogItem.Description);
            Assert.Equal(companyName, commonCatalogItem.Source);
        }

        [Theory]
        [InlineData()]
        public void AddSupplier_WhenInputValid_ShouldAddSupplier()
        {

        }
    }
}
