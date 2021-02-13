namespace BunningsCodeSkillsChallenge.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Interfaces.Models;
    using Domain.Models.Entities;
    using Domain.Services;
    using Moq;
    using Xunit;

    public class SupplierProductBarcodeServiceTests
    {
        [Fact]
        public void GetSupplierProductBarcodesForProduct_WhenValid_ShouldReturnSuccessfully()
        {
            // Arrange
            var sku = "123-abc-xyz";
            var barcode = "17987128974";
            var supplierID = 5;

            var existingSupplierProductBarcodes = new List<SupplierProductBarcode>()
            {
                new SupplierProductBarcode()
                {
                    Barcode = barcode,
                    SKU = sku,
                    SupplierID = supplierID
                }
            };

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.SupplierProductBarcodes).Returns(existingSupplierProductBarcodes);

            var supplierProductBarcodeService = new SupplierProductBarcodeService(null);

            // Act
            var supplierProductBarcodes = supplierProductBarcodeService.GetSupplierProductBarcodesForProduct(mockCompany.Object, sku);

            // Assert
            Assert.Single(supplierProductBarcodes);

            var supplierProductBarcode = supplierProductBarcodes.Single();
            Assert.Equal(sku, supplierProductBarcode.SKU);
            Assert.Equal(barcode, supplierProductBarcode.Barcode);
            Assert.Equal(supplierID, supplierProductBarcode.SupplierID);

            mockCompany.Verify();
        }

        [Fact]
        public void InsertSupplierProductBarcodes_WhenValid_ShouldInsertBarcodes()
        {
            // Arrange
            var supplierId = 1;
            var sku = "123-abc-xyz";
            var expectedBarcodes = new[]
            {
                "x111111"
            };

            var expectedSupplierProductBarcode = new SupplierProductBarcode()
            {
                SKU = sku,
                SupplierID = supplierId,
                Barcode = expectedBarcodes.First()
            };

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.InsertSupplierProductBarcode(It.Is<SupplierProductBarcode>(b =>
                    b.SKU == sku &&
                    b.Barcode == expectedBarcodes.First()
                    && b.SupplierID == supplierId)))
                .Returns(expectedSupplierProductBarcode);

            var supplierProductBarcodeService = new SupplierProductBarcodeService(null);

            // Act
            var actualSupplierProductBarcodes = supplierProductBarcodeService.
                InsertSupplierProductBarcodes(mockCompany.Object, supplierId, sku, expectedBarcodes);

            // Assert
            Assert.Single(actualSupplierProductBarcodes);
            Assert.Equal(expectedSupplierProductBarcode, actualSupplierProductBarcodes.First());
            mockCompany.Verify();
        }
    }
}
