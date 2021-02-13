namespace BunningsCodeSkillsChallenge.Domain.Services
{
    using System.Collections.Generic;
    using Interfaces.Models;
    using Interfaces.Services;
    using Microsoft.Extensions.Logging;
    using Models.Entities;

    public class SupplierService : ISupplierService
    {
        private readonly ILogger _logger;
        public SupplierService(ILogger<SupplierService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<Supplier> GetSuppliers(ICompany company)
        {
            return company.Suppliers;
        }

        public Supplier InsertSupplier(ICompany company, string name)
        {
            var newSupplier = new Supplier()
            {
                Name = name
            };

            return company.InsertSupplier(newSupplier);
        }
    }
}
