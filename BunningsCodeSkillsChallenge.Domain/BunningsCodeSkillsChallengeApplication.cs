namespace BunningsCodeSkillsChallenge.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Interfaces.Services;
    using Microsoft.Extensions.Logging;
    using Models;
    using Models.Entities;

    public class BunningsCodeSkillsChallengeApplication : IApplication
    {
        private readonly ILogger _logger;
        private readonly IImportExportService _importExport;
        private readonly IMegaMergerService _megaMergerService;
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;

        private List<Company> _companies { get; set; }
        private CommonCatalog _commonCatalog { get; set; }

        public BunningsCodeSkillsChallengeApplication(ILogger<BunningsCodeSkillsChallengeApplication> logger, 
            IImportExportService importExport, IMegaMergerService megaMergerService, IProductService productService, 
            ISupplierService supplierService)
        {
            _logger = logger;
            _importExport = importExport;
            _megaMergerService = megaMergerService;
            _productService = productService;
            _supplierService = supplierService;
            _commonCatalog = new CommonCatalog(Enumerable.Empty<CommonCatalogItem>());
            _companies = new List<Company>();
        }

        public void ImportCompany(string name, string suppliersLocation, string catalogsLocation, string supplierProductBarcodesLocation)
        {
            var company = _importExport.ImportCompany(name, suppliersLocation, catalogsLocation, supplierProductBarcodesLocation);

            _companies.Add(company);

            ReloadCommonCatalog();
        }

        public void ExportCommonCatalog(string exportLocation)
        {
            _importExport.ExportCommonCatalog(_commonCatalog, exportLocation);
        }

        public CommonCatalog GetCommonCatalog()
        {
            return _commonCatalog;
        }

        public Catalog GetProduct(string companyName, string sku)
        {
            var company = GetCompany(companyName);

            return _productService.GetProduct(company, sku);
        }

        public void AddNewProduct(string companyName, string sku, string description)
        {
            var company = GetCompany(companyName);

            _productService.AddProduct(company, sku, description);

            ReloadCommonCatalog();
        }

        public void RemoveProduct(string companyName, string sku)
        {
            var company = GetCompany(companyName);

            _productService.RemoveProduct(company, sku);

            ReloadCommonCatalog();
        }

        public IEnumerable<Supplier> GetSuppliers(string companyName)
        {
            var company = GetCompany(companyName);

            return _supplierService.GetSuppliers(company);
        }

        public Supplier AddSupplier(string companyName, string supplierName)
        {
            var company = GetCompany(companyName);

            return _supplierService.CreateSupplier(company, supplierName);
        }

        public void AddProductBarcodes(string companyName, string sku, int supplierId, IEnumerable<string> barcodes)
        {
            var company = GetCompany(companyName);

            _productService.AddBarcodesToProduct(company, supplierId, sku, barcodes);

            ReloadCommonCatalog();
        }

        private Company GetCompany(string companyName)
        {
            var company = _companies.FirstOrDefault(_ => _.Name == companyName);
            if (company == null)
               throw new Exception("Company not found");

            return company;
        }

        private void ReloadCommonCatalog()
        {
            _commonCatalog = _megaMergerService.GetCommonCatalog(_companies);
        }
    }
}
