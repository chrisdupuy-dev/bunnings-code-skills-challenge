namespace BunningsCodeSkillsChallenge.Domain.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Models;
    using Interfaces.Services;
    using Microsoft.Extensions.Logging;
    using Models.Entities;

    public class SupplierProductBarcodeService : ISupplierProductBarcodeService
    {
        private readonly ILogger _logger;

        public SupplierProductBarcodeService(ILogger<SupplierProductBarcodeService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<SupplierProductBarcode> GetSupplierProductBarcodesForProduct(ICompany company, string sku)
        {
            return company.SupplierProductBarcodes.Where(_ => _.SKU == sku);
        }

        public IEnumerable<SupplierProductBarcode> InsertSupplierProductBarcodes(ICompany company, int supplierId, string sku, IEnumerable<string> barcodes)
        {
            var insertedSupplierProductBarcodes = new List<SupplierProductBarcode>();
            foreach (var barcode in barcodes)
            {
                var newSupplierProductBarcode = new SupplierProductBarcode()
                {
                    SupplierID = supplierId,
                    SKU = sku,
                    Barcode = barcode
                };

                insertedSupplierProductBarcodes.Add(company.InsertSupplierProductBarcode(newSupplierProductBarcode));
            }

            return insertedSupplierProductBarcodes;
        }
    }
}
