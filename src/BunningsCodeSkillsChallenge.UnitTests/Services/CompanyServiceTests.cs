namespace BunningsCodeSkillsChallenge.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using Domain.Models;
    using Domain.Models.Entities;
    using Domain.Services;
    using Xunit;

    public class CompanyServiceTests
    {
        [Fact]
        public void AddCompany_WhenCalledWithCompany_ShouldAddCompany()
        {
            // Arrange
            var companyName = "A";
            var expectedCompany = new Company(companyName, new List<Catalog>(), new List<SupplierProductBarcode>(), new List<Supplier>());

            var companyService = new CompanyService(null);

            // Act
            companyService.AddCompany(expectedCompany);

            // Assert
            var actualCompany = companyService.GetCompany(companyName);
            Assert.Equal(expectedCompany, actualCompany);
        }

        [Fact]
        public void GetCompany_WhenCalledWithInvalidName_ShouldThrowException()
        {
            // Arrange
            var companyService = new CompanyService(null);

            // Act & Assert
            Assert.Throws<Exception>(() => companyService.GetCompany("This company doesn't exist"));
        }

        [Fact]
        public void GetAllCompanies_WhenCalled_ShouldReturnAllCompanies()
        {
            // Arrange
            var expectedCompanyA = new Company("A", new List<Catalog>(), new List<SupplierProductBarcode>(), new List<Supplier>());
            var expectedCompanyB = new Company("B", new List<Catalog>(), new List<SupplierProductBarcode>(), new List<Supplier>());

            var companyService = new CompanyService(null);
            companyService.AddCompany(expectedCompanyA);
            companyService.AddCompany(expectedCompanyB);

            // Act
            var actualAllCompanies = companyService.GetAllCompanies();

            // Assert
            Assert.Contains(expectedCompanyA, actualAllCompanies);
            Assert.Contains(expectedCompanyB, actualAllCompanies);
        }
    }
}
