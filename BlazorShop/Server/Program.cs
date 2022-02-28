global using BlazorShop.Shared;
global using Microsoft.EntityFrameworkCore;
global using BlazorShop.Server.Data;
global using BlazorShop.Server.Services.ProductServices;
global using BlazorShop.Server.Services.CategoryServices;
global using BlazorShop.Server.Services.CartService;
global using BlazorShop.Shared.Dto;
using Microsoft.AspNetCore.ResponseCompression;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//add connection to database, configure to sequelServer
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
}); 

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();//added
builder.Services.AddSwaggerGen(); //add swagger
builder.Services.AddScoped<IProductService, ProductService>(); //for dependencyinjections and use of product && Iproduct
builder.Services.AddScoped<ICategoryService, CategoryService>(); //As above, add global using att top
builder.Services.AddScoped<ICartService, CartService>(); //..

var app = builder.Build();

app.UseSwaggerUI(); //add swagger

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger(); //add swagger
app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
