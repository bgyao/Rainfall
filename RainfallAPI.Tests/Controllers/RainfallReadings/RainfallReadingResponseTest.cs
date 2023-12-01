using RainfallAPI.Controllers.RainfallReadings;
using System.Net;
using System.Web.Http;

namespace RainfallAPI.Tests.Controllers.RainfallReadings;

public class RainfallReadingResponseTest
{
    [Fact]
    public async Task ShouldGetAllRainfallDataOfStation()
    {
        var controller = new RainfallReadingResponse();
        var result = await controller.Get("4168");
        //Assert.NotNull(result);
        Assert.NotEmpty(result.Value.Readings);
    }

    [Fact]
    public async Task ShouldReturnNotFoundIfStationDoesNotExist()
    {
        try
        {
            var controller = new RainfallReadingResponse();
            var result = await controller.Get("1");
            Assert.NotNull(result);
        }
        catch (HttpResponseException ex)
        {
            Assert.Equal(HttpStatusCode.NotFound, ex.Response.StatusCode);
        }
        
    }
}
