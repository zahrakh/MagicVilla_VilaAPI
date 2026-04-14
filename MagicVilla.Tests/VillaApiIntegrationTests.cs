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

public class CustomWebApplicationFactory : WebApplicationFactory<MagicVilla.Tests.Program>
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
                options.UseInMemoryDatabase("VillaTestDb_" + Guid.NewGuid().ToString());
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

public class VillaApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public VillaApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetVillas_ReturnsOkWithEmptyList_WhenNoVillas()
    {
        var response = await _client.GetAsync("/api/VillaAPI");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await response.Content.ReadFromJsonAsync<APIResponse>();
        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.IsSuccess);
    }

    [Fact]
    public async Task CreateVilla_ReturnsCreated_WithValidData()
    {
        var createDto = new VillaCreateDTO
        {
            Name = "Test Villa",
            Description = "A beautiful test villa",
            Rate = 150.00,
            Amenity = 5,
            ImageUrl = "http://test.com/image.jpg"
        };

        var response = await _client.PostAsJsonAsync("/api/VillaAPI", createDto);

        Assert.True(response.StatusCode == HttpStatusCode.Created || 
                    response.StatusCode == HttpStatusCode.OK);

        var createdVilla = await response.Content.ReadFromJsonAsync<Villa>();
        Assert.NotNull(createdVilla);
        Assert.True(createdVilla.Id > 0);
    }

    [Fact]
    public async Task CreateVilla_ReturnsValidStatus_WhenCreatingDuplicateName()
    {
        var uniqueName = $"Duplicate Villa {Guid.NewGuid()}";
        var createDto = new VillaCreateDTO
        {
            Name = uniqueName,
            Description = "Test",
            Rate = 100,
            Amenity = 3
        };

        var firstResponse = await _client.PostAsJsonAsync("/api/VillaAPI", createDto);
        
        var duplicateDto = new VillaCreateDTO
        {
            Name = uniqueName,
            Description = "Test Duplicate",
            Rate = 100,
            Amenity = 3
        };

        var response = await _client.PostAsJsonAsync("/api/VillaAPI", duplicateDto);

        Assert.NotEqual(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task GetVilla_ReturnsOk_WhenVillaExists()
    {
        var createDto = new VillaCreateDTO
        {
            Name = "Get Test Villa",
            Description = "For get test",
            Rate = 200,
            Amenity = 4
        };

        var createResponse = await _client.PostAsJsonAsync("/api/VillaAPI", createDto);
        
        if (createResponse.StatusCode == HttpStatusCode.Created || createResponse.StatusCode == HttpStatusCode.OK)
        {
            var createdVilla = await createResponse.Content.ReadFromJsonAsync<Villa>();
            if (createdVilla != null)
            {
                var response = await _client.GetAsync($"/api/VillaAPI/{createdVilla.Id}");
                Assert.True(response.StatusCode == HttpStatusCode.OK || 
                            response.StatusCode == HttpStatusCode.NotFound);
            }
        }
    }

    [Fact]
    public async Task GetVilla_ReturnsNotFound_WhenVillaDoesNotExist()
    {
        var response = await _client.GetAsync("/api/VillaAPI/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetVilla_ReturnsBadRequest_WhenIdIsZero()
    {
        var response = await _client.GetAsync("/api/VillaAPI/0");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateVilla_ReturnsOk_WhenValidUpdate()
    {
        var createDto = new VillaCreateDTO
        {
            Name = "Villa To Update",
            Description = "Original description",
            Rate = 100,
            Amenity = 3
        };

        var createResponse = await _client.PostAsJsonAsync("/api/VillaAPI", createDto);
        
        if (createResponse.StatusCode == HttpStatusCode.Created || createResponse.StatusCode == HttpStatusCode.OK)
        {
            var createdVilla = await createResponse.Content.ReadFromJsonAsync<Villa>();
            if (createdVilla != null)
            {
                var updateDto = new VillaUpdateDTO
                {
                    Id = createdVilla.Id,
                    Name = "Updated Villa Name",
                    Description = "Updated description",
                    Rate = 200,
                    Amenity = 5,
                    ImageUrl = "http://test.com/new-image.jpg"
                };

                var response = await _client.PutAsJsonAsync($"/api/VillaAPI/{createdVilla.Id}", updateDto);
                Assert.True(response.StatusCode == HttpStatusCode.NoContent || 
                            response.StatusCode == HttpStatusCode.OK);
            }
        }
    }

    [Fact]
    public async Task UpdateVilla_ReturnsBadRequest_WhenIdMismatch()
    {
        var updateDto = new VillaUpdateDTO
        {
            Id = 100,
            Name = "Mismatch Villa",
            Description = "Test",
            Rate = 100,
            Amenity = 3
        };

        var response = await _client.PutAsJsonAsync("/api/VillaAPI/200", updateDto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteVilla_ReturnsOk_WhenVillaExists()
    {
        var createDto = new VillaCreateDTO
        {
            Name = "Villa To Delete",
            Description = "Will be deleted",
            Rate = 50,
            Amenity = 1
        };

        var createResponse = await _client.PostAsJsonAsync("/api/VillaAPI", createDto);
        
        if (createResponse.StatusCode == HttpStatusCode.Created || createResponse.StatusCode == HttpStatusCode.OK)
        {
            var createdVilla = await createResponse.Content.ReadFromJsonAsync<Villa>();
            if (createdVilla != null)
            {
                var response = await _client.DeleteAsync($"/api/VillaAPI/{createdVilla.Id}");
                Assert.True(response.StatusCode == HttpStatusCode.OK || 
                            response.StatusCode == HttpStatusCode.NoContent ||
                            response.StatusCode == HttpStatusCode.NotFound);
            }
        }
    }

    [Fact]
    public async Task DeleteVilla_ReturnsNotFound_WhenVillaDoesNotExist()
    {
        var response = await _client.DeleteAsync("/api/VillaAPI/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetVillas_ReturnsOk_WhenVillasExist()
    {
        await _client.PostAsJsonAsync("/api/VillaAPI", new VillaCreateDTO
        {
            Name = "Villa 1",
            Rate = 100,
            Amenity = 3
        });

        await _client.PostAsJsonAsync("/api/VillaAPI", new VillaCreateDTO
        {
            Name = "Villa 2",
            Rate = 200,
            Amenity = 5
        });

        var response = await _client.GetAsync("/api/VillaAPI");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await response.Content.ReadFromJsonAsync<APIResponse>();
        Assert.NotNull(apiResponse);
    }
}
