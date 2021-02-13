namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Models.Entities;
    using Models;

    public interface ICatalogService
    {
        Catalog GetCatalog(ICompany company, string sku);
        IEnumerable<SupplierProductBarcode> GetSupplierProductBarcodesForProduct(ICompany company, string sku);
        Catalog InsertCatalog(ICompany company, string sku, string description);
        void DeleteCatalog(ICompany company, string sku);
        IEnumerable<SupplierProductBarcode> InsertSupplierProductBarcodes(ICompany company, int supplierId, string sku, IEnumerable<string> barcodes);
    }
}
