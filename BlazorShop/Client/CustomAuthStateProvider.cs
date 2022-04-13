using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorShop.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _http;

        //will get the auth-token from the local storage
        //Then pass the claims and create a new claims identity
        //Then notify the components that want to be notified of these new claims identety
        //With that the application will know if the current user is autehnticated or not
        public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient http)
        {
            _localStorageService = localStorageService;
            _http = http;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string authToken = await _localStorageService.GetItemAsStringAsync("authToken");

            var identity = new ClaimsIdentity();
            //set a default header, inject hpptClient
            _http.DefaultRequestHeaders.Authorization = null; //User is unautherized in this stage

            if (!string.IsNullOrEmpty(authToken))
            {
                try
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
                    _http.DefaultRequestHeaders.Authorization = //sets the auth-header
                        new AuthenticationHeaderValue("Bearer", authToken.Replace("\"","")); //adds the bearerToken, remove the ""-marks, otherwise the token wont work(same as Solidity)
                }
                catch (Exception)
                {
                    await _localStorageService.RemoveItemAsync("authToken");
                    identity = new ClaimsIdentity();
                }
            }
            //sets the user
            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        private byte[] ParseBase64WithoutPadding(string base64) 
        {
            switch (base64.Length % 4) 
            {
                case 2: base64 += "==";
                    break;
                case 3: base64 += "=";
                    break;
            }
            return Convert.FromBase64String(base64);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt) 
        {
            var payload = jwt.Split('.')[1]; //gets the second part of the array
            var jsonBytes = ParseBase64WithoutPadding(payload);
            //create key-value pairs
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));

            return claims;
        }
    }
}
