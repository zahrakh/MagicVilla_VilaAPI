using AutoMapper;
using MagicVilla_Web.Controllers;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace MagicVilla.Tests;

public class VillaControllerTests
{
    [Fact]
    // [Theory]
    // [InlineData(2, 3, 5)]
    // [InlineData(10, 5, 15)]
    public async Task CreateVilla_Post_WhenServiceSuccess_RedirectsToIndex()
    {
        var serviceMock = new Mock<IVillaService>();
        serviceMock
            .Setup(s => s.CrateSync<APIResponse>(It.IsAny<VillaCreateDTO>()))
            .ReturnsAsync(new APIResponse { IsSuccess = true });
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<VillaController>>();
        var controller = new VillaController(serviceMock.Object, mapperMock.Object, loggerMock.Object);
        var model = new VillaCreateDTO { Name = "Blue", ImageUrl = "x.jpg" };

        IActionResult result = await controller.CreateVilla(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("IndexVilla", redirect.ActionName);
    }

    [Fact]
    public async Task UpdateVilla_Get_WhenServiceFails_ReturnsNotFound()
    {
        var serviceMock = new Mock<IVillaService>();
        serviceMock
            .Setup(s => s.GetSync<APIResponse>(It.IsAny<int>()))
            .ReturnsAsync(new APIResponse { IsSuccess = false });
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<VillaController>>();
        var controller = new VillaController(serviceMock.Object, mapperMock.Object, loggerMock.Object);

        IActionResult result = await controller.UpdateVilla(5);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteVilla_Post_WhenServiceFails_ReturnsViewWithModel()
    {
        var serviceMock = new Mock<IVillaService>();
        serviceMock
            .Setup(s => s.DeleteSync<APIResponse>(It.IsAny<int>()))
            .ReturnsAsync(new APIResponse { IsSuccess = false });
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<VillaController>>();
        var controller = new VillaController(serviceMock.Object, mapperMock.Object, loggerMock.Object);
        var model = new VillaDTO { Id = 7, Name = "FailedDelete" };

        IActionResult result = await controller.DeleteVilla(model);

        var viewResult = Assert.IsType<ViewResult>(result);
        var returnedModel = Assert.IsType<VillaDTO>(viewResult.Model);
        Assert.Equal(model.Id, returnedModel.Id);
    }

    [Fact]
    public async Task IndexVilla_WhenServiceReturnsData_ReturnsViewWithList()
    {
        var villas = new List<VillaDTO> { new() { Id = 1, Name = "A" } };
        var response = new APIResponse
        {
            IsSuccess = true,
            Result = JsonConvert.SerializeObject(villas)
        };

        var serviceMock = new Mock<IVillaService>();
        serviceMock
            .Setup(s => s.GetAllSync<APIResponse>())
            .ReturnsAsync(response);
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<VillaController>>();
        var controller = new VillaController(serviceMock.Object, mapperMock.Object, loggerMock.Object);

        IActionResult result = await controller.IndexVilla();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<VillaDTO>>(viewResult.Model);
        Assert.Single(model);
    }
}
