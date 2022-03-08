global using BlazorShop.Shared;
global using System.Net.Http.Json;
global using BlazorShop.Client.Services.ProductService;
global using BlazorShop.Client.Services.CategorySerrvice;
global using BlazorShop.Shared.Dto;

using BlazorShop.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using BlazorShop.Client.Services.CartService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage(); //Adds nuget, also in _import.razor
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IProductService, ProductService>(); //for dependencyinjections and use of product && Iproduct

builder.Services.AddScoped<ICategoryService, CategoryService>(); //as above
builder.Services.AddScoped<ICartService, CartService>(); //As above


await builder.Build().RunAsync();
