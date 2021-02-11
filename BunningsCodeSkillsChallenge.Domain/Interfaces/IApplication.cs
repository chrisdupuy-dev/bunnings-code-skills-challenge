namespace BunningsCodeSkillsChallenge.Domain.Interfaces
{
    using System.Collections.Generic;
    using Models;
    using Models.Entities;

    public interface IApplication
    {
        void ImportCompany(string name, string suppliersLocation, string catalogsLocation, string supplierProductBarcodesLocation);
        void ExportCommonCatalog(string exportLocation);
        CommonCatalog GetCommonCatalog();
        Catalog GetProduct(string companyName, string sku);
        void AddNewProduct(string companyName, string sku, string description);
        void RemoveProduct(string companyName, string sku);
        Supplier AddSupplier(string companyName, string supplierName);
        IEnumerable<Supplier> GetSuppliers(string companyName);
        void AddProductBarcodes(string companyName, string sku, int supplierId, IEnumerable<string> barcodes);
    }
}
