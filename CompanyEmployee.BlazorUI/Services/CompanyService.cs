using AutoMapper;
using Blazored.LocalStorage;
using CompanyEmployee.BlazorUI.Contracts;
using CompanyEmployee.BlazorUI.Models.Company;
using CompanyEmployee.BlazorUI.Services.Base;

namespace CompanyEmployee.BlazorUI.Services
{
    public class CompanyService : BaseHttpService, ICompanyService
    {
        private readonly IMapper _mapper;

        public CompanyService(IClient client, IMapper mapper, ILocalStorageService localStorage) : base(client, localStorage)
        {
            this._mapper = mapper;
        }

        public async Task<Response<Guid>> CreateCompay(CompanyVM company)
        {
            try
            {
                var createCompanyCommand = _mapper.Map<CompanyForCreationDto>(company);
                await _client.CreateCompanyAsync(createCompanyCommand);
                return new Response<Guid>()
                {
                    Success = true,
                };
            }
            catch (ApiException ex)
            {

                return ConvertApiExceptions<Guid>(ex);
            }
        }

        public async Task<Response<Guid>> DeleteCompay(System.Guid id)
        {
            try
            {
                await _client.CompaniesDELETEAsync(id);
                return new Response<Guid>() { Success = true };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<Guid>(ex);
            }
        }

        public async Task<List<CompanyVM>> GetCompanies()
        {
          
            var companies = await _client.GetCompaniesAsync();
            return _mapper.Map<List<CompanyVM>>(companies);
        }


        //   System.Threading.Tasks.Task CompaniesPUTAsync(System.Guid id, CompanyForUpdateDto body);
        public async  Task<Response<Guid>> UpdateCompay(System.Guid id, CompanyVM company)
        {
            try
            {
                var updateCompanyCommand = _mapper.Map<CompanyForUpdateDto>(company);
                await _client.CompaniesPUTAsync(id, updateCompanyCommand);
                return new Response<Guid>()
                {
                    Success = true,
                };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<Guid>(ex);
            }
        }
    }
}
