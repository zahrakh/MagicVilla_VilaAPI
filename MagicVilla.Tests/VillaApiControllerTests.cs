using AutoMapper;
using MagicVilla.Controllers;
using MagicVilla.Logging;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using MagicVilla.repository.InterfaceRepository;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MagicVilla.Tests;

public class VillaApiControllerTests
{
    [Fact]
    public async Task GetVilla_WithZeroId_ReturnsBadRequest()
    {
        var repositoryMock = new Mock<IVillaRepository>();
        var loggerMock = new Mock<ILogging>();
        var mapperMock = new Mock<IMapper>();
        var controller = new VillaApiController(repositoryMock.Object, loggerMock.Object, mapperMock.Object);

        ActionResult<APIResponse> result = await controller.GetVilla(0);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetVilla_WhenVillaNotFound_ReturnsNotFound()
    {
        var repositoryMock = new Mock<IVillaRepository>();
        repositoryMock
            .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Villa, bool>>>(), true, null))
            .ReturnsAsync((Villa?)null);
        var loggerMock = new Mock<ILogging>();
        var mapperMock = new Mock<IMapper>();
        var controller = new VillaApiController(repositoryMock.Object, loggerMock.Object, mapperMock.Object);

        ActionResult<APIResponse> result = await controller.GetVilla(99);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetVillas_ReturnsOkWithMappedResult()
    {
        var villas = new List<Villa> { new() { Id = 1, Name = "Villa A" } };
        var mappedVillas = new List<VillaDTO> { new() { Id = 1, Name = "Villa A" } };

        var repositoryMock = new Mock<IVillaRepository>();
        repositoryMock
            .Setup(r => r.GetAllAsync(null, null))
            .ReturnsAsync(villas);
        var loggerMock = new Mock<ILogging>();
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<List<VillaDTO>>(villas)).Returns(mappedVillas);
        var controller = new VillaApiController(repositoryMock.Object, loggerMock.Object, mapperMock.Object);

        ActionResult<APIResponse> result = await controller.GetVillas();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<APIResponse>(okResult.Value);
        Assert.True(response.IsSuccess);
        Assert.Same(mappedVillas, response.Result);
    }

    [Fact]
    public async Task UpdateVilla_WithIdMismatch_ReturnsBadRequest()
    {
        var repositoryMock = new Mock<IVillaRepository>();
        var loggerMock = new Mock<ILogging>();
        var mapperMock = new Mock<IMapper>();
        var controller = new VillaApiController(repositoryMock.Object, loggerMock.Object, mapperMock.Object);
        var dto = new VillaUpdateDTO
        {
            Id = 10,
            Name = "Mismatch",
            ImageUrl = "img.jpg"
        };

        ActionResult<APIResponse> result = await controller.UpdateVilla(9, dto);

        Assert.IsType<BadRequestObjectResult>(result.Result);
        repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Villa>()), Times.Never);
    }
}
