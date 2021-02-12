namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using System.Collections.Generic;
    using Models;

    public interface ICompanyService
    {
        Company AddCompany(Company company);
        Company GetCompany(string name);
        IEnumerable<Company> GetAllCompanies();
    }
}
