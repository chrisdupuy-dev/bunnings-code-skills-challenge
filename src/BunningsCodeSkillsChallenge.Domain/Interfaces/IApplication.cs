namespace BunningsCodeSkillsChallenge.Domain.Interfaces
{
    using System.Collections.Generic;
    using Domain.Models;
    using Domain.Models.Entities;

    public interface IApplication
    {
        void ImportCompany(string name, string suppliersLocation, string catalogsLocation, string supplierProductBarcodesLocation);
        void ExportCommonCatalog(string exportLocation);
        IEnumerable<CommonCatalog> GetCommonCatalogs();
        Catalog GetCatalog(string companyName, string sku);
        Catalog InsertCatalog(string companyName, string sku, string description);
        void DeleteCatalog(string companyName, string sku);
        IEnumerable<Supplier> GetSuppliers(string companyName);
        Supplier InsertSupplier(string companyName, string supplierName);
        IEnumerable<SupplierProductBarcode> InsertSupplierProductBarcodes(string companyName, string sku, int supplierId, IEnumerable<string> barcodes);
    }
}
