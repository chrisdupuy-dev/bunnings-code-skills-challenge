﻿namespace BunningsCodeSkillsChallenge.Domain.Services
{
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using CsvHelper;
    using Interfaces.Services;
    using Models;

    public class CsvReaderService : IReaderService
    {
        public Company ReadCompany(string suppliersLocation, string supplierProductBarcodesLocation, string catalogsLocation)
        {
            var suppliers = Read<Supplier>(suppliersLocation);
            var supplierProductBarcodes = Read<SupplierProductBarcode>(supplierProductBarcodesLocation);
            var catalogs = Read<Catalog>(catalogsLocation);

            return new Company()
            {
                Catalogs = catalogs,
                SupplierProductBarcodes = supplierProductBarcodes,
                Suppliers = suppliers
            };
        }

        private T[] Read<T>(string location)
        {
            using (var reader = new StreamReader(location))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    return csv.GetRecords<T>().ToArray();
                }
            }
        }
    }
}
