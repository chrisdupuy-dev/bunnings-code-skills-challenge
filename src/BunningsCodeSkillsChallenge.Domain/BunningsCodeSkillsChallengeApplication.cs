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
        private readonly ICommonCatalogService _commonCatalogService;
        private readonly ICompanyManager _companyManager;
        private readonly ICatalogService _catalogService;
        private readonly ISupplierService _supplierService;
        private readonly ISupplierProductBarcodeService _supplierProductBarcodeService;

        private IEnumerable<CommonCatalog> _commonCatalogs { get; set; }

        public BunningsCodeSkillsChallengeApplication(ILogger<BunningsCodeSkillsChallengeApplication> logger, 
            IImportExportService importExport, ICommonCatalogService commonCatalogService, ICompanyManager companyManager, 
            ICatalogService catalogService, ISupplierService supplierService, ISupplierProductBarcodeService supplierProductBarcodeService)
        {
            _logger = logger;
            _importExport = importExport;
            _commonCatalogService = commonCatalogService;
            _companyManager = companyManager;
            _catalogService = catalogService;
            _supplierService = supplierService;
            _supplierProductBarcodeService = supplierProductBarcodeService;
            _commonCatalogs = Enumerable.Empty<CommonCatalog>();
        }

        public void ImportCompany(string name, string suppliersLocation, string catalogsLocation, string supplierProductBarcodesLocation)
        {
            var company = _importExport.ImportCompany(name, suppliersLocation, catalogsLocation, supplierProductBarcodesLocation);
            
            _companyManager.AddCompany(company);

            ReloadCommonCatalogs();
        }

        public void ExportCommonCatalog(string exportLocation)
        {
            _importExport.ExportCommonCatalog(_commonCatalogs, exportLocation);
        }

        public IEnumerable<CommonCatalog> GetCommonCatalogs()
        {
            return _commonCatalogs;
        }

        public Catalog GetCatalog(string companyName, string sku)
        {
            var company = _companyManager.GetCompany(companyName);

            return _catalogService.GetCatalog(company, sku);
        }

        public Catalog InsertCatalog(string companyName, string sku, string description)
        {
            var company = _companyManager.GetCompany(companyName);

            var productAdded = _catalogService.InsertCatalog(company, sku, description);

            ReloadCommonCatalogs();

            return productAdded;
        }

        public void DeleteCatalog(string companyName, string sku)
        {
            var company = _companyManager.GetCompany(companyName);

            _catalogService.DeleteCatalog(company, sku);

            ReloadCommonCatalogs();
        }

        public IEnumerable<Supplier> GetSuppliers(string companyName)
        {
            var company = _companyManager.GetCompany(companyName);

            return _supplierService.GetSuppliers(company);
        }

        public Supplier InsertSupplier(string companyName, string supplierName)
        {
            var company = _companyManager.GetCompany(companyName);

            return _supplierService.InsertSupplier(company, supplierName);
        }

        public IEnumerable<SupplierProductBarcode> InsertSupplierProductBarcodes(string companyName, string sku, int supplierId, IEnumerable<string> barcodes)
        {
            var company = _companyManager.GetCompany(companyName);

            var barcodesAdded = _supplierProductBarcodeService.InsertSupplierProductBarcodes(company, supplierId, sku, barcodes);

            ReloadCommonCatalogs();

            return barcodesAdded;
        }

        private void ReloadCommonCatalogs()
        {
            _commonCatalogs = _commonCatalogService.GetCommonCatalogs(_companyManager.GetAllCompanies());
        }
    }
}
