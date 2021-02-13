namespace BunningsCodeSkillsChallenge.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Models;
    using Interfaces.Services;
    using Microsoft.Extensions.Logging;

    public class CompanyManager : ICompanyManager
    {
        private readonly ILogger _logger;

        private List<ICompany> _companies { get; }

        public CompanyManager(ILogger<CompanyManager> logger)
        {
            _logger = logger;
            _companies = new List<ICompany>();
        }

        public ICompany AddCompany(ICompany company)
        {
            if (_companies.Contains(company))
                throw new Exception("Company already exists");

            if (_companies.Any(_ => _.Name == company.Name))
                throw new Exception("Company name already in use");

            _companies.Add(company);

            return company;
        }

        public ICompany GetCompany(string name)
        {
            var company = _companies.FirstOrDefault(_ => _.Name == name);
            if (company == null)
                throw new Exception("Company not found");

            return company;
        }

        public IEnumerable<ICompany> GetAllCompanies()
        {
            return _companies;
        }
    }
}
