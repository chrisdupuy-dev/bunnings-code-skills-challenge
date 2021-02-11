namespace BunningsCodeSkillsChallenge.Domain.Services
{
    using Interfaces.Services;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class MegaMergerService : IMegaMergerService
    {
        public CommonCatalog GetCommonCatalog(IEnumerable<Company> companies)
        {
            var commonCatalogItems = new List<CommonCatalogItem>();
            var skusTooIgnore = new HashSet<int>();

            foreach (var company in companies)
            {
                var otherCompanies = companies.Where(_ => _ != company);

                // Iterate each product in this company
                var supplierProductBarcodes = company.SupplierProductBarcodes.GroupBy(_ => _.SKU);
                foreach (var supplierProductBarcode in supplierProductBarcodes)
                {
                    // Skip this product if already in the system due to previous company (assuming company name is unique) 
                    if (skusTooIgnore.Contains(company.Name.GetHashCode() ^ supplierProductBarcode.Key.GetHashCode()))
                        continue;

                    var description = company.Catalogs.First(_ => _.SKU == supplierProductBarcode.Key).Description;
                    commonCatalogItems.Add(new CommonCatalogItem(supplierProductBarcode.Key, description, company.Name));

                    foreach (var barcode in supplierProductBarcode.Select(_ => _.Barcode))
                    {
                        foreach (var otherCompany in otherCompanies)
                        {
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

            return new CommonCatalog(commonCatalogItems);
        }
    }
}
