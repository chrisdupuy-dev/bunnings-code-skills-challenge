namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using System.Collections.Generic;
    using Models;

    public interface IReaderService
    {
        Company ReadCompany(string catalogsLocation, string supplierProductBarcodesLocation, string suppliersLocation);
    }
}
