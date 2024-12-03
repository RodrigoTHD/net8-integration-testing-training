using FluentAssertions;

namespace MyApp.IntegrationTests;

public class WeatherForecastUnitTests
{
    private WeatherForecast? weatherForecast;

    [OneTimeSetUp]
    public void BeforeAll()
    {
        weatherForecast = new WeatherForecast();
    }

    [SetUp]
    public void Before()
    {
    }

    [TearDown]
    public void AfterAll()
    {
    }

    [OneTimeTearDown]
    public void After()
    {
    }

    [Test]
    public void ShouldReturnTemperatureFromTemperatureC()
    {
        // Arrange        
        //var weatherForecast = new WeatherForecast();
        weatherForecast!.TemperatureC = 35;
        var expectedTemperatureFResult = 94;


        // Act
        var result = weatherForecast.TemperatureF;

        // Assert
        Assert.That(result, Is.EqualTo(expectedTemperatureFResult));
    }

    [Test]
    public void ShouldReturnTemperatureC()
    {
        // Arrange        
        //var weatherForecast = new WeatherForecast();

        // Act
        weatherForecast!.TemperatureC = 35;

        // Assert
        Assert.That(weatherForecast.TemperatureC, Is.EqualTo(35));
    }

    [Test]
    public void ShouldReturnSummary()
    {
        // Arrange        
        //var weatherForecast = new WeatherForecast();

        // Act
        weatherForecast!.Summary = "WeatherForecast sumarry...";

        // Assert
        weatherForecast.Summary
            .Should()
            .StartWith("WeatherForecast")
            .And
            .EndWith("sumarry...");
    }
}
