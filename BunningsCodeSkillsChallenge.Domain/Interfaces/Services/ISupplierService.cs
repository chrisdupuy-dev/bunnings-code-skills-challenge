namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using Models.Entities;

    public interface ISupplierService
    {
        Supplier GetSupplier(int ID);
        Supplier CreateSupplier(string Name);
    }
}
