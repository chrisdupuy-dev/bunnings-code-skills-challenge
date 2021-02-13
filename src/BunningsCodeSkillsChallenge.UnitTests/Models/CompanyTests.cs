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
    }
}
