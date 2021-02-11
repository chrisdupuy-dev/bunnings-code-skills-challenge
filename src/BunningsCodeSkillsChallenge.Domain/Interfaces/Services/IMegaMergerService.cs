namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using System.Collections.Generic;
    using Models;

    public interface IMegaMergerService
    {
        CommonCatalog GetCommonCatalog(IEnumerable<Company> companies);
    }
}
