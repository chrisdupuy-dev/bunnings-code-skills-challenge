namespace BunningsCodeSkillsChallenge.Domain
{
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
        private readonly ICompanyService _companyService;
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;

        private CommonCatalog _commonCatalog { get; set; }

        public BunningsCodeSkillsChallengeApplication(ILogger<BunningsCodeSkillsChallengeApplication> logger, 
            IImportExportService importExport, IMegaMergerService megaMergerService, ICompanyService companyService, IProductService productService, 
            ISupplierService supplierService)
        {
            _logger = logger;
            _importExport = importExport;
            _megaMergerService = megaMergerService;
            _companyService = companyService;
            _productService = productService;
            _supplierService = supplierService;
            _commonCatalog = new CommonCatalog(Enumerable.Empty<CommonCatalogItem>());
        }

        public void ImportCompany(string name, string suppliersLocation, string catalogsLocation, string supplierProductBarcodesLocation)
        {
            var company = _importExport.ImportCompany(name, suppliersLocation, catalogsLocation, supplierProductBarcodesLocation);
            
            _companyService.AddCompany(company);

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
            var company = _companyService.GetCompany(companyName);

            return _productService.GetProduct(company, sku);
        }

        public Catalog AddNewProduct(string companyName, string sku, string description)
        {
            var company = _companyService.GetCompany(companyName);

            var productAdded = _productService.AddProduct(company, sku, description);

            ReloadCommonCatalog();

            return productAdded;
        }

        public void RemoveProduct(string companyName, string sku)
        {
            var company = _companyService.GetCompany(companyName);

            _productService.RemoveProduct(company, sku);

            ReloadCommonCatalog();
        }

        public IEnumerable<Supplier> GetSuppliers(string companyName)
        {
            var company = _companyService.GetCompany(companyName);

            return _supplierService.GetSuppliers(company);
        }

        public Supplier AddSupplier(string companyName, string supplierName)
        {
            var company = _companyService.GetCompany(companyName);

            return _supplierService.CreateSupplier(company, supplierName);
        }

        public IEnumerable<SupplierProductBarcode> AddProductBarcodes(string companyName, string sku, int supplierId, IEnumerable<string> barcodes)
        {
            var company = _companyService.GetCompany(companyName);

            var barcodesAdded = _productService.AddBarcodesToProduct(company, supplierId, sku, barcodes);

            ReloadCommonCatalog();

            return barcodesAdded;
        }

        private void ReloadCommonCatalog()
        {
            _commonCatalog = _megaMergerService.GetCommonCatalog(_companyService.GetAllCompanies());
        }
    }
}
