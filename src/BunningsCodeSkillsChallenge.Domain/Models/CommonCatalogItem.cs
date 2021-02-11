namespace BunningsCodeSkillsChallenge.Domain.Models
{
    public class CommonCatalogItem
    {
        public string SKU { get; }
        public string Description { get; }
        public string Source { get; }

        public CommonCatalogItem(string sku, string description, string source)
        {
            SKU = sku;
            Description = description;
            Source = source;
        }
    }
}
