global using BlazorShop.Shared;
global using System.Net.Http.Json;
global using BlazorShop.Client.Services.ProductService;

global using BlazorShop.Client.Services.CategorySerrvice;

using BlazorShop.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IProductService, ProductService>(); //for dependencyinjections and use of product && Iproduct

builder.Services.AddScoped<ICategoryService, CategoryService>(); //as above


await builder.Build().RunAsync();
