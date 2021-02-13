namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Models.Entities;
    using Models;

    public interface ISupplierService
    {
        Supplier InsertSupplier(ICompany company, string name);
        IEnumerable<Supplier> GetSuppliers(ICompany company);
    }
}
