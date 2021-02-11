namespace BunningsCodeSkillsChallenge.Domain
{
    using System.Collections.Generic;
    using Interfaces;
    using Interfaces.Services;
    using Microsoft.Extensions.Logging;
    using Models;

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
            _commonCatalog = new CommonCatalog();
            _companies = new List<Company>();
        }

        public void ImportCompany(string name, string suppliersLocation, string catalogsLocation, string supplierProductBarcodesLocation)
        {
            var company = _importExport.ImportCompany(name, suppliersLocation, catalogsLocation, supplierProductBarcodesLocation);

            _companies.Add(company);

            _commonCatalog = _megaMergerService.GetCommonCatalog(_companies);
        }

        public void ExportCommonCatalog(string exportLocation)
        {
            _importExport.ExportCommonCatalog(_commonCatalog, exportLocation);
        }
    }
}
