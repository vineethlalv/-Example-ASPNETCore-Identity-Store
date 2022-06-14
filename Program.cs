using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using example.AspnetCoreIdentity.StoragePlugin.IdentityStore.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// @NOTE: Add Identity Types
//        (needs to explicitly add services that were already injected with EF Core version of Identity)
builder.Services.AddIdentityCore<IdentityUser>()
                .AddSignInManager();              // @NOTE: AddSignInManager() Require AddAuthentication() and an authentication scheme

// @NOTE: Plug-in types for ASP.NET Identity
builder.Services.AddSingleton<ILookupNormalizer, LookUpNormalizer>();
builder.Services.AddSingleton<IUserStore<IdentityUser>, CustomUserStore>();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.ApplicationScheme);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();