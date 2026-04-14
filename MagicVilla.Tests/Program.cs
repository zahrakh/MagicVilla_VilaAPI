using MagicVilla.Data;
using MagicVilla.Logging;
using MagicVilla.repository;
using MagicVilla.repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MagicVilla.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCaching();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();

builder.Services.AddAutoMapper(typeof(MagicVilla.MappingConfig));

var key = builder.Configuration["ApiSettings:Secret"] ?? "TestSecretKey12345678901234567890";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(VillaApiController).Assembly);

builder.Services.AddSingleton<ILogging, Logging>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

app.MapControllers();
app.Run();

namespace MagicVilla.Tests
{
    public partial class Program { }
}
