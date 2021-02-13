namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Models;
    using Models;

    public interface ICommonCatalogService
    {
        IEnumerable<CommonCatalog> GetCommonCatalogs(IEnumerable<ICompany> companies);
    }
}
