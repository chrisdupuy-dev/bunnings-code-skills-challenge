namespace BunningsCodeSkillsChallenge.Domain.Services
{
    using Interfaces.Services;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class MegaMergerService : IMegaMergerService
    {
        public IEnumerable<CommonCatalog> GetCommonCatalog(IEnumerable<Company> companies)
        {
            var commonCatalogs = new List<CommonCatalog>();

            var companyA = companies.First();
            var companyB = companies.Last();

            var supplierProductBarcodesA = companyA.SupplierProductBarcodes.GroupBy(_ => _.SKU);
            var supplierProductBarcodesB = companyB.SupplierProductBarcodes.GroupBy(_ => _.SKU);

            var skusTooIgnore = new List<string>();

            foreach (var supplierProductBarcodeA in supplierProductBarcodesA)
            {
                commonCatalogs.Add(new CommonCatalog()
                {
                    SKU = supplierProductBarcodeA.Key,
                    Description = companyA.Catalogs.First(_ => _.SKU == supplierProductBarcodeA.Key).Description,
                    Source = companyA.Name
                });

                var barcodesA = supplierProductBarcodeA.Select(_ => _.Barcode);

                foreach (var supplierProductBarcodeB in supplierProductBarcodesB)
                {
                    var barcodesB = supplierProductBarcodeB.Select(_ => _.Barcode);
                    foreach (var barcodeB in barcodesB)
                    {
                        if (barcodesA.Any(_ => _ == barcodeB))
                        {
                            skusTooIgnore.Add(supplierProductBarcodeB.Key);
                        }
                    }
                }
            }

            foreach (var supplierProductBarcodeB in supplierProductBarcodesB.Where(_ => !skusTooIgnore.Contains(_.Key)))
            {
                commonCatalogs.Add(new CommonCatalog()
                {
                    SKU = supplierProductBarcodeB.Key,
                    Description = companyB.Catalogs.First(_ => _.SKU == supplierProductBarcodeB.Key).Description,
                    Source = companyB.Name
                });
            }

            return commonCatalogs;
        }
    }
}
