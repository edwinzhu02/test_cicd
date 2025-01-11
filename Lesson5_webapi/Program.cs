//using Microsoft.Extensions.Configuration;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
////////////////////
///// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
////////////////////
// 配置 URL
builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

//Add Configuration
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

//AddAutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly); // 配置 AutoMapper 服务


builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();
//AddAuthentication
// 从配置中获取JWT密钥
var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Key"]);

// 配置认证服务
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // 在生产环境中应设置为true
    options.SaveToken = true; // 将Token保存到HttpContext中
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // 验证签名密钥
        IssuerSigningKey = new SymmetricSecurityKey(key), // 设置签名密钥
        ValidateIssuer = false, // 如果你有特定的Issuer，可以设置为true并指定Issuer
        ValidateAudience = false, // 如果你有特定的Audience，可以设置为true并指定Audience
        ValidateLifetime = true, // 验证Token的过期时间
        ClockSkew = TimeSpan.Zero // 设置时钟偏差为0，确保令牌过期时间准确
    };
});
// 添加服务
builder.Services.AddControllers(options =>
{
    // 注册全局 Filter
    options.Filters.Add<LogActionFilter>();
});
// 默认使用 Console 和 Debug 日志提供程序
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
//Serilog 
//Log.Logger = new LoggerConfiguration()
//    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day) // 每天滚动日志文件
//    .CreateLogger();

//builder.Host.UseSerilog();

var app = builder.Build();

app.UseAuthentication(); // 启用鉴权
app.UseAuthorization();

// Use CORS policy
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });

}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();
// 注册自定义异常处理 Middleware



app.MapControllers();

app.Run();
