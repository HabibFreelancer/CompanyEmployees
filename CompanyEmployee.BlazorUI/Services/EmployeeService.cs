using CompanyEmployee.BlazorUI.Contracts;
using CompanyEmployee.BlazorUI.Services.Base;

namespace CompanyEmployee.BlazorUI.Services
{
    public class EmployeeService : BaseHttpService, IEmployeeService
    {
        public EmployeeService(IClient client) : base(client)
        {
        }
    }
}
