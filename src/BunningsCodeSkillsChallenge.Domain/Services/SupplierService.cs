namespace BunningsCodeSkillsChallenge.Domain.Services
{
    using System.Collections.Generic;
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

            return company.InsertSupplier(newSupplier);
        }
    }
}
