using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Toast.Services;

namespace Toast.Tests.Integration;

public class TestRunnerTests
{
    [Fact]
    public async Task ExecuteUrlAsync_ReturnsSuccessStatusCode()
    {
        // Arrange
        var url = "https://services.odata.org/V4/TripPinServiceRW/";
        var testRunner = new TestRunner();

        // Act
        var response = await testRunner.ExecuteUrlAsync(url);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public void ValidateResponse_ReturnsTrueForSuccessStatusCode()
    {
        // Arrange
        var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        var testRunner = new TestRunner();

        // Act
        var isValid = testRunner.ValidateResponse(response);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void ValidateResponse_ReturnsFalseForNonSuccessStatusCode()
    {
        // Arrange
        var response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        var testRunner = new TestRunner();

        // Act
        var isValid = testRunner.ValidateResponse(response);

        // Assert
        Assert.False(isValid);
    }
}
