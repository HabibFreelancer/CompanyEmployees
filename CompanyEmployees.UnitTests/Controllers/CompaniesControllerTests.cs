using Application.Features.Company.Handlers.Queries;
using Application.Features.Company.Requests.Queries;
using AutoMapper;
using CompanyEmployees.AutoMapperProfiles;
using CompanyEmployees.Presentation.Controllers;
using CompanyEmployees.UnitTests.Mocks;
using Contracts.Persistence;
using Entities.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.DataTransferObjects.Company;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.UnitTests.Controllers
{

    public class CompaniesControllerTests
    {
        private readonly Mock<ISender> _mockSender;
        private readonly Mock<IPublisher> _mockPublisher;
        private readonly CompaniesController _controller;



        public CompaniesControllerTests()
        {
            _mockSender = MockCompanyMediatorSender.GetMockCompanyMediatorSender();
            _mockPublisher = MockCompanyMediatorPublisher.GetMockCompanyMediatorPublisher();
            _controller = new CompaniesController(_mockSender.Object, _mockPublisher.Object);
        }

        [Fact]
        public void GetCompanies_Handler_ReturnsOk()
        {
            //arrange 


            //Act
            var result = _controller.GetCompanies();
            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

    }
}
