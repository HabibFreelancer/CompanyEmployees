using CompanyEmployee.BlazorUI.Contracts;
using CompanyEmployee.BlazorUI.Services.Base;

namespace CompanyEmployee.BlazorUI.Services
{
    public class CompanyService : BaseHttpService, ICompanyService
    {
        public CompanyService(IClient client) : base(client)
        {
        }
    }
}
