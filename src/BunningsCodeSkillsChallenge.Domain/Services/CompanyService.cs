namespace BunningsCodeSkillsChallenge.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Services;
    using Microsoft.Extensions.Logging;
    using Models;

    public class CompanyService : ICompanyService
    {
        private readonly ILogger _logger;

        private List<Company> _companies { get; }

        public CompanyService(ILogger<CompanyService> logger)
        {
            _logger = logger;
            _companies = new List<Company>();
        }

        public Company AddCompany(Company company)
        {
            if (_companies.Contains(company))
                throw new Exception("Company already exists");

            if (_companies.Any(_ => _.Name == company.Name))
                throw new Exception("Company name already in use");

            _companies.Add(company);

            return company;
        }

        public Company GetCompany(string name)
        {
            var company = _companies.FirstOrDefault(_ => _.Name == name);
            if (company == null)
                throw new Exception("Company not found");

            return company;
        }

        public IEnumerable<Company> GetAllCompanies()
        {
            return _companies;
        }
    }
}
