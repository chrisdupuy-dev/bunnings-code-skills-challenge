namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using System.Collections.Generic;
    using Models;

    public interface ICompanyService
    {
        Catalog GetProduct(string sku);
        IEnumerable<SupplierProductBarcode> GetSupplierProductBarcodesForProduct(string sku);
        void AddProduct(string sku, string description);
        void RemoveProduct(string sku);
    }
}
