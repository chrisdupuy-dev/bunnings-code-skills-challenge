namespace BunningsCodeSkillsChallenge.Domain.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Services;
    using Models;
    using Models.Entities;

    public class SupplierService : ISupplierService
    {
        private readonly Company _company;

        public SupplierService(Company company)
        {
            _company = company;
        }

        public Supplier GetSupplier(int id)
        {
            return _company.Suppliers.FirstOrDefault(_ => _.ID == id);
        }

        public Supplier GetSupplierByName(string name)
        {
            return _company.Suppliers.FirstOrDefault(_ => _.Name == name);
        }

        public IEnumerable<Supplier> GetSuppliers()
        {
            return _company.Suppliers;
        }

        public Supplier CreateSupplier(string name)
        {
            var newSupplier = new Supplier()
            {
                Name = name
            };

            _company.InsertSupplier(newSupplier);

            return new Supplier();
        }
    }
}
