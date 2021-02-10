namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using Models;

    public interface IReaderService
    {
        Company ReadCompany(string companyName, string catalogsLocation, string supplierProductBarcodesLocation, string suppliersLocation);
    }
}
