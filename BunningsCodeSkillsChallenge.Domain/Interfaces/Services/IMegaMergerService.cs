namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using System.Collections.Generic;
    using Models;

    public interface IMegaMergerService
    {
        IEnumerable<CommonCatalog> GetCommonCatalog(IEnumerable<Company> companies);
    }
}
