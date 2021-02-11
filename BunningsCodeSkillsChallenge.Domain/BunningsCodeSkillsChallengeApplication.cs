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
    using Services;

    public class BunningsCodeSkillsChallengeApplication : IApplication
    {
        private readonly ILogger _logger;
        private readonly IImportExportService _importExport;
        private readonly IMegaMergerService _megaMergerService;

        private List<Company> _companies { get; set; }
        private CommonCatalog _commonCatalog { get; set; }

        public BunningsCodeSkillsChallengeApplication(ILogger<BunningsCodeSkillsChallengeApplication> logger, IImportExportService importExport, IMegaMergerService megaMergerService)
        {
            _logger = logger;
            _importExport = importExport;
            _megaMergerService = megaMergerService;
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

        public void AddNewProduct(string companyName, string sku, string description)
        {
            var company = _companies.FirstOrDefault(_ => _.Name == companyName);
            if (company == null)
                throw new Exception("Company not found");

            var productService = new ProductService(company);
            productService.AddProduct(sku, description);

            ReloadCommonCatalog();
        }

        public void RemoveProduct(string companyName, string sku)
        {
            var company = _companies.FirstOrDefault(_ => _.Name == companyName);
            if (company == null)
                throw new Exception("Company not found");

            var productService = new ProductService(company);
            productService.RemoveProduct(sku);

            ReloadCommonCatalog();
        }

        public IEnumerable<Supplier> GetSuppliers(string companyName)
        {
            var company = _companies.FirstOrDefault(_ => _.Name == companyName);
            if (company == null)
                throw new Exception("Company not found");

            var supplierService = new SupplierService(company);

            return supplierService.GetSuppliers();
        }

        public Supplier AddSupplier(string companyName, string supplierName)
        {
            var company = _companies.FirstOrDefault(_ => _.Name == companyName);
            if (company == null)
                throw new Exception("Company not found");

            var supplierService = new SupplierService(company);
            return supplierService.CreateSupplier(supplierName);
        }

        public void AddProductBarcodes(string companyName, string sku, int supplierId, IEnumerable<string> barcodes)
        {
            var company = _companies.FirstOrDefault(_ => _.Name == companyName);
            if (company == null)
                throw new Exception("Company not found");

            var productService = new ProductService(company);
            productService.AddBarcodesToProduct(supplierId, sku, barcodes);

            ReloadCommonCatalog();
        }

        private void ReloadCommonCatalog()
        {
            _commonCatalog = _megaMergerService.GetCommonCatalog(_companies);
        }
    }
}
