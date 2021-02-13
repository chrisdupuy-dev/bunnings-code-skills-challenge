namespace BunningsCodeSkillsChallenge.Domain.Models
{
    public class CommonCatalog
    {
        public string SKU { get; }
        public string Description { get; }
        public string Source { get; }

        public CommonCatalog(string sku, string description, string source)
        {
            SKU = sku;
            Description = description;
            Source = source;
        }
    }
}
