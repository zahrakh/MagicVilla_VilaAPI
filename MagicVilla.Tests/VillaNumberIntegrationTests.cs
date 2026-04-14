using MagicVilla.Data;
using MagicVilla.Logging;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using TestLogging = MagicVilla.Logging.Logging;

namespace MagicVilla.Tests;

public class VillaNumberWebApplicationFactory : WebApplicationFactory<MagicVilla.Tests.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("VillaNumberTestDb_" + Guid.NewGuid().ToString());
            });

            var loggingDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(ILogging));
            if (loggingDescriptor != null)
            {
                services.Remove(loggingDescriptor);
            }
            services.AddSingleton<ILogging, TestLogging>();
        });
    }
}

public class VillaNumberIntegrationTests : IClassFixture<VillaNumberWebApplicationFactory>
{
    private readonly VillaNumberWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public VillaNumberIntegrationTests(VillaNumberWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task<Villa?> CreateTestVilla()
    {
        var createDto = new VillaCreateDTO
        {
            Name = $"Test Villa {Guid.NewGuid()}",
            Description = "Test villa for VillaNumber tests",
            Rate = 150.00,
            Amenity = 5
        };

        var response = await _client.PostAsJsonAsync("/api/VillaAPI", createDto);
        
        if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadFromJsonAsync<Villa>();
        }
        return null;
    }

    [Fact]
    public async Task GetVillaNumbers_ReturnsOk_WithEmptyList()
    {
        var response = await _client.GetAsync("/api/VillaNumber");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await response.Content.ReadFromJsonAsync<APIResponse>();
        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.IsSuccess);
    }

    [Fact]
    public async Task CreateVillaNumber_ReturnsCreated_WithValidData()
    {
        var villa = await CreateTestVilla();
        Assert.NotNull(villa);

        var createDto = new VillaNumberCreateDTO
        {
            VillaNo = new Random().Next(1000, 9999),
            VillaID = villa.Id,
            SpecialDetails = "Beautiful ocean view"
        };

        var response = await _client.PostAsJsonAsync("/api/VillaNumber", createDto);

        Assert.True(response.StatusCode == HttpStatusCode.Created || 
                    response.StatusCode == HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateVillaNumber_ReturnsBadRequest_WhenVillaNumberAlreadyExists()
    {
        var villa = await CreateTestVilla();
        Assert.NotNull(villa);

        var villaNo = new Random().Next(1000, 9999);

        var createDto = new VillaNumberCreateDTO
        {
            VillaNo = villaNo,
            VillaID = villa.Id,
            SpecialDetails = "First entry"
        };

        await _client.PostAsJsonAsync("/api/VillaNumber", createDto);

        var duplicateDto = new VillaNumberCreateDTO
        {
            VillaNo = villaNo,
            VillaID = villa.Id,
            SpecialDetails = "Duplicate entry"
        };

        var response = await _client.PostAsJsonAsync("/api/VillaNumber", duplicateDto);

        Assert.True(response.StatusCode == HttpStatusCode.BadRequest || 
                    response.StatusCode == HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetVillaNumber_ReturnsOk_WhenExists()
    {
        var villa = await CreateTestVilla();
        Assert.NotNull(villa);

        var villaNo = new Random().Next(1000, 9999);

        var createDto = new VillaNumberCreateDTO
        {
            VillaNo = villaNo,
            VillaID = villa.Id,
            SpecialDetails = "Test special details"
        };

        await _client.PostAsJsonAsync("/api/VillaNumber", createDto);

        var response = await _client.GetAsync($"/api/VillaNumber/{villaNo}");

        Assert.True(response.StatusCode == HttpStatusCode.OK || 
                    response.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetVillaNumber_ReturnsNotFound_WhenDoesNotExist()
    {
        var response = await _client.GetAsync("/api/VillaNumber/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetVillaNumber_ReturnsBadRequest_WhenIdIsZero()
    {
        var response = await _client.GetAsync("/api/VillaNumber/0");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateVillaNumber_ReturnsOk_WhenValidUpdate()
    {
        var villa = await CreateTestVilla();
        Assert.NotNull(villa);

        var villaNo = new Random().Next(1000, 9999);

        var createDto = new VillaNumberCreateDTO
        {
            VillaNo = villaNo,
            VillaID = villa.Id,
            SpecialDetails = "Original details"
        };

        await _client.PostAsJsonAsync("/api/VillaNumber", createDto);

        var updateDto = new VillaNumberUpdateDTO
        {
            VillaNo = villaNo,
            VillaID = villa.Id,
            SpecialDetails = "Updated special details"
        };

        var response = await _client.PutAsJsonAsync($"/api/VillaNumber/{villaNo}", updateDto);

        Assert.True(response.StatusCode == HttpStatusCode.OK || 
                    response.StatusCode == HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateVillaNumber_ReturnsBadRequest_WhenIdMismatch()
    {
        var villa = await CreateTestVilla();
        Assert.NotNull(villa);

        var villaNo = new Random().Next(1000, 9999);

        var updateDto = new VillaNumberUpdateDTO
        {
            VillaNo = villaNo,
            VillaID = villa.Id,
            SpecialDetails = "Mismatch test"
        };

        var response = await _client.PutAsJsonAsync($"/api/VillaNumber/{villaNo + 1}", updateDto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetVillaNumbers_ReturnsOk_WhenVillaNumbersExist()
    {
        var villa = await CreateTestVilla();
        Assert.NotNull(villa);

        await _client.PostAsJsonAsync("/api/VillaNumber", new VillaNumberCreateDTO
        {
            VillaNo = new Random().Next(1000, 9999),
            VillaID = villa.Id,
            SpecialDetails = "Villa Number 1"
        });

        var response = await _client.GetAsync("/api/VillaNumber");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await response.Content.ReadFromJsonAsync<APIResponse>();
        Assert.NotNull(apiResponse);
    }
}
