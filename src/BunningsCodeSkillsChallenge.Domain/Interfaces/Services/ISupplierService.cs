namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using System.Collections.Generic;
    using Models;
    using Models.Entities;

    public interface ISupplierService
    {
        Supplier CreateSupplier(Company company, string name);
        IEnumerable<Supplier> GetSuppliers(Company company);
    }
}
