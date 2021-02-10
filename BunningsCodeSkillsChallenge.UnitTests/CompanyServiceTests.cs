namespace BunningsCodeSkillsChallenge.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Models;
    using Domain.Services;
    using Xunit;

    public class CompanyServiceTests
    {
        [Theory]
        [InlineData("123-abc-xyz", "2x4 Timber")]
        public void AddProduct_WhenValid_ShouldAddSuccessfully(string sku, string description)
        {
            // Arrange
            var company = new Company();

            var companyService = new CompanyService(company);

            // Act
            companyService.AddProduct(sku, description);

            // Assert
            var catalog = companyService.GetProduct(sku);
            Assert.NotNull(catalog);
            Assert.Equal(description, catalog.Description);
        }

        [Theory]
        [InlineData("123-abc-xyz")]
        public void RemoveProduct_WhenValid_ShouldRemoveSuccessfully(string sku)
        {
            // Arrange
            var company = new Company();
            company.Catalogs = new List<Catalog>()
            {
                new Catalog() { SKU = sku }
            };

            company.SupplierProductBarcodes = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode()
                {
                    Barcode = "17987128974",
                    SKU = sku,
                    SupplierID = 5
                }
            };

            var companyService = new CompanyService(company);

            // Act
            companyService.RemoveProduct(sku);

            // Assert
            var catalog = companyService.GetProduct(sku);
            Assert.Null(catalog);

            var supplierProductBarcodes = companyService.GetSupplierProductBarcodesForProduct(sku);
            Assert.Empty(supplierProductBarcodes);
        }

        [Theory]
        [InlineData("123-abc-xyz", "2x4 Timber")]
        public void GetProduct_WhenValid_ShouldReturnSuccessfully(string sku, string description)
        {
            // Arrange
            var company = new Company
            {
                Catalogs = new List<Catalog>() {new Catalog() {SKU = sku, Description = description}}
            };

            var companyService = new CompanyService(company);

            // Act
            var catalog = companyService.GetProduct(sku);

            // Assert
            Assert.NotNull(catalog);
            Assert.Equal(sku, catalog.SKU);
        }

        [Theory]
        [InlineData("123-abc-xyz", "17987128974", 5)]
        public void GetSupplierProductBarcodesForProduct_WhenValid_ShouldReturnSuccessfully(string sku, string barcode, int supplierID)
        {
            // Arrange
            var company = new Company();
            company.Catalogs = new List<Catalog>()
            {
                new Catalog() { SKU = sku }
            };

            company.SupplierProductBarcodes = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode()
                {
                    Barcode = barcode,
                    SKU = sku,
                    SupplierID = supplierID
                }
            };

            var companyService = new CompanyService(company);

            // Act
            var supplierProductBarcodes = companyService.GetSupplierProductBarcodesForProduct(sku);

            // Assert
            Assert.Single(supplierProductBarcodes);

            var supplierProductBarcode = supplierProductBarcodes.Single();
            Assert.Equal(sku, supplierProductBarcode.SKU);
            Assert.Equal(barcode, supplierProductBarcode.Barcode);
            Assert.Equal(supplierID, supplierProductBarcode.SupplierID);
        }
    }
}
