namespace BunningsCodeSkillsChallenge.Domain.Models
{
    using System.Collections.Generic;
    public class CommonCatalog
    {
        public IEnumerable<CommonCatalogItem> CommonCatalogItems { get; }

        public CommonCatalog(IEnumerable<CommonCatalogItem> commonCatalogItems)
        {
            CommonCatalogItems = commonCatalogItems;
        }
    }
}
