namespace BunningsCodeSkillsChallenge.Domain.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Services;
    using Microsoft.Extensions.Logging;
    using Models;
    using Models.Entities;

    public class SupplierService : ISupplierService
    {
        private readonly ILogger _logger;
        public SupplierService(ILogger<SupplierService> logger)
        {
            _logger = logger;
        }

        public Supplier GetSupplier(Company company, int id)
        {
            return company.Suppliers.FirstOrDefault(_ => _.ID == id);
        }

        public Supplier GetSupplierByName(Company company, string name)
        {
            return company.Suppliers.FirstOrDefault(_ => _.Name == name);
        }

        public IEnumerable<Supplier> GetSuppliers(Company company)
        {
            return company.Suppliers;
        }

        public Supplier CreateSupplier(Company company, string name)
        {
            var newSupplier = new Supplier()
            {
                Name = name
            };

            company.InsertSupplier(newSupplier);

            return new Supplier();
        }
    }
}
