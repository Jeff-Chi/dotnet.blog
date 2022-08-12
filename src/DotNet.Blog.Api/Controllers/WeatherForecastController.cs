using DotNet.Blog.Api.Authorization;
using DotNet.Blog.Application;
using DotNet.Blog.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace DotNet.Blog.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        IGuidGenerator GuidGenerator = new SequentialGuidGenerator();
        Console.WriteLine(BlogAppServiceBase.CreateGuid(GuidGenerator));// ser = new();

        MinimumAgeRequirement minimumAge = new MinimumAgeRequirement(1)
        {
            MinimumAge = 444
        };

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet]
    [Authorize(Policy = "RankClaim")]
    //[Authorize("RankClaim")]
    public string GetForRankClaim()
    {
        return "Rank claim only";
    }

    // 要求用户具有声明“Rank”，且值为“M3”
    [HttpGet]
    [Authorize(Policy = "RankClaimP3")]
    public string GetForRankClaimP3()
    {
        return "Rank claim P3";
    }

    // 要求用户具有声明“Rank”，且值为“P3” 或 “M3”
    [HttpGet]
    [Authorize(Policy = "RankClaimP3OrM3")]
    public string GetForRankClaimP3OrM3()
    {
        return "Rank claim P3 || M3";
    }

    // 要求用户具有声明“Rank”，且值为“P3” 和 “M3”
    [HttpGet]
    [Authorize(Policy = "RankClaimP3AndM3")]
    public string GetForRankClaimP3AndM3V1()
    {
        return "Rank claim P3 && M3";
    }

    // 要求用户具有声明“Rank”，且值为“P3” 和 “M3”
    [Authorize(Policy = "RankClaimP3")]
    [Authorize(Policy = "RankClaimM3")]
    [HttpGet]
    public string GetForRankClaimP3AndM3V2()
    {
        return "Rank claim P3 && M3";
    }

    [HttpGet]
    [Authorize(Policy = "AtLeast18Age")]
    public string GetForAtLeast18Age()
    {
        return "At least 18 age";
    }

    /// <summary>
    /// 自定义authorize
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [MinimumAgeAuthorize(20)]
    public string GetForAtLeast20Age()
    {
        return "At least 20 age";
    }

    [HttpGet]
    public string GetBase64()
    {

        var weatherForecast = new WeatherForecast
        {
            Date = DateTime.Parse("2019-08-01"),
            Summary = "Hot",
            TemperatureC = 100
        };

        var jsonContent = JsonSerializer.Serialize(weatherForecast);

        byte[] bytes = Encoding.UTF8.GetBytes(jsonContent);
        return Convert.ToBase64String(bytes);
    }


    [HttpGet]
    public WeatherForecast GetBase64fFrom(string token)
    {

        var weatherForecast = new WeatherForecast
        {
            Date = DateTime.Parse("2019-08-01"),
            Summary = "Hot",
            TemperatureC = 100
        };

        byte[] bytes = Convert.FromBase64String(token);

        var dto = JsonSerializer.Deserialize<WeatherForecast>(bytes);


        return dto!;
    }


    // IAuthorizationRequirement 授权要求

    // IAuthorizationHandler 授权处理程序
}
