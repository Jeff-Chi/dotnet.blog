using DotNet.Blog.Api;
using DotNet.Blog.Api.Authorization;
using DotNet.Blog.Api.Extensions;
using DotNet.Blog.Application;
using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain;
using DotNet.Blog.EFCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// log
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()  // new RenderedCompactJsonFormatter()
            .WriteTo.File(
                Path.Combine("Logs/log-.txt"),
                LogEventLevel.Error,
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: 10 * 1024 * 1024,
                retainedFileCountLimit: 7,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(2),
                outputTemplate: @"{source}{Timestamp} [{Level:u3}][{TraceId}][{SpanId}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

builder.Logging.ClearProviders().AddSerilog();

// Add services to the container. 
builder.Services.RegisterServices();

// generics service
builder.Services.AddScoped(typeof(IRepository<>), typeof(EFCoreRepository<>));

// current user service.
builder.Services.AddScoped<CurrentUserContext>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add DbContext 1.MySqlServerVersion.LatestSupportedServerVersion
// 2.var mySqlVersion = new MySqlServerVersion(new Version(configuration["MySql:Version"]));
// 3.MySqlServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Blog")
builder.Services.AddDbContext<BlogDbContext>(options =>
options.UseMySql(builder.Configuration.GetConnectionString("Blog"), MySqlServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Blog"))));
// Add JwtBearer Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecurityKey"])),
            ClockSkew = TimeSpan.FromMinutes(5) // 偏差
        };
    });


// 注入自定义的授权要求与处理器 调用顺序与注册顺序有关?
//builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IAuthorizationHandler, MinimumAgeAnotherAuthorizationHandler>());
//builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IAuthorizationHandler, MinimumAgeAuthorizationHandler>());

// 注入自定义策略提供程序
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();

// todo .. Authorization
builder.Services.AddAuthorization(options =>
{
    // 配置默认策略
    //options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    //options.InvokeHandlersAfterFailure = true;
    // 添加策略,基于声明的授权
    options.AddPolicy("RankClaim", policy => policy.RequireClaim("Rank"));

    // 指定claim的值
    options.AddPolicy("RankClaimP3", policy => policy.RequireClaim("Rank", "P3"));
    options.AddPolicy("RankClaimM3", policy => policy.RequireClaim("Rank", "M3"));

    options.AddPolicy("RankClaimP3OrM3", policy => policy.RequireClaim("Rank", "P3", "M3"));
    options.AddPolicy("RankClaimP3AndM3", policy => policy.RequireClaim("Rank", "P3").RequireClaim("Rank", "M3"));

    // 自定义策略，授权要求，授权处理器  简单策略
    // options.AddPolicy("AtLeast18Age", policy => policy.Requirements.Add(new MinimumAgeRequirement(18)));
});


// Jwt Options
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

// 内部调用了AddAuthorization
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // 忽略json序列化循环引用
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errorResponse = new ErrorResponse()
            {
                Code = "400",
                Message = "One or more validation errors occurred"
            };

            var validationErrors = new List<ValidationErrorInfo>();

            foreach (var item in context.ModelState)
            {
                var errorInfo = new ValidationErrorInfo
                {
                    Member = item.Key,
                    // item.Value.Errors.Count
                    // errorInfo.Messages = new string[item.Value.Errors.Count];

                    Messages = item.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                };

                validationErrors.Add(errorInfo);
            }

            errorResponse.ValidationErrors = validationErrors.ToArray();
            return new BadRequestObjectResult(errorResponse);

            //return new UnprocessableEntityObjectResult(errorResponse);
            //{
            //    //StatusCode = StatusCodes.Status422UnprocessableEntity,
            //};
        };
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowCredentials()
            .AllowAnyMethod();
    });
});



// swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Blog API",
        Version = "v1",
        Description = "An ASP.NET Core Web API For Blog.",
        Contact = new OpenApiContact
        {
            Name = "Jeff",
            //Url = new Uri("https://example.com/contact"),
            //Email = "asds@gmail.com"
        }
    });

    // xml comments
    var fileInfo = new FileInfo(typeof(Program).Assembly.Location);
    var files = Directory.GetFiles(fileInfo.DirectoryName!, "*.xml");
    foreach (var file in files)
    {
        options.IncludeXmlComments(file, true);
    }

    // 身份验证
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });

});


// forwarded headers
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto |
        ForwardedHeaders.XForwardedHost;
});

// kestrel
//builder.Services.Configure<KestrelServerOptions>(options =>
//{
//    options.AllowSynchronousIO = true;
//    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(5);
//});

// 1.urls
// builder.WebHost.UseUrls("http://localhost:5400");


builder.WebHost.ConfigureKestrel(serverOptions =>
{
    // 2. ListenLocalhost
    // serverOptions.ListenLocalhost(5500);
});

// auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



// DI Services
//builder.Services.AddSingleton<
//    IAuthorizationMiddlewareResultHandler, PermissionAuthorizationHandler>();

builder.Services.AddScoped<PermissionAuthorizationHandler>();

var app = builder.Build();

// 3.urls add
// app.Urls.Add("http://localhost:5600");

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//if (bool.TryParse(builder.Configuration["UseSwagger"], out bool useSwagger) && useSwagger)
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


// TODO: 异常处理
app.UseExceptionHandler(options =>
{
    options.Run(async context =>
    {
        // HttpStatusCode
        int statusCode = StatusCodes.Status500InternalServerError;
        var errorResponse = new ErrorResponse();

        var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
        if (exception != null)
        {
            if (exception is BusinessException businessException)
            {
                if (businessException.HttpStatusCode > 0)
                {
                    statusCode = businessException.HttpStatusCode;

                    // 处理参数验证错误
                    if (businessException.HttpStatusCode == StatusCodes.Status400BadRequest)
                    {
                        var validationErrors = new List<ValidationErrorInfo>();
                        ValidationErrorInfo validationError = new();
                        validationError.Member = businessException.ValidationErrorMember!;
                        validationError.Messages = businessException.ValidationErrorMessages;
                        validationErrors.Add(validationError);
                    }

                }

                errorResponse.Code = businessException.Code;

                errorResponse.Message = businessException.Message;
            }
            else if (app.Environment.IsDevelopment())
            {
                errorResponse.Message = exception.Message;
            }
            else
            {
                // localization
                // var localizer = context.RequestServices.GetService<IStringLocalizer<object>>();
                errorResponse.Message = "Internal server error";
            }
        }

        // Response StatusCode
        statusCode = statusCode > 0 ? statusCode : StatusCodes.Status403Forbidden;

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = Application.Json;

        await context.Response.WriteAsJsonAsync(errorResponse);
    });
});

// 本地化
//app.UseRequestLocalization(options =>
//{
//    var supportedCultures = new[] { "en-US", "zh-CN" };

//    options.SetDefaultCulture(supportedCultures[0]);
//    options.AddSupportedCultures(supportedCultures);
//    options.AddSupportedUICultures(supportedCultures);
//});

// cors
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

// get current user info
// app.UseMiddleware<CurrentContextMiddleware>();

app.UseAuthorization();

app.MapControllers();

// efocre auto save change.
// app.UseMiddleware<EFCoreSaveChangeMiddleware<BlogContext>>();

app.Run();
