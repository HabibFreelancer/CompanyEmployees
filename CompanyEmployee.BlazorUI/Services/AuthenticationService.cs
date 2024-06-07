using Blazored.LocalStorage;
using CompanyEmployee.BlazorUI.Contracts;
using CompanyEmployee.BlazorUI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace CompanyEmployee.BlazorUI.Services
{
    public class AuthenticationService : BaseHttpService, IAuthenticationService
    {
       // private readonly AuthenticationStateProvider _authenticationStateProvider;
        public AuthenticationService(IClient client,
            ILocalStorageService localStorage/*,
            AuthenticationStateProvider authenticationStateProvider*/) : base(client, localStorage)
        {
            //_authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            return false; 
            //try
            //{
            //  //  AuthRequest authenticationRequest = new AuthRequest() { Email = email, Password = password };
            //   // var authenticationResponse = await _client.LoginAsync(authenticationRequest);
            //    UserForAuthenticationDto user = new UserForAuthenticationDto { UserName =email, Password =password};
            //    var authenticationResponse = await _client.LoginAsync(user) ;
            //    if (authenticationResponse.Token != string.Empty)
            //    {
            //        await _localStorage.SetItemAsync("token", authenticationResponse.Token);

            //        // Set claims in Blazor and login state
            //        await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();
            //        return true;
            //    }
            //    return false;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}

        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterAsync(string firstName, string lastName, string userName, string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
