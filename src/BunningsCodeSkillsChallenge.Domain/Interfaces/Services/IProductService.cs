namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using System.Collections.Generic;
    using Models;
    using Models.Entities;

    public interface IProductService
    {
        Catalog GetProduct(Company company, string sku);
        IEnumerable<SupplierProductBarcode> GetSupplierProductBarcodesForProduct(Company company, string sku);
        Catalog AddProduct(Company company, string sku, string description);
        void RemoveProduct(Company company, string sku);
        IEnumerable<SupplierProductBarcode> AddBarcodesToProduct(Company company, int supplierId, string sku, IEnumerable<string> barcodes);
    }
}
