namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using System.Collections.Generic;
    using Models;

    public interface ICompanyManager
    {
        ICompany AddCompany(ICompany company);
        ICompany GetCompany(string name);
        IEnumerable<ICompany> GetAllCompanies();
    }
}
