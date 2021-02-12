namespace BunningsCodeSkillsChallenge.UnitTests
{
    using System.Collections.Generic;
    using Domain;
    using Domain.Interfaces.Services;
    using Domain.Models;
    using Domain.Models.Entities;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class BunningsCodeSkillsChallengeApplicationTests
    {
        [Fact]
        public void ImportCompany_WhenImporting_ShouldImportAndMergeCatalog()
        {
            // Arrange
            var companyName = "A";
            var supplierPath = "\\pathToSuppliers";
            var catalogPath = "\\pathToCatalogs";
            var barcodesPath = "\\ppathToBarcodesathToSuppliers";

            var mockLogger = new Mock<ILogger<BunningsCodeSkillsChallengeApplication>>();
            var mockProductService = new Mock<IProductService>();
            var mockSupplierService = new Mock<ISupplierService>();

            var company = new Company(companyName, null, null, null);
            var allCompanies = new List<Company> {company};

            var mockCompanyService = new Mock<ICompanyService>();
            mockCompanyService.Setup(_ => _.GetAllCompanies()).Returns(allCompanies);

            var mockImportExportService = new Mock<IImportExportService>();
            mockImportExportService.Setup(_ => _.ImportCompany(companyName, supplierPath, catalogPath, barcodesPath))
                .Returns(company);

            var mockMegaMergerService = new Mock<IMegaMergerService>();
            mockMegaMergerService.Setup(_ => _.GetCommonCatalog(allCompanies))
                .Returns(new CommonCatalog(null));

            var app = new BunningsCodeSkillsChallengeApplication(mockLogger.Object, mockImportExportService.Object, mockMegaMergerService.Object, mockCompanyService.Object, mockProductService.Object, mockSupplierService.Object);

            // Act
            app.ImportCompany(companyName, supplierPath, catalogPath, barcodesPath);

            // Assert
            mockImportExportService.Verify(_ => _.ImportCompany(companyName, supplierPath, catalogPath, barcodesPath));
            mockCompanyService.Verify(_ => _.AddCompany(company));
            mockMegaMergerService.Verify(_ => _.GetCommonCatalog(allCompanies));
        }

        [Fact]
        public void ExportCommonCatalog_WhenGivenExportLocation_ShouldExportCommonCatalog()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<BunningsCodeSkillsChallengeApplication>>();
            var mockCompanyService = new Mock<ICompanyService>();
            var mockProductService = new Mock<IProductService>();
            var mockSupplierService = new Mock<ISupplierService>();
            var mockImportExportService = new Mock<IImportExportService>();
            var mockMegaMergerService = new Mock<IMegaMergerService>();

            var app = new BunningsCodeSkillsChallengeApplication(mockLogger.Object, mockImportExportService.Object, mockMegaMergerService.Object, mockCompanyService.Object, mockProductService.Object, mockSupplierService.Object);

            var exportPath = "\\pathToExportTo";

            // Act
            app.ExportCommonCatalog(exportPath);

            // Assert
            mockImportExportService.Verify(_ => _.ExportCommonCatalog(It.IsAny<CommonCatalog>(), exportPath));
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
            var mockMegaMergerService = new Mock<IMegaMergerService>();

            var company = new Company(companyName, null, null, null);

            var mockCompanyService = new Mock<ICompanyService>();
            mockCompanyService.Setup(_ => _.GetCompany(companyName)).Returns(company);

            var newCatalog = new Catalog();

            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(_ => _.GetProduct(company, sku)).Returns(newCatalog);

            var app = new BunningsCodeSkillsChallengeApplication(mockLogger.Object, mockImportExportService.Object, mockMegaMergerService.Object, mockCompanyService.Object, mockProductService.Object, mockSupplierService.Object);

            // Act
            var catalog = app.GetProduct(companyName, sku);

            // Assert
            Assert.Equal(newCatalog, catalog);

            mockCompanyService.Verify(_ => _.GetCompany(companyName));
            mockProductService.Verify(_ => _.GetProduct(company, sku));
        }

        [Fact]
        public void AddNewProduct_WhenCompanyExists_ShouldAddProductAndMergeCatalog()
        {
            // Arrange 
            var companyName = "A";
            var sku = "123-abc-789";
            var description = "2x4 Timber";

            var mockLogger = new Mock<ILogger<BunningsCodeSkillsChallengeApplication>>();
            var mockSupplierService = new Mock<ISupplierService>();
            var mockImportExportService = new Mock<IImportExportService>();
            var mockMegaMergerService = new Mock<IMegaMergerService>();
            var mockProductService = new Mock<IProductService>();

            var company = new Company(companyName, null, null, null);
            var allCompanies = new List<Company> { company };

            var mockCompanyService = new Mock<ICompanyService>();
            mockCompanyService.Setup(_ => _.GetCompany(companyName)).Returns(company);
            mockCompanyService.Setup(_ => _.GetAllCompanies()).Returns(allCompanies);

            var app = new BunningsCodeSkillsChallengeApplication(mockLogger.Object, mockImportExportService.Object, mockMegaMergerService.Object, mockCompanyService.Object, mockProductService.Object, mockSupplierService.Object);

            // Act
            app.AddNewProduct(companyName, sku, description);

            // Assert
            mockCompanyService.Verify(_ => _.GetCompany(companyName));
            mockProductService.Verify(_ => _.AddProduct(company, sku, description));
            mockMegaMergerService.Verify(_ => _.GetCommonCatalog(allCompanies));
        }

        [Fact]
        public void RemoveProduct_WhenCompanyExists_ShouldRemoveProductAndMergeCatalog()
        {
            // Arrange 
            var companyName = "A";
            var sku = "123-abc-789";

            var mockLogger = new Mock<ILogger<BunningsCodeSkillsChallengeApplication>>();
            var mockSupplierService = new Mock<ISupplierService>();
            var mockImportExportService = new Mock<IImportExportService>();
            var mockMegaMergerService = new Mock<IMegaMergerService>();
            var mockProductService = new Mock<IProductService>();

            var company = new Company(companyName, null, null, null);
            var allCompanies = new List<Company> { company };

            var mockCompanyService = new Mock<ICompanyService>();
            mockCompanyService.Setup(_ => _.GetCompany(companyName)).Returns(company);
            mockCompanyService.Setup(_ => _.GetAllCompanies()).Returns(allCompanies);

            var app = new BunningsCodeSkillsChallengeApplication(mockLogger.Object, mockImportExportService.Object, mockMegaMergerService.Object, mockCompanyService.Object, mockProductService.Object, mockSupplierService.Object);

            // Act
            app.RemoveProduct(companyName, sku);

            // Assert
            mockCompanyService.Verify(_ => _.GetCompany(companyName));
            mockProductService.Verify(_ => _.RemoveProduct(company, sku));
            mockMegaMergerService.Verify(_ => _.GetCommonCatalog(allCompanies));
        }

        [Fact]
        public void GetSuppliers_WhenCompanyExists_ShouldReturnSupplier()
        {
            // Arrange 
            var companyName = "A";

            var mockLogger = new Mock<ILogger<BunningsCodeSkillsChallengeApplication>>();
            var mockImportExportService = new Mock<IImportExportService>();
            var mockMegaMergerService = new Mock<IMegaMergerService>();
            var mockProductService = new Mock<IProductService>();

            var company = new Company(companyName, null, null, null);

            var mockCompanyService = new Mock<ICompanyService>();
            mockCompanyService.Setup(_ => _.GetCompany(companyName)).Returns(company);

            var existingSuppliers = new List<Supplier> {new Supplier()};

            var mockSupplierService = new Mock<ISupplierService>();
            mockSupplierService.Setup(_ => _.GetSuppliers(company)).Returns(existingSuppliers);

            var app = new BunningsCodeSkillsChallengeApplication(mockLogger.Object, mockImportExportService.Object, mockMegaMergerService.Object, mockCompanyService.Object, mockProductService.Object, mockSupplierService.Object);

            // Act
            var suppliers = app.GetSuppliers(companyName);

            // Assert
            Assert.Equal(existingSuppliers, suppliers);

            mockCompanyService.Verify(_ => _.GetCompany(companyName));
            mockSupplierService.Verify(_ => _.GetSuppliers(company));
        }

        [Fact]
        public void AddSupplier_WhenCompanyExists_ShouldAddSupplier()
        {
            // Arrange 
            var companyName = "A";
            var supplierName = "Sue Sampson";

            var mockLogger = new Mock<ILogger<BunningsCodeSkillsChallengeApplication>>();
            var mockImportExportService = new Mock<IImportExportService>();
            var mockMegaMergerService = new Mock<IMegaMergerService>();
            var mockProductService = new Mock<IProductService>();

            var company = new Company(companyName, null, null, null);

            var mockCompanyService = new Mock<ICompanyService>();
            mockCompanyService.Setup(_ => _.GetCompany(companyName)).Returns(company);

            var newSupplier = new Supplier();

            var mockSupplierService = new Mock<ISupplierService>();
            mockSupplierService.Setup(_ => _.CreateSupplier(company, supplierName)).Returns(newSupplier);

            var app = new BunningsCodeSkillsChallengeApplication(mockLogger.Object, mockImportExportService.Object, mockMegaMergerService.Object, mockCompanyService.Object, mockProductService.Object, mockSupplierService.Object);

            // Act
            var supplierReturned = app.AddSupplier(companyName, supplierName);

            // Assert
            Assert.Equal(newSupplier, supplierReturned);

            mockCompanyService.Verify(_ => _.GetCompany(companyName));
            mockSupplierService.Verify(_ => _.CreateSupplier(company, supplierName));
        }

        [Fact]
        public void AddProductBarcodes_WhenCompanyExists_ShouldAddBarcodesToProductAndMergeCatalog()
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
            var mockMegaMergerService = new Mock<IMegaMergerService>();
            var mockSupplierService = new Mock<ISupplierService>();

            var company = new Company(companyName, null, null, null);
            var allCompanies = new List<Company> { company };

            var mockCompanyService = new Mock<ICompanyService>();
            mockCompanyService.Setup(_ => _.GetCompany(companyName)).Returns(company);
            mockCompanyService.Setup(_ => _.GetAllCompanies()).Returns(allCompanies);

            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(_ => _.AddBarcodesToProduct(company, supplierId, sku, barcodes));

            var app = new BunningsCodeSkillsChallengeApplication(mockLogger.Object, mockImportExportService.Object, mockMegaMergerService.Object, mockCompanyService.Object, mockProductService.Object, mockSupplierService.Object);

            // Act
            app.AddProductBarcodes(companyName, sku, supplierId, barcodes);

            // Assert
            mockCompanyService.Verify(_ => _.GetCompany(companyName));
            mockProductService.Verify(_ => _.AddBarcodesToProduct(company, supplierId, sku, barcodes));
            mockMegaMergerService.Verify(_ => _.GetCommonCatalog(allCompanies));
        }
    }
}
