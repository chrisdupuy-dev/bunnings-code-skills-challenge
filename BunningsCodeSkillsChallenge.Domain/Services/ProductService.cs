namespace BunningsCodeSkillsChallenge.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Services;
    using Models;
    using Models.Entities;

    public class ProductService : IProductService
    {
        private readonly Company _company;

        public ProductService(Company company)
        {
            _company = company;
        }

        public void AddProduct(string sku, string description)
        {
            var catalog = new Catalog()
            {
                SKU = sku,
                Description = description
            };

            _company.InsertCatalog(catalog);
        }

        public void RemoveProduct(string sku)
        {
            var catalogToRemove = _company.Catalogs.FirstOrDefault(_ => _.SKU == sku);
            if (catalogToRemove == null)
                throw new Exception();

            var supplierProductCodes = _company.SupplierProductBarcodes.Where(_ => _.SKU == sku);
            _company.RemoveSupplierProductBarcodes(supplierProductCodes);
            _company.RemoveCatalog(catalogToRemove);
        }

        public Catalog GetProduct(string sku)
        {
            return _company.Catalogs.FirstOrDefault(_ => _.SKU == sku);
        }

        public IEnumerable<SupplierProductBarcode> GetSupplierProductBarcodesForProduct(string sku)
        {
            return _company.SupplierProductBarcodes.Where(_ => _.SKU == sku);
        }

        public void AddBarcodesToProduct(int supplierId, string sku, IEnumerable<string> barcodes)
        {
            if (!_company.Suppliers.Any(_ => _.ID == supplierId))
                throw new Exception();

            if (!_company.Catalogs.Any(_ => _.SKU == sku))
                throw new Exception();

            foreach (var barcode in barcodes)
            {
                var newSupplierProductBarcode = new SupplierProductBarcode()
                {
                    SupplierID = supplierId,
                    SKU = sku,
                    Barcode = barcode
                };

                _company.InsertSupplierProductBarcode(newSupplierProductBarcode);
            }
        }
    }
}
