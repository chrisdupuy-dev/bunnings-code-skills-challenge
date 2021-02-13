namespace BunningsCodeSkillsChallenge.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices.ComTypes;
    using System.Text;
    using Entities;

    public class Company
    {
        public string Name { get; }

        public IEnumerable<Catalog> Catalogs { get; private set; }
        public IEnumerable<SupplierProductBarcode> SupplierProductBarcodes { get; private set; }
        public IEnumerable<Supplier> Suppliers { get; private set; }

        public Company(string name, IEnumerable<Catalog> catalogs, IEnumerable<SupplierProductBarcode> supplierProductBarcodes, IEnumerable<Supplier> suppliers)
        {
            ValidateSupplierProductBarcodes(catalogs, supplierProductBarcodes, suppliers);

            Name = name;
            Catalogs = catalogs;
            SupplierProductBarcodes = supplierProductBarcodes;
            Suppliers = suppliers;
        }

        private void ValidateSupplierProductBarcodes(IEnumerable<Catalog> catalogs, IEnumerable<SupplierProductBarcode> supplierProductBarcodes, IEnumerable<Supplier> suppliers)
        {
            foreach (var supplierProductBarcode in supplierProductBarcodes)
            {
                if (!catalogs.Any(_ => _.SKU == supplierProductBarcode.SKU))
                    throw new Exception("No matching SKU found in catalogs");

                if (!suppliers.Any(_ => _.ID == supplierProductBarcode.SupplierID))
                    throw new Exception("No matching supplier ID in suppliers");
            }
        }

        public Supplier InsertSupplier(Supplier newSupplier)
        {
            int newId = Suppliers.Any() ? Suppliers.Max(_ => _.ID) + 1 : 1;
            newSupplier.ID = newId;

            Suppliers = Suppliers.Union(new[] {newSupplier});

            return newSupplier;
        }

        public Catalog InsertCatalog(Catalog newCatalog)
        {
            Catalogs = Catalogs.Union(new[] { newCatalog });

            return newCatalog;
        }

        public void RemoveCatalog(Catalog catalogToRemove)
        {
            Catalogs = Catalogs.Except(new[] { catalogToRemove });
        }

        public SupplierProductBarcode InsertSupplierProductBarcode(SupplierProductBarcode supplierProductBarcode)
        {
            SupplierProductBarcodes = SupplierProductBarcodes.Union(new []{ supplierProductBarcode });

            return supplierProductBarcode;
        }

        public void RemoveSupplierProductBarcodes(IEnumerable<SupplierProductBarcode> supplierProductBarcodes)
        {
            SupplierProductBarcodes = SupplierProductBarcodes.Except(supplierProductBarcodes);
        }
    }
}
