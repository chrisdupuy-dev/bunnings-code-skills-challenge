namespace BunningsCodeSkillsChallenge.UnitTests
{
    using System;
    using Domain;
    using Domain.Interfaces.Models;
    using Moq;
    using Xunit;

    public class CompanyManagerTests
    {
        [Fact]
        public void InsertCompany_WhenCalledWithCompany_ShouldAddCompany()
        {
            // Arrange
            var companyName = "A";
            
            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.Name).Returns(companyName);

            var companyManager = new CompanyManager(null);

            // Act
            companyManager.AddCompany(mockCompany.Object);

            // Assert
            var actualCompany = companyManager.GetCompany(companyName);
            Assert.Equal(mockCompany.Object, actualCompany);
            
            mockCompany.Verify();
        }

        [Fact]
        public void InsertCompany_WhenCalledWithExistingCompany_ShouldThrowException()
        {
            // Arrange
            var companyName = "A";

            var mockCompany = new Mock<ICompany>();
            mockCompany.Setup(_ => _.Name).Returns(companyName);

            var companyManager = new CompanyManager(null);
            companyManager.AddCompany(mockCompany.Object);

            // Act & Assert
            Assert.Throws<Exception>(() => companyManager.AddCompany(mockCompany.Object));
            mockCompany.Verify();
        }

        [Fact]
        public void InsertCompany_WhenCalledWithExistingCompanyName_ShouldThrowException()
        {
            // Arrange
            var companyName = "A";

            var mockCompanyA = new Mock<ICompany>();
            mockCompanyA.Setup(_ => _.Name).Returns(companyName);

            var mockCompanyB = new Mock<ICompany>();
            mockCompanyB.Setup(_ => _.Name).Returns(companyName);

            var companyManager = new CompanyManager(null);
            companyManager.AddCompany(mockCompanyA.Object);

            // Act & Assert
            Assert.Throws<Exception>(() => companyManager.AddCompany(mockCompanyB.Object));

            mockCompanyA.Verify();
            mockCompanyB.Verify();
        }

        [Fact]
        public void GetCompany_WhenCalledWithInvalidName_ShouldThrowException()
        {
            // Arrange
            var companyService = new CompanyManager(null);

            // Act & Assert
            Assert.Throws<Exception>(() => companyService.GetCompany("This company doesn't exist"));
        }

        [Fact]
        public void GetAllCompanies_WhenCalled_ShouldReturnAllCompanies()
        {
            // Arrange
            var mockCompanyA = new Mock<ICompany>();
            mockCompanyA.Setup(_ => _.Name).Returns("A");

            var mockCompanyB = new Mock<ICompany>();
            mockCompanyB.Setup(_ => _.Name).Returns("B");

            var companyManager = new CompanyManager(null);
            companyManager.AddCompany(mockCompanyA.Object);
            companyManager.AddCompany(mockCompanyB.Object);

            // Act
            var actualAllCompanies = companyManager.GetAllCompanies();

            // Assert
            Assert.Contains(mockCompanyA.Object, actualAllCompanies);
            Assert.Contains(mockCompanyB.Object, actualAllCompanies);
        }
    }
}
