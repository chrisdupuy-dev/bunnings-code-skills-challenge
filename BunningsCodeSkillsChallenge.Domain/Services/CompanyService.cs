namespace BunningsCodeSkillsChallenge.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Services;
    using Models;
    using Models.Entities;

    public class CompanyService : ICompanyService
    {
        private readonly Company _company;

        public CompanyService(Company company)
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

            _company.Catalogs.Add(catalog);
        }

        public void RemoveProduct(string sku)
        {
            var catalogToRemove = _company.Catalogs.FirstOrDefault(_ => _.SKU == sku);
            if (catalogToRemove == null)
                throw new Exception();

            _company.SupplierProductBarcodes.RemoveAll(_ => _.SKU == sku);
            _company.Catalogs.Remove(catalogToRemove);
        }

        public Catalog GetProduct(string sku)
        {
            return _company.Catalogs.FirstOrDefault(_ => _.SKU == sku);
        }

        public IEnumerable<SupplierProductBarcode> GetSupplierProductBarcodesForProduct(string sku)
        {
            return _company.SupplierProductBarcodes.Where(_ => _.SKU == sku);
        }
    }
}
