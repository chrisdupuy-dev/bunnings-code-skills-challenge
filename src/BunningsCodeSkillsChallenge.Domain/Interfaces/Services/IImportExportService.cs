namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Models;
    using Models;

    public interface IImportExportService
    {
        ICompany ImportCompany(string companyName, string suppliersLocation, string catalogsLocation, string supplierProductBarcodesLocation);
        void ExportCommonCatalog(IEnumerable<CommonCatalog> commonCatalogs, string destinationLocation);
    }
}
