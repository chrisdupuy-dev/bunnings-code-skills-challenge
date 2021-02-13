namespace BunningsCodeSkillsChallenge.Domain.Services
{
    using System;
    using Interfaces.Services;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Models;
    using Microsoft.Extensions.Logging;

    public class CommonCatalogService : ICommonCatalogService
    {
        private readonly ILogger _logger;

        public CommonCatalogService(ILogger<CommonCatalogService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<CommonCatalog> GetCommonCatalogs(IEnumerable<ICompany> companies)
        {
            var commonCatalogItems = new List<CommonCatalog>();
            var skusTooIgnore = new HashSet<int>();

            foreach (var company in companies)
            {
                var otherCompanies = companies.Where(_ => _ != company);

                // Iterate each product in this company
                var groupedSupplierProductBarcodes = company.SupplierProductBarcodes.GroupBy(_ => _.SKU);
                foreach (var groupSupplierProductBarcode in groupedSupplierProductBarcodes)
                {
                    // Skip this product if already in the system due to previous company (assuming company name is unique) 
                    if (skusTooIgnore.Contains(company.Name.GetHashCode() ^ groupSupplierProductBarcode.Key.GetHashCode()))
                        continue;

                    var description = company.Catalogs.First(_ => _.SKU == groupSupplierProductBarcode.Key).Description;
                    commonCatalogItems.Add(new CommonCatalog(groupSupplierProductBarcode.Key, description, company.Name));

                    foreach (var barcode in groupSupplierProductBarcode.Select(_ => _.Barcode))
                    {
                        foreach (var otherCompany in otherCompanies)
                        {
                            CheckForConflictingSku(otherCompany, groupSupplierProductBarcode.Key, barcode);

                            // Iterate other companies products looking for matching barcodes to determine if matching products exist
                            // so that duplicates are not added to the catalog, could be improved by stopping when matching SKU is found
                            // for a barcode
                            var matchingBarcodes = otherCompany.SupplierProductBarcodes.Where(_ => _.Barcode == barcode);
                            foreach (var matchingBarcode in matchingBarcodes)
                            {
                                skusTooIgnore.Add(otherCompany.Name.GetHashCode() ^ matchingBarcode.SKU.GetHashCode());
                            }
                        }
                    }
                }
            }

            return commonCatalogItems;
        }

        private void CheckForConflictingSku(ICompany otherCompany, string sku, string barcode)
        {
            var sameSkuBarcodes = otherCompany.SupplierProductBarcodes.Where(_ => _.SKU == sku);
            if (sameSkuBarcodes.Any() && !sameSkuBarcodes.Any(_ => _.Barcode == barcode))
                throw new Exception("Conflicting SKUs found");
        }
    }
}
