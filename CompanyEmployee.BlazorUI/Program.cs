using Blazored.LocalStorage;
using CompanyEmployee.BlazorUI;
using CompanyEmployee.BlazorUI.Common;
using CompanyEmployee.BlazorUI.Contracts;
using CompanyEmployee.BlazorUI.Providers;
using CompanyEmployee.BlazorUI.Services;
using CompanyEmployee.BlazorUI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection;
using System.Runtime.CompilerServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddTransient<AcceptViaHeadersHandler>();
builder.Services.AddTransient<JwtAuthorizationMessageHandler>();

// Microsoft.Extension.Http 
builder.Services.AddHttpClient<IClient, Client>(Client => Client.BaseAddress = new Uri("https://localhost:7050"))
    .AddHttpMessageHandler<AcceptViaHeadersHandler>()
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

await builder.Build().RunAsync();
