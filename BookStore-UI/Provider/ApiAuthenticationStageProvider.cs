using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStore_UI.Provider
{
    public class ApiAuthenticationStageProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        public ApiAuthenticationStageProvider(ILocalStorageService localStorage,
            JwtSecurityTokenHandler tokenHandler)
        {
            _localStorage = localStorage;
            _tokenHandler = tokenHandler;
        }

        //check if person is authorized or not base our authority we set on our application.
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var saveToken = await _localStorage.GetItemAsync<string>("authToken");
                if (string.IsNullOrWhiteSpace(saveToken))
                {
                    //Say Claims is empty == nobody home.
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

                }
                var tokenContent = _tokenHandler.ReadJwtToken(saveToken);
                var expiry = tokenContent.ValidTo;
                if(expiry < DateTime.Now)
                {
                    //Remove Token
                    await _localStorage.RemoveItemAsync("authToken");
                    //Say Claims is empty == nobody home. No body is authenticated.
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                //Get Claims from token and Build authenticated user object.
                var claims = parseClaims(tokenContent);
                var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

                //Return authenticated person
                return new AuthenticationState(user);


            }
            catch (Exception)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task LoggedIn()
        {
            var saveToken = await _localStorage.GetItemAsync<string>("authToken");
            var tokenContent = _tokenHandler.ReadJwtToken(saveToken);
            var claims = parseClaims(tokenContent);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);

        }

        public void LoggedOut()
        {
            var nobody = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(nobody));
            NotifyAuthenticationStateChanged(authState);
        }

        private IList<Claim> parseClaims(JwtSecurityToken tokenContent)
        {
            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
            return claims;
        }
    }
}
