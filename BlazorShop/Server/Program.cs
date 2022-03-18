global using BlazorShop.Shared;
global using Microsoft.EntityFrameworkCore;
global using BlazorShop.Server.Data;
global using BlazorShop.Server.Services.ProductServices;
global using BlazorShop.Server.Services.CategoryServices;
global using BlazorShop.Server.Services.CartService;
global using BlazorShop.Server.Services.AuthService;
global using BlazorShop.Shared.Dto;
global using BlazorShop.Server.Services.OrderService;
global using BlazorShop.Server.Services.PaymentService;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
builder.Services.AddScoped<IAuthService, AuthService>(); //..
builder.Services.AddScoped<IOrderService, OrderService>(); //..
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//ctrl+. install the package
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = 
                new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddHttpContextAccessor(); //to be able to access the user in the services

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
//order is important from line 32
app.UseAuthentication(); //adds authen middleware
app.UseAuthorization(); //adds author middleware


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
