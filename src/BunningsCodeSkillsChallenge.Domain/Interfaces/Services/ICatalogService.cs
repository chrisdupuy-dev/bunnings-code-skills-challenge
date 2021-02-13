namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using Domain.Models.Entities;
    using Models;

    public interface ICatalogService
    {
        Catalog GetCatalog(ICompany company, string sku);
        Catalog InsertCatalog(ICompany company, string sku, string description);
        void DeleteCatalog(ICompany company, string sku);
    }
}
