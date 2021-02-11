namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using System.Collections.Generic;
    using Models.Entities;

    public interface IProductService
    {
        Catalog GetProduct(string sku);
        IEnumerable<SupplierProductBarcode> GetSupplierProductBarcodesForProduct(string sku);
        void AddProduct(string sku, string description);
        void RemoveProduct(string sku);
        void AddBarcodesToProduct(int supplierId, string sku, IEnumerable<string> barcodes);
    }
}
