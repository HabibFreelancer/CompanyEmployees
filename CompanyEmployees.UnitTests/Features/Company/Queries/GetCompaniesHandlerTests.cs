using Application.Features.Company.Handlers.Queries;
using Application.Features.Company.Requests.Queries;
using AutoMapper;
using CompanyEmployees.AutoMapperProfiles;
using CompanyEmployees.UnitTests.Mocks;
using Contracts.Persistence;
using Moq;
using Shared.DataTransferObjects.Company;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.UnitTests.Features.Company.Queries
{
    /*     private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;*/
    public class GetCompaniesHandlerTests
    {
        private readonly Mock<IRepositoryManager> _mockRepoManager;
        private readonly IMapper _mapper;

        public GetCompaniesHandlerTests()
        {
            // mock Repositories
            _mockRepoManager = MockRepositoryManager.GetMockRepositoryManager();
            //configure mapper 
            var mapperConfig = new MapperConfiguration(m =>
                {
                    m.AddProfile<MappingProfile>();
                });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task GetCompaniesTest()
        {
            var handler = new GetCompaniesHandler(_mockRepoManager.Object, _mapper);

            var result = await handler.Handle(new GetCompaniesQuery(false), CancellationToken.None);

            result.ShouldBeOfType<List<CompanyDto>>();
            result.ToList().Count.ShouldBe(3);
        }

    }
}
