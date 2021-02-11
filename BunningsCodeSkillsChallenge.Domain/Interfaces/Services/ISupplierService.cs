namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using System.Collections.Generic;
    using Models;
    using Models.Entities;

    public interface ISupplierService
    {
        Supplier GetSupplier(Company company, int ID);
        Supplier CreateSupplier(Company company, string name);
        Supplier GetSupplierByName(Company company, string name);
        IEnumerable<Supplier> GetSuppliers(Company company);
    }
}
