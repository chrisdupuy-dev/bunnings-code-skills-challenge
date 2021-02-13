namespace BunningsCodeSkillsChallenge.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Domain.Interfaces;
    using Domain.Interfaces.Models;
    using Domain.Interfaces.Services;
    using Domain.Models;
    using Domain.Models.Entities;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class BunningsCodeSkillsChallengeApplicationTests
    {
        [Fact]
        public void ImportCompany_WhenImporting_ShouldImportAndUpdateCommonCatalog()
        {
            // Arrange
            var companyName = "A";
            var supplierPath = "\\pathToSuppliers";
            var catalogPath = "\\pathToCatalogs";
            var barcodesPath = "\\ppathToBarcodesathToSuppliers";

            var mockLogger = new Mock<ILogger<BunningsCodeSkillsChallengeApplication>>();
            var mockCatalogService = new Mock<ICatalogService>();
            var mockSupplierService = new Mock<ISupplierService>();
            var supplierProductBarcodeService = new Mock<ISupplierProductBarcodeService>();

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.Name).Returns(companyName);

            var allCompanies = new List<ICompany> { mockCompany.Object };

            var mockCompanyService = new Mock<ICompanyManager>();
            mockCompanyService.Setup(_ => _.GetAllCompanies()).Returns(allCompanies);

            var mockImportExportService = new Mock<IImportExportService>();
            mockImportExportService.Setup(_ => _.ImportCompany(companyName, supplierPath, catalogPath, barcodesPath))
                .Returns(mockCompany.Object);

            var mockCommonCatalogService = new Mock<ICommonCatalogService>();
            mockCommonCatalogService.Setup(_ => _.GetCommonCatalogs(allCompanies))
                .Returns(Enumerable.Empty<CommonCatalog>());

            var app = new BunningsCodeSkillsChallengeApplication(mockLogger.Object, mockImportExportService.Object,
                mockCommonCatalogService.Object, mockCompanyService.Object,
                mockCatalogService.Object, mockSupplierService.Object, supplierProductBarcodeService.Object);

            // Act
            app.ImportCompany(companyName, supplierPath, catalogPath, barcodesPath);

            // Assert
            mockImportExportService.Verify(_ => _.ImportCompany(companyName, supplierPath, catalogPath, barcodesPath));
            mockCompanyService.Verify(_ => _.AddCompany(mockCompany.Object));
            mockCommonCatalogService.Verify(_ => _.GetCommonCatalogs(allCompanies));
            mockCompany.Verify();
        }

        [Fact]
        public void ExportCommonCatalog_WhenGivenExportLocation_ShouldExportCommonCatalog()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<BunningsCodeSkillsChallengeApplication>>();
            var mockCompanyService = new Mock<ICompanyManager>();
            var mockCatalogService = new Mock<ICatalogService>();
            var mockSupplierService = new Mock<ISupplierService>();
            var supplierProductBarcodeService = new Mock<ISupplierProductBarcodeService>();
            var mockImportExportService = new Mock<IImportExportService>();
            var mockCommonCatalogService = new Mock<ICommonCatalogService>();

            var app = new BunningsCodeSkillsChallengeApplication(mockLogger.Object, mockImportExportService.Object, 
                mockCommonCatalogService.Object, mockCompanyService.Object, 
                mockCatalogService.Object, mockSupplierService.Object, supplierProductBarcodeService.Object);

            var exportPath = "\\pathToExportTo";

            // Act
            app.ExportCommonCatalog(exportPath);

            // Assert
            mockImportExportService.Verify(_ => _.ExportCommonCatalog(It.IsAny<IEnumerable<CommonCatalog>>(), exportPath));
        }

        [Fact]
        public void GetProduct_WhenCompanyAndProductExist_ShouldReturnCatalog()
        {
            // Arrange
            var companyName = "A";
            var sku = "123-abc-789";

            var mockLogger = new Mock<ILogger<BunningsCodeSkillsChallengeApplication>>();
            var mockSupplierService = new Mock<ISupplierService>();
            var mockImportExportService = new Mock<IImportExportService>();
            var mockCommonCatalogService = new Mock<ICommonCatalogService>();
            var supplierProductBarcodeService = new Mock<ISupplierProductBarcodeService>();

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.Name).Returns(companyName);

            var mockCompanyService = new Mock<ICompanyManager>();
            mockCompanyService.Setup(_ => _.GetCompany(companyName)).Returns(mockCompany.Object);

            var newCatalog = new Catalog();

            var mockCatalogService = new Mock<ICatalogService>();
            mockCatalogService.Setup(_ => _.GetCatalog(mockCompany.Object, sku)).Returns(newCatalog);

            var app = new BunningsCodeSkillsChallengeApplication(mockLogger.Object, mockImportExportService.Object,
                mockCommonCatalogService.Object, mockCompanyService.Object,
                mockCatalogService.Object, mockSupplierService.Object, supplierProductBarcodeService.Object);

            // Act
            var catalog = app.GetCatalog(companyName, sku);

            // Assert
            Assert.Equal(newCatalog, catalog);

            mockCompanyService.Verify(_ => _.GetCompany(companyName));
            mockCatalogService.Verify(_ => _.GetCatalog(mockCompany.Object, sku));
            mockCompany.Verify();
        }

        [Fact]
        public void InsertCatalog_WhenCompanyExists_ShouldAddProductAndUpdateCommonCatalog()
        {
            // Arrange 
            var companyName = "A";
            var sku = "123-abc-789";
            var description = "2x4 Timber";

            var mockLogger = new Mock<ILogger<BunningsCodeSkillsChallengeApplication>>();
            var mockSupplierService = new Mock<ISupplierService>();
            var mockImportExportService = new Mock<IImportExportService>();
            var mockCommonCatalogService = new Mock<ICommonCatalogService>();
            var mockCatalogService = new Mock<ICatalogService>();
            var supplierProductBarcodeService = new Mock<ISupplierProductBarcodeService>();

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.Name).Returns(companyName);
            var allCompanies = new List<ICompany> { mockCompany.Object };

            var mockCompanyService = new Mock<ICompanyManager>();
            mockCompanyService.Setup(_ => _.GetCompany(companyName)).Returns(mockCompany.Object);
            mockCompanyService.Setup(_ => _.GetAllCompanies()).Returns(allCompanies);

            var app = new BunningsCodeSkillsChallengeApplication(mockLogger.Object, mockImportExportService.Object,
                mockCommonCatalogService.Object, mockCompanyService.Object,
                mockCatalogService.Object, mockSupplierService.Object, supplierProductBarcodeService.Object);

            // Act
            app.InsertCatalog(companyName, sku, description);

            // Assert
            mockCompanyService.Verify(_ => _.GetCompany(companyName));
            mockCatalogService.Verify(_ => _.InsertCatalog(mockCompany.Object, sku, description));
            mockCommonCatalogService.Verify(_ => _.GetCommonCatalogs(allCompanies));
            mockCompany.Verify();
        }

        [Fact]
        public void DeleteCatalog_WhenCompanyExists_ShouldRemoveProductAndUpdateCommonCatalog()
        {
            // Arrange 
            var companyName = "A";
            var sku = "123-abc-789";

            var mockLogger = new Mock<ILogger<BunningsCodeSkillsChallengeApplication>>();
            var mockSupplierService = new Mock<ISupplierService>();
            var mockImportExportService = new Mock<IImportExportService>();
            var mockCommonCatalogService = new Mock<ICommonCatalogService>();
            var mockCatalogService = new Mock<ICatalogService>();
            var supplierProductBarcodeService = new Mock<ISupplierProductBarcodeService>();

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.Name).Returns(companyName);
            var allCompanies = new List<ICompany> { mockCompany.Object };

            var mockCompanyService = new Mock<ICompanyManager>();
            mockCompanyService.Setup(_ => _.GetCompany(companyName)).Returns(mockCompany.Object);
            mockCompanyService.Setup(_ => _.GetAllCompanies()).Returns(allCompanies);

            var app = new BunningsCodeSkillsChallengeApplication(mockLogger.Object, mockImportExportService.Object,
                mockCommonCatalogService.Object, mockCompanyService.Object,
                mockCatalogService.Object, mockSupplierService.Object, supplierProductBarcodeService.Object);

            // Act
            app.DeleteCatalog(companyName, sku);

            // Assert
            mockCompanyService.Verify(_ => _.GetCompany(companyName));
            mockCatalogService.Verify(_ => _.DeleteCatalog(mockCompany.Object, sku));
            mockCommonCatalogService.Verify(_ => _.GetCommonCatalogs(allCompanies));
            mockCompany.Verify();
        }

        [Fact]
        public void GetSuppliers_WhenCompanyExists_ShouldReturnSupplier()
        {
            // Arrange 
            var companyName = "A";

            var mockLogger = new Mock<ILogger<BunningsCodeSkillsChallengeApplication>>();
            var mockImportExportService = new Mock<IImportExportService>();
            var mockCommonCatalogService = new Mock<ICommonCatalogService>();
            var mockCatalogService = new Mock<ICatalogService>();
            var supplierProductBarcodeService = new Mock<ISupplierProductBarcodeService>();

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.Name).Returns(companyName);

            var mockCompanyService = new Mock<ICompanyManager>();
            mockCompanyService.Setup(_ => _.GetCompany(companyName)).Returns(mockCompany.Object);

            var existingSuppliers = new List<Supplier> {new Supplier()};

            var mockSupplierService = new Mock<ISupplierService>();
            mockSupplierService.Setup(_ => _.GetSuppliers(mockCompany.Object)).Returns(existingSuppliers);

            var app = new BunningsCodeSkillsChallengeApplication(mockLogger.Object, mockImportExportService.Object,
                mockCommonCatalogService.Object, mockCompanyService.Object,
                mockCatalogService.Object, mockSupplierService.Object, supplierProductBarcodeService.Object);

            // Act
            var suppliers = app.GetSuppliers(companyName);

            // Assert
            Assert.Equal(existingSuppliers, suppliers);

            mockCompanyService.Verify(_ => _.GetCompany(companyName));
            mockSupplierService.Verify(_ => _.GetSuppliers(mockCompany.Object));
            mockCompany.Verify();
        }

        [Fact]
        public void InsertSupplier_WhenCompanyExists_ShouldAddSupplier()
        {
            // Arrange 
            var companyName = "A";
            var supplierName = "Sue Sampson";

            var mockLogger = new Mock<ILogger<BunningsCodeSkillsChallengeApplication>>();
            var mockImportExportService = new Mock<IImportExportService>();
            var mockCommonCatalogService = new Mock<ICommonCatalogService>();
            var mockCatalogService = new Mock<ICatalogService>();
            var supplierProductBarcodeService = new Mock<ISupplierProductBarcodeService>();

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.Name).Returns(companyName);

            var mockCompanyService = new Mock<ICompanyManager>();
            mockCompanyService.Setup(_ => _.GetCompany(companyName)).Returns(mockCompany.Object);

            var newSupplier = new Supplier();

            var mockSupplierService = new Mock<ISupplierService>();
            mockSupplierService.Setup(_ => _.InsertSupplier(mockCompany.Object, supplierName)).Returns(newSupplier);

            var app = new BunningsCodeSkillsChallengeApplication(mockLogger.Object, mockImportExportService.Object,
                mockCommonCatalogService.Object, mockCompanyService.Object,
                mockCatalogService.Object, mockSupplierService.Object, supplierProductBarcodeService.Object);

            // Act
            var supplierReturned = app.InsertSupplier(companyName, supplierName);

            // Assert
            Assert.Equal(newSupplier, supplierReturned);

            mockCompanyService.Verify(_ => _.GetCompany(companyName));
            mockSupplierService.Verify(_ => _.InsertSupplier(mockCompany.Object, supplierName));
            mockCompany.Verify();
        }

        [Fact]
        public void InsertSupplierProductBarcodes_WhenCompanyExists_ShouldAddBarcodesToProductAndUpdateCommonCatalog()
        {
            // Arrange 
            var companyName = "A";
            var supplierId = 1;
            var sku = "123-abc-789";
            var barcodes = new string[]
            {
                "x111111",
                "y222222",
                "z333333"
            };

            var mockLogger = new Mock<ILogger<BunningsCodeSkillsChallengeApplication>>();
            var mockImportExportService = new Mock<IImportExportService>();
            var mockCommonCatalogService = new Mock<ICommonCatalogService>();
            var mockSupplierService = new Mock<ISupplierService>();
            var mockCatalogService = new Mock<ICatalogService>();

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.Name).Returns(companyName);
            var allCompanies = new List<ICompany> { mockCompany.Object };

            var mockCompanyService = new Mock<ICompanyManager>();
            mockCompanyService.Setup(_ => _.GetCompany(companyName)).Returns(mockCompany.Object);
            mockCompanyService.Setup(_ => _.GetAllCompanies()).Returns(allCompanies);

            var supplierProductBarcodeService = new Mock<ISupplierProductBarcodeService>();
            supplierProductBarcodeService.Setup(_ => _.InsertSupplierProductBarcodes(mockCompany.Object, supplierId, sku, barcodes));

            var app = new BunningsCodeSkillsChallengeApplication(mockLogger.Object, mockImportExportService.Object,
                mockCommonCatalogService.Object, mockCompanyService.Object,
                mockCatalogService.Object, mockSupplierService.Object, supplierProductBarcodeService.Object);

            // Act
            app.InsertSupplierProductBarcodes(companyName, sku, supplierId, barcodes);

            // Assert
            mockCompanyService.Verify(_ => _.GetCompany(companyName));
            supplierProductBarcodeService.Verify(_ => _.InsertSupplierProductBarcodes(mockCompany.Object, supplierId, sku, barcodes));
            mockCommonCatalogService.Verify(_ => _.GetCommonCatalogs(allCompanies));
            mockCompany.Verify();
        }
    }
}
