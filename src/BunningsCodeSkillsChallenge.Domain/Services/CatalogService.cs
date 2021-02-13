namespace BunningsCodeSkillsChallenge.Domain.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Models;
    using Interfaces.Services;
    using Microsoft.Extensions.Logging;
    using Models.Entities;

    public class CatalogService : ICatalogService
    {
        private readonly ILogger _logger;
        public CatalogService(ILogger<CatalogService> logger)
        {
            _logger = logger;
        }

        public Catalog InsertCatalog(ICompany company, string sku, string description)
        {
            var catalog = new Catalog
            {
                SKU = sku,
                Description = description
            };

            return company.InsertCatalog(catalog);
        }

        public void DeleteCatalog(ICompany company, string sku)
        {
            company.DeleteSupplierProductBarcodes(sku);
            company.DeleteCatalog(sku);
        }

        public Catalog GetCatalog(ICompany company, string sku)
        {
            return company.Catalogs.FirstOrDefault(_ => _.SKU == sku);
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
