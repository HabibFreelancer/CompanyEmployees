using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using CompanyEmployee.BlazorUI;
using CompanyEmployee.BlazorUI.Shared;
using CompanyEmployee.BlazorUI.Contracts;
using CompanyEmployee.BlazorUI.Models.Company;

namespace CompanyEmployee.BlazorUI.Pages.Companies
{
    public partial class Index
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public ICompanyService CompanyService { get; set; }

        public List<CompanyVM> Companies { get; private set; }
        public string Message { get; set; } = string.Empty;

        protected void CreateCompany()
        {
            NavigationManager.NavigateTo("/companies/create/"); // todo : view to create 
        }

        protected void EditCompany(Guid id)
        {
            NavigationManager.NavigateTo($"/companies/edit/{id}"); // todo : view to create 
        }

        protected async Task DeleteCompany(Guid id)
        {
            var response = await CompanyService.DeleteCompay(id);
            if (response.Success)
            {

                await OnInitializedAsync();
            }
            else
            {
                Message = response.Message;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            Companies = await CompanyService.GetCompanies();
        }


    }
}