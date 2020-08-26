using Blazored.LocalStorage;
using BookStore_UI.Contract;
using BookStore_UI.Models;
using BookStore_UI.Provider;
using BookStore_UI.Static;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_UI.Service
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IHttpClientFactory _client;
        private readonly ILocalStorageService _localStorage;
        private readonly ApiAuthenticationStageProvider _apiAuthenticationStageProvider;

        public AuthenticationRepository(IHttpClientFactory client, 
            ILocalStorageService localStorage,
            ApiAuthenticationStageProvider apiAuthenticationStageProvider)
        {
            _client = client;
            _localStorage = localStorage;
            _apiAuthenticationStageProvider = apiAuthenticationStageProvider;

        }

        public async Task<bool> Login(LoginModel user)
        {
            //Create a request type of Post with Login Endpoint.
            var request = new HttpRequestMessage(HttpMethod.Post,
                Endpoints.LoginEndpoint);
            //Convert content to Json format.
            request.Content = new StringContent(JsonConvert.SerializeObject(user),
                Encoding.UTF8, "application/json");

            //Create http client.
            var client = _client.CreateClient();

            //send the request.
            HttpResponseMessage response = await client.SendAsync(request);

            //if login is not truning 201 code then return false.
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            //Read content from the request.
            var content = await response.Content.ReadAsStringAsync();

            //Getting back Token key.
            var token = JsonConvert.DeserializeObject<TokenResponse>(content);

            //Store Token at local blazor.
            await _localStorage.SetItemAsync("authToken", token.Token);

            //Change Authentication stage of application.
            await ((ApiAuthenticationStageProvider)_apiAuthenticationStageProvider).LoggedIn();

            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("bearer", token.Token);


            return true;
        }

        public async Task LogOut()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStageProvider)_apiAuthenticationStageProvider).LoggedOut();

        }

        public async Task<bool> Register(RegistrationModel user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, 
                Endpoints.RegisterEndpoint);
            request.Content = new StringContent(JsonConvert.SerializeObject(user), 
                Encoding.UTF8, "application/json");

            var client = _client.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;


        }
    }
}
