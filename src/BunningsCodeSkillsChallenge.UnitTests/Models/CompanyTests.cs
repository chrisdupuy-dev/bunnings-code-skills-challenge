namespace BunningsCodeSkillsChallenge.UnitTests.Models
{
    using System;
    using System.Collections.Generic;
    using Domain.Models;
    using Domain.Models.Entities;
    using Xunit;

    public class CompanyTests
    {
        [Fact]
        public void ValidateSupplierProductBarcodes_WhenSupplierAndCatalogExists_ShouldConstructSuccessfully()
        {
            // Arrange
            var sku = "123-abc-789";
            var supplierId = 1;

            var catalogs = new List<Catalog>() {new Catalog() {SKU = sku } };
            var suppliers = new List<Supplier>() {new Supplier() {ID = supplierId } };
            var supplierProductBarcodes = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode(){SKU = sku, SupplierID = supplierId, Barcode = "X111111"}
            };

            // Act
            var company = new Company("A", catalogs, supplierProductBarcodes, suppliers);

            // Assert
            Assert.Equal(catalogs, company.Catalogs);
            Assert.Equal(suppliers, company.Suppliers);
            Assert.Equal(supplierProductBarcodes, company.SupplierProductBarcodes);
        }

        [Fact]
        public void ValidateSupplierProductBarcodes_WhenSupplierMissing_ShouldThrowException()
        {
            // Arrange
            var sku = "123-abc-789";
            var supplierId = 1;

            var catalogs = new List<Catalog>() { new Catalog() { SKU = sku } };
            var suppliers = new List<Supplier>() { new Supplier() { ID = supplierId } };
            var supplierProductBarcodes = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode(){SKU = sku, SupplierID = 5, Barcode = "X111111"}
            };

            // Act & Assert
            Assert.Throws<Exception>(() => new Company("A", catalogs, supplierProductBarcodes, suppliers));
        }

        [Fact]
        public void ValidateSupplierProductBarcodes_WhenCatalogMissing_ShouldThrowException()
        {
            // Arrange
            var sku = "123-abc-789";
            var supplierId = 1;

            var catalogs = new List<Catalog>() { new Catalog() { SKU = sku } };
            var suppliers = new List<Supplier>() { new Supplier() { ID = supplierId } };
            var supplierProductBarcodes = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode(){SKU = "789-xyz-321", SupplierID = 1, Barcode = "X111111"}
            };

            // Act & Assert
            Assert.Throws<Exception>(() => new Company("A", catalogs, supplierProductBarcodes, suppliers));
        }

        [Fact]
        public void InsertSupplierProductBarcode_WhenProductDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var supplierId = 1;

            var supplierProductBarcode = new SupplierProductBarcode()
            {
                SKU = "123-abc-xyz",
                Barcode = "X1111",
                SupplierID = supplierId
            };

            var company = new Company("A", new List<Catalog>(),
                new List<SupplierProductBarcode>(), new List<Supplier>() { new Supplier() { ID = supplierId } });

            // Act & Assert
            Assert.Throws<Exception>(() => company.InsertSupplierProductBarcode(supplierProductBarcode));
        }

        [Fact]
        public void InsertSupplierProductBarcode_WhenSupplierDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var sku = "123-abc-xyz";

            var supplierProductBarcode = new SupplierProductBarcode()
            {
                SKU = sku,
                Barcode = "X1111",
                SupplierID = 1
            };

            var company = new Company("A", new List<Catalog>() { new Catalog() { SKU = sku } },
                new List<SupplierProductBarcode>(), new List<Supplier>());

            // Act & Assert
            Assert.Throws<Exception>(() => company.InsertSupplierProductBarcode(supplierProductBarcode));
        }

        [Fact]
        public void InsertCatalog_WhenProductWithExistingSkuAlreadyExists_ShouldThrowException()
        {
            // Arrange
            var sku = "123-abc-xyz";
            var description = "2x4 Timber";

            var existingCatalog = new Catalog()
            {
                SKU = sku,
                Description = "Some existing catalog"
            };

            var newCatalog = new Catalog()
            {
                SKU = sku,
                Description = "Some new catalog"
            };

            var company = new Company("A", new List<Catalog>() { existingCatalog }, new List<SupplierProductBarcode>(), new List<Supplier>());

            // Act & Assert
            Assert.Throws<Exception>(() => company.InsertCatalog(newCatalog));
        }

        [Fact]
        public void DeleteCatalog_WhenCatalogDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var company = new Company("A", new List<Catalog>(), new List<SupplierProductBarcode>(), new List<Supplier>());

            // Act & Assert
            Assert.Throws<Exception>(() => company.DeleteCatalog("123-abc-789"));
        }

        [Fact]
        public void DeleteCatalog_WhenSupplierProductBarcodeExists_ShouldThrowException()
        {
            // Arrange
            var sku = "123-abc-789";

            var catalog = new Catalog()
            {
                SKU = sku,
                Description = "2x4 Timber"
            };

            var supplier = new Supplier()
            {
                ID = 1
            };

            var supplierProductBarcode = new SupplierProductBarcode()
            {
                SKU = sku,
                Barcode = "X1111",
                SupplierID = 1
            };

            var company = new Company("A", 
                new List<Catalog>() { catalog }, 
                new List<SupplierProductBarcode>() { supplierProductBarcode },
                new List<Supplier>(){ supplier });

            // Act & Assert
            Assert.Throws<Exception>(() => company.DeleteCatalog(sku));
        }
    }
}
