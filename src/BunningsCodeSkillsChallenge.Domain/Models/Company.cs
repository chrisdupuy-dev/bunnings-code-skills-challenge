namespace BunningsCodeSkillsChallenge.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using Interfaces.Models;

    public class Company : ICompany
    {
        public string Name { get; }

        public IEnumerable<Catalog> Catalogs { get; private set; }
        public IEnumerable<SupplierProductBarcode> SupplierProductBarcodes { get; private set; }
        public IEnumerable<Supplier> Suppliers { get; private set; }

        public Company(string name, IEnumerable<Catalog> catalogs, 
            IEnumerable<SupplierProductBarcode> supplierProductBarcodes, IEnumerable<Supplier> suppliers)
        {
            ValidateSupplierProductBarcodes(catalogs, supplierProductBarcodes, suppliers);

            Name = name;
            Catalogs = catalogs;
            SupplierProductBarcodes = supplierProductBarcodes;
            Suppliers = suppliers;
        }

        private void ValidateSupplierProductBarcodes(IEnumerable<Catalog> catalogs, 
            IEnumerable<SupplierProductBarcode> supplierProductBarcodes, IEnumerable<Supplier> suppliers)
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
            if (Catalogs.Any(_ => _.SKU == newCatalog.SKU))
                throw new Exception("This SKU already exists");

            Catalogs = Catalogs.Union(new[] { newCatalog });

            return newCatalog;
        }

        public void DeleteCatalog(string sku)
        {
            if (SupplierProductBarcodes.Any(_ => _.SKU == sku))
                throw new Exception("Barcodes found for product");

            var catalogToRemove = Catalogs.FirstOrDefault(_ => _.SKU == sku);
            if (catalogToRemove == null)
                throw new Exception("SKU does not exist");

            Catalogs = Catalogs.Except(new[] { catalogToRemove });
        }

        public SupplierProductBarcode InsertSupplierProductBarcode(SupplierProductBarcode supplierProductBarcode)
        {
            if (Suppliers.All(_ => _.ID != supplierProductBarcode.SupplierID))
                throw new Exception("Supplier does not exist");

            if (Catalogs.All(_ => _.SKU != supplierProductBarcode.SKU))
                throw new Exception("SKU does not exist");

            SupplierProductBarcodes = SupplierProductBarcodes.Union(new []{ supplierProductBarcode });

            return supplierProductBarcode;
        }

        public void DeleteSupplierProductBarcodes(string sku)
        {
            SupplierProductBarcodes = SupplierProductBarcodes.Where(_ => _.SKU != sku);
        }
    }
}
