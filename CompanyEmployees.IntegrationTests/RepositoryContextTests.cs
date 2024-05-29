using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository;
using Shouldly;

namespace CompanyEmployees.IntegrationTests
{
    public class RepositoryContextTests
    {
        private readonly DbContext _RepositoryContext;

        public RepositoryContextTests()
        {
            /*Configure DbContext In Memory*/
            var dbOptions = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _RepositoryContext = new RepositoryContext(dbOptions);
        }
        [Fact]
        public async void Save_Company()
        {
            //Arrange
            var company = new Company
            {
                Id = Guid.NewGuid(),
                Name = "Name x",
                Address = "address x",
                Country = "Country x",
                Employees = new List<Employee>()
            };

            //Act
            _RepositoryContext.Set<Company>().Add(company);
            await _RepositoryContext.SaveChangesAsync();

            //Assert

            company.Name.ShouldNotBeNull();

        }
    }
}