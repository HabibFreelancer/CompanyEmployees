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
    public class MockRepositoryManager
    {
        public static Mock<IRepositoryManager> GetMockRepositoryManager()
        {

            // initialize instances 
            var mockEmployeeRepository = MockEmployeeRepository.GetMockEmployeeRepository();
            var mockCompanyRepository = MockCompanyRepository.GetCompaniesMockCompanyRepository();
            var mockManagerRepo = new Mock<IRepositoryManager>();

            //setup objects and methods 
            mockManagerRepo.Setup(m => m.Company).Returns(mockCompanyRepository.Object);
            mockManagerRepo.Setup(m => m.Employee).Returns(mockEmployeeRepository.Object);

            return mockManagerRepo;


        }
    }
}
