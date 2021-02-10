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
            var skusTooIgnore = new HashSet<int>();

            foreach (var company in companies)
            {
                var supplierProductBarcodes = company.SupplierProductBarcodes.GroupBy(_ => _.SKU);
                foreach (var supplierProductBarcode in supplierProductBarcodes)
                {
                    if (skusTooIgnore.Contains(company.Name.GetHashCode() ^ supplierProductBarcode.Key.GetHashCode()))
                        continue;

                    commonCatalogs.Add(new CommonCatalog()
                    {
                        SKU = supplierProductBarcode.Key,
                        Description = company.Catalogs.First(_ => _.SKU == supplierProductBarcode.Key).Description,
                        Source = company.Name
                    });

                    var otherCompanies = companies.Where(_ => _ != company);

                    foreach (var barcode in supplierProductBarcode.Select(_ => _.Barcode))
                    {
                        foreach (var otherCompany in otherCompanies)
                        {
                            var matchingBarcodes = otherCompany.SupplierProductBarcodes.Where(_ => _.Barcode == barcode);
                            foreach (var matchingBarcode in matchingBarcodes)
                            {
                                skusTooIgnore.Add(otherCompany.Name.GetHashCode() ^ matchingBarcode.SKU.GetHashCode());
                            }
                        }
                    }
                }
            }

            return commonCatalogs;
        }
    }
}
