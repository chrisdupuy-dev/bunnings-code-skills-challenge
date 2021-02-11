namespace BunningsCodeSkillsChallenge.Domain.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;

    public class Company
    {
        public string Name { get; private set; }

        public IEnumerable<Catalog> Catalogs { get; private set; }
        public IEnumerable<SupplierProductBarcode> SupplierProductBarcodes { get; private set; }
        public IEnumerable<Supplier> Suppliers { get; private set; }

        public Company(string name, IEnumerable<Catalog> catalogs, IEnumerable<SupplierProductBarcode> supplierProductBarcodes, IEnumerable<Supplier> suppliers)
        {
            Name = name;
            Catalogs = catalogs;
            SupplierProductBarcodes = supplierProductBarcodes;
            Suppliers = suppliers;
        }

        public Supplier InsertSupplier(Supplier newSupplier)
        {
            var newId = Suppliers.Max(_ => _.ID) + 1;
            newSupplier.ID = newId;

            Suppliers = Suppliers.Union(new[] {newSupplier});

            return newSupplier; // Probably should create new object
        }

        public Catalog InsertCatalog(Catalog newCatalog)
        {
            Catalogs = Catalogs.Union(new[] { newCatalog });

            return newCatalog; // Probably should create new object
        }

        public void RemoveCatalog(Catalog catalogToRemove)
        {
            Catalogs = Catalogs.Except(new[] { catalogToRemove });
        }

        public void InsertSupplierProductBarcode(SupplierProductBarcode supplierProductBarcode)
        {
            SupplierProductBarcodes = SupplierProductBarcodes.Union(new []{ supplierProductBarcode });
        }

        public void RemoveSupplierProductBarcodes(IEnumerable<SupplierProductBarcode> supplierProductBarcodes)
        {
            SupplierProductBarcodes = SupplierProductBarcodes.Except(supplierProductBarcodes);
        }
    }
}
