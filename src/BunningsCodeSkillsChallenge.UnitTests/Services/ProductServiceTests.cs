namespace BunningsCodeSkillsChallenge.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Models;
    using Domain.Models.Entities;
    using Domain.Services;
    using Xunit;

    public class ProductServiceTests
    {
        [Theory]
        [InlineData("123-abc-xyz", "2x4 Timber")]
        public void AddProduct_WhenValid_ShouldAddSuccessfully(string sku, string description)
        {
            // Arrange
            var company = new Company("A", new List<Catalog>(), new List<SupplierProductBarcode>(), new List<Supplier>());

            var productService = new ProductService(null);

            // Act
            productService.AddProduct(company, sku, description);

            // Assert
            var catalog = productService.GetProduct(company, sku);
            Assert.NotNull(catalog);
            Assert.Equal(description, catalog.Description);
        }

        [Theory]
        [InlineData("123-abc-xyz")]
        public void RemoveProduct_WhenValid_ShouldRemoveSuccessfully(string sku)
        {
            // Arrange
            var existingCatalogs = new List<Catalog>()
            {
                new Catalog() { SKU = sku }
            };

            var existingSupplierProductBarcodes = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode()
                {
                    Barcode = "17987128974",
                    SKU = sku,
                    SupplierID = 5
                }
            };

            var company = new Company("A", existingCatalogs, existingSupplierProductBarcodes, new List<Supplier>());

            var productService = new ProductService(null);

            // Act
            productService.RemoveProduct(company, sku);

            // Assert
            var catalog = productService.GetProduct(company, sku);
            Assert.Null(catalog);

            var supplierProductBarcodes = productService.GetSupplierProductBarcodesForProduct(company, sku);
            Assert.Empty(supplierProductBarcodes);
        }

        [Theory]
        [InlineData("123-abc-xyz", "2x4 Timber")]
        public void GetProduct_WhenValid_ShouldReturnSuccessfully(string sku, string description)
        {
            // Arrange
            var existingCatalogs = new List<Catalog>()
            {
                new Catalog() { SKU = sku }
            };

            var company = new Company("A", existingCatalogs, new List<SupplierProductBarcode>(), new List<Supplier>());

            var productService = new ProductService(null);

            // Act
            var catalog = productService.GetProduct(company, sku); // remove stuff like this, use company to verify

            // Assert
            Assert.NotNull(catalog);
            Assert.Equal(sku, catalog.SKU);
        }

        [Theory]
        [InlineData("123-abc-xyz", "17987128974", 5)]
        public void GetSupplierProductBarcodesForProduct_WhenValid_ShouldReturnSuccessfully(string sku, string barcode, int supplierID)
        {
            // Arrange
            var existingCatalogs = new List<Catalog>()
            {
                new Catalog() { SKU = sku }
            };

            var existingSupplierProductBarcodes = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode()
                {
                    Barcode = barcode,
                    SKU = sku,
                    SupplierID = supplierID
                }
            };

            var company = new Company("A", existingCatalogs, existingSupplierProductBarcodes, new List<Supplier>());

            var productService = new ProductService(null);

            // Act
            var supplierProductBarcodes = productService.GetSupplierProductBarcodesForProduct(company, sku);

            // Assert
            Assert.Single(supplierProductBarcodes);

            var supplierProductBarcode = supplierProductBarcodes.Single();
            Assert.Equal(sku, supplierProductBarcode.SKU);
            Assert.Equal(barcode, supplierProductBarcode.Barcode);
            Assert.Equal(supplierID, supplierProductBarcode.SupplierID);
        }

        [Theory]
        [InlineData()]
        public void AddBarcodesToProduct_()
        {
            // Fill this in
        }
    }
}
