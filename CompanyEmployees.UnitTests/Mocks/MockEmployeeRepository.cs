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
    public class MockEmployeeRepository
    {
        public static Mock<IEmployeeRepository> GetMockEmployeeRepository()
        {

            //TODO : setup Methods 

            // initialize a new instance 
            var mockRepo = new Mock<IEmployeeRepository>();
            return mockRepo;
        }
    }
}
