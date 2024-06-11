using Blazored.LocalStorage;
using CompanyEmployee.BlazorUI.Contracts;
using CompanyEmployee.BlazorUI.Providers;
using CompanyEmployee.BlazorUI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace CompanyEmployee.BlazorUI.Services
{
    public class AuthenticationService : BaseHttpService, IAuthenticationService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public AuthenticationService(IClient client,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider) : base(client, localStorage)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            try
            {
                UserForAuthenticationDto user = new UserForAuthenticationDto { UserName = email, Password = password };
                var authenticationResponse = await _client.LoginAsync(user);
                if (authenticationResponse.Token.AccessToken != string.Empty)
                {
                    await _localStorage.SetItemAsync("token", authenticationResponse.Token.AccessToken);

                    // Set claims in Blazor and login state
                     await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task Logout()
        {
            // remove claims in Blazor and invalidate login state
            await((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
        }

        public async Task<bool> RegisterAsync(string firstName, string lastName, string userName, string email, string password)
        {
            UserForRegistrationDto registrationRequest = new UserForRegistrationDto() { FirstName = firstName, LastName = lastName, Email = email, UserName = userName, Password = password ,
                Roles = new string[] { "Manager" } };
            var response = await _client.RegisterAsync(registrationRequest);

            if (!string.IsNullOrEmpty(response.Email))
            {
                return true;
            }
            return false;
        }
    }
}
