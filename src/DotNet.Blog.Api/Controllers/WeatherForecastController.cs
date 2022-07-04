using DotNet.Blog.Api.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        string PolicyPrefix = "MinimumAge";
        string policy = "MinimumAge12";


        var asd = policy[PolicyPrefix.Length..];

        Console.WriteLine("---........-����..........-----");
        Console.WriteLine(asd);

        MinimumAgeRequirement minimumAge = new MinimumAgeRequirement(1)
        {
            MinimumAge = 444
        };

        Console.WriteLine(minimumAge.MinimumAge);

        _logger.LogError("Insert log...");
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

    // Ҫ���û�����������Rank������ֵΪ��M3��
    [HttpGet]
    [Authorize(Policy = "RankClaimP3")]
    public string GetForRankClaimP3()
    {
        return "Rank claim P3";
    }

    // Ҫ���û�����������Rank������ֵΪ��P3�� �� ��M3��
    [HttpGet]
    [Authorize(Policy = "RankClaimP3OrM3")]
    public string GetForRankClaimP3OrM3()
    {
        return "Rank claim P3 || M3";
    }

    // Ҫ���û�����������Rank������ֵΪ��P3�� �� ��M3��
    [HttpGet]
    [Authorize(Policy = "RankClaimP3AndM3")]
    public string GetForRankClaimP3AndM3V1()
    {
        return "Rank claim P3 && M3";
    }

    // Ҫ���û�����������Rank������ֵΪ��P3�� �� ��M3��
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
    /// �Զ���authorize
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [MinimumAgeAuthorize(20)]
    public string GetForAtLeast20Age()
    {
        return "At least 20 age";
    }

    // IAuthorizationRequirement ��ȨҪ��

    // IAuthorizationHandler ��Ȩ�������
}
