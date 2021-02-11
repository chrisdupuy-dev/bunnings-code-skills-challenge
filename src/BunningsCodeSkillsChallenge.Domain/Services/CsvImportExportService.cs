namespace BunningsCodeSkillsChallenge.Domain.Services
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using CsvHelper;
    using Interfaces.Services;
    using Models;
    using Models.Entities;

    public class CsvImportExportService : IImportExportService
    {
        public Company ImportCompany(string companyName, string suppliersLocation, string catalogsLocation, string supplierProductBarcodesLocation)
        {
            var suppliers = Read<Supplier>(suppliersLocation);
            var supplierProductBarcodes = Read<SupplierProductBarcode>(supplierProductBarcodesLocation);
            var catalogs = Read<Catalog>(catalogsLocation);

            return new Company(companyName, catalogs, supplierProductBarcodes, suppliers);
        }

        public void ExportCommonCatalog(CommonCatalog commonCatalog, string destinationLocation)
        {
            Write<CommonCatalogItem>(destinationLocation, commonCatalog.CommonCatalogItems.OrderBy(_ => _.Description));
        }

        private List<T> Read<T>(string location)
        {
            using (var reader = new StreamReader(location))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    return csv.GetRecords<T>().ToList();
                }
            }
        }

        private void Write<T>(string location, IEnumerable<T> records)
        {
            using (var writer = new StreamWriter(location))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
            }
        }
    }
}
