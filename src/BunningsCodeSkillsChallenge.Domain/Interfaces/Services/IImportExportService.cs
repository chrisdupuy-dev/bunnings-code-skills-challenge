namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using Models;

    public interface IImportExportService
    {
        Company ImportCompany(string companyName, string suppliersLocation, string catalogsLocation, string supplierProductBarcodesLocation);
        void ExportCommonCatalog(CommonCatalog commonCatalog, string destinationLocation);
    }
}
