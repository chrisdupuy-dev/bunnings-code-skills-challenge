namespace BunningsCodeSkillsChallenge.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Services;
    using Microsoft.Extensions.Logging;
    using Models;
    using Models.Entities;

    public class ProductService : IProductService
    {
        private readonly ILogger _logger;
        public ProductService(ILogger<ProductService> logger)
        {
            _logger = logger;
        }

        public Catalog AddProduct(Company company, string sku, string description)
        {
            var catalog = new Catalog
            {
                SKU = sku,
                Description = description
            };

            return company.InsertCatalog(catalog);
        }

        public void RemoveProduct(Company company, string sku)
        {
            var catalogToRemove = company.Catalogs.FirstOrDefault(_ => _.SKU == sku);
            if (catalogToRemove == null)
                throw new Exception("Product does not exist");

            var supplierProductCodes = company.SupplierProductBarcodes.Where(_ => _.SKU == sku);
            company.RemoveSupplierProductBarcodes(supplierProductCodes);
            company.RemoveCatalog(catalogToRemove);
        }

        public Catalog GetProduct(Company company, string sku)
        {
            return company.Catalogs.FirstOrDefault(_ => _.SKU == sku);
        }

        public IEnumerable<SupplierProductBarcode> GetSupplierProductBarcodesForProduct(Company company, string sku)
        {
            return company.SupplierProductBarcodes.Where(_ => _.SKU == sku);
        }

        public IEnumerable<SupplierProductBarcode> AddBarcodesToProduct(Company company, int supplierId, string sku, IEnumerable<string> barcodes)
        {
            if (company.Suppliers.All(_ => _.ID != supplierId))
                throw new Exception("Supplier does not exist");

            if (company.Catalogs.All(_ => _.SKU != sku))
                throw new Exception("Product does not exist");

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
