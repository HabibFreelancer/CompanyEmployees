using Application.Features.Company.Handlers.Queries;
using Application.Features.Company.Requests.Queries;
using AutoMapper;
using CompanyEmployees.AutoMapperProfiles;
using Contracts.Persistence;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.UnitTests.Mocks
{
    public class MockCompanyMediatorSender
    {
        public static Mock<ISender> GetMockCompanyMediatorSender()
        {
            // get instance mock manager repository
            var mockRepoManager = MockRepositoryManager.GetMockRepositoryManager();
            //configure mapper 
            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile<MappingProfile>();
            });
            var mapper = mapperConfig.CreateMapper();

            var getCompaniesHandler = new GetCompaniesHandler(mockRepoManager.Object, mapper);

            /*  var handler = new GetCompaniesHandler(_mockRepoManager.Object, _mapper);
            */
            var returnedGetCompaniesHandler = getCompaniesHandler.Handle(new GetCompaniesQuery(false), CancellationToken.None).Result;

            // initialize a new instance 
            var mockSender = new Mock<ISender>();
            mockSender.Setup(s => s.Send(getCompaniesHandler, CancellationToken.None)).ReturnsAsync(
               returnedGetCompaniesHandler);


            return mockSender;


        }
    }
}
