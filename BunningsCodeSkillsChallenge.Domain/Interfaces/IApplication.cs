namespace BunningsCodeSkillsChallenge.Domain.Interfaces
{
    public interface IApplication
    {
        void ImportCompany(string name, string suppliersLocation, string catalogsLocation, string supplierProductBarcodesLocation);

        void ExportCommonCatalog(string exportLocation);
    }
}
