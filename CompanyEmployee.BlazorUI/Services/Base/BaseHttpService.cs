using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace CompanyEmployee.BlazorUI.Services.Base
{
    public class BaseHttpService
    {
        protected IClient _client;
        protected readonly ILocalStorageService _localStorage;
        public BaseHttpService(IClient client, ILocalStorageService localStorage)
        {
            _client = client;
            _localStorage = localStorage;
        }

        /*Convert the ApiException that generate from ServiceClient to our custorm return response Response.cs */
        protected Response<Guid> ConvertApiExceptions<Guid>(ApiException ex)
        {
            if (ex.StatusCode == 400 || ex.StatusCode == 422)
            {
                return new Response<Guid>() { Message = "Invalid data was submitted", ValidationErrors = ex.Response, Success = false };
            }
            else if (ex.StatusCode == 404)
            {
                return new Response<Guid>() { Message = "The record was not found.", Success = false };
            }
            else
            {
                return new Response<Guid>() { Message = "Something went wrong, please try again later.", Success = false };
            }
        }

        protected async Task AddBearerToken()
        {
            string savedToken = await _localStorage.GetItemAsync<string>("token");
            if (!string.IsNullOrEmpty(savedToken))
                _client.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", savedToken);
        }
    }
}
