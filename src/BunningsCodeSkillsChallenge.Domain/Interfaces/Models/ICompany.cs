namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Models
{
    using System.Collections.Generic;
    using Domain.Models.Entities;

    public interface ICompany
    {
        string Name { get; }

        IEnumerable<Catalog> Catalogs { get; }
        IEnumerable<SupplierProductBarcode> SupplierProductBarcodes { get; }
        IEnumerable<Supplier> Suppliers { get; }

        Supplier InsertSupplier(Supplier newSupplier);
        Catalog InsertCatalog(Catalog newCatalog);
        void DeleteCatalog(string sku);
        SupplierProductBarcode InsertSupplierProductBarcode(SupplierProductBarcode supplierProductBarcode);
        void DeleteSupplierProductBarcodes(string sku);
    }
}
