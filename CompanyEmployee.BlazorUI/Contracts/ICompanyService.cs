using CompanyEmployee.BlazorUI.Models.Company;
using CompanyEmployee.BlazorUI.Services.Base;

namespace CompanyEmployee.BlazorUI.Contracts
{
    public interface ICompanyService
    {
        Task<List<CompanyVM>> GetCompanies();

        Task<Response<Guid>> CreateCompay(CompanyVM leaveType);
        Task<Response<Guid>> UpdateCompay(System.Guid id, CompanyVM leaveType);
        Task<Response<Guid>> DeleteCompay(System.Guid id);
    }
}
