using Contracts.Persistence;
using Entities.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CompanyEmployees.UnitTests.Mocks
{
    public class MockCompanyRepository
    {
        public static Mock<ICompanyRepository> GetCompaniesMockCompanyRepository()
        {
            var companies = new List<Company>
            {

                new Company
                {
                    Id =Guid.NewGuid() ,
                    Name ="Name 1",
                    Address = "address 1",
                    Country = "Country 1",
                    Employees=  new List<Employee>()
                },
                new Company
                {
                    Id = Guid.NewGuid() ,
                    Name ="Name 2",
                    Address = "address 2",
                    Country = "Country 2",
                    Employees=  new List<Employee>()
                },
                new Company
                {
                    Id = Guid.NewGuid() ,
                    Name ="Name 3",
                    Address = "address 3",
                    Country = "Country 3",
                    Employees=  new List<Employee>()
                }
            };

            // initialize a new instance 
            var mockRepo = new Mock<ICompanyRepository>();

            //setup methods
            mockRepo.Setup(s => s.GetAllCompaniesAsync(false)).ReturnsAsync(companies);

            mockRepo.Setup(s => s.CreateCompany(It.IsAny<Company>()))
                .Callback<Company>(arg => companies.Add(arg)); //<-- use call back to perform function

            return mockRepo;

        }
    }
}
