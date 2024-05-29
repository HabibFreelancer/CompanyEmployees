using CompanyEmployees.Presentation.Controllers;
using CompanyEmployees.UnitTests.Mocks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.DataTransferObjects.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.UnitTests.Controllers
{
    public class EmployeesControllerTests
    {
        private readonly Mock<ISender> _mockSender;
        private readonly Mock<IPublisher> _mockPublisher;
        private readonly EmployeesController _controller;
        public EmployeesControllerTests()
        {

            _mockSender = MockCompanyMediatorSender.GetMockCompanyMediatorSender();
            _mockPublisher = MockCompanyMediatorPublisher.GetMockCompanyMediatorPublisher();
            _controller = new EmployeesController(_mockSender.Object, _mockPublisher.Object);
        }

        [Fact]
        public void GetEmployeeForCompanyAsync_ReturnsAnEmployee()
        {
            /*TODO : to complete*/
            //arrange 
            Guid comapnyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870");
            Guid employeeId = new Guid("80abbca8-664d-4b20-b5de-024705497d4a");

            //Act
            var result = _controller.GetEmployeeForCompanyAsync(comapnyId, employeeId);

            //Assert
            Assert.IsType<EmployeeDto>(result);

            //Jon Van

        }

    }
}
