global using BlazorShop.Shared;
global using System.Net.Http.Json;
global using BlazorShop.Client.Services.ProductService;
global using BlazorShop.Client.Services.CategorySerrvice;
global using BlazorShop.Client.Services.AuthService;
global using BlazorShop.Shared.Dto;
global using Microsoft.AspNetCore.Components.Authorization;
global using BlazorShop.Client.Services.CartService;
global using BlazorShop.Client.Services.OrderService;
global using BlazorShop.Client.Services.AddressService;
global using BlazorShop.Client.Services.ProductTypeService;
using BlazorShop.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage(); //Adds nuget, also in _import.razor
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IProductService, ProductService>(); //for dependencyinjections and use of product && Iproduct
builder.Services.AddScoped<ICategoryService, CategoryService>(); //as above
builder.Services.AddScoped<ICartService, CartService>(); //As above
builder.Services.AddScoped<IAuthService, AuthService>(); //As above
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddOptions(); //for global using Microsoft.AspNetCore.Components.Authorization;
builder.Services.AddAuthorizationCore(); //for global using Microsoft.AspNetCore.Components.Authorization;
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>(); //for global using Microsoft.AspNetCore.Components.Authorization;
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();

await builder.Build().RunAsync();
