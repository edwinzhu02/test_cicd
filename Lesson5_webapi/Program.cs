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
// ���� URL
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
builder.Services.AddAutoMapper(typeof(Program).Assembly); // ���� AutoMapper ����


builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();
//AddAuthentication
// �������л�ȡJWT��Կ
var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Key"]);

// ������֤����
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // ������������Ӧ����Ϊtrue
    options.SaveToken = true; // ��Token���浽HttpContext��
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // ��֤ǩ����Կ
        IssuerSigningKey = new SymmetricSecurityKey(key), // ����ǩ����Կ
        ValidateIssuer = false, // ��������ض���Issuer����������Ϊtrue��ָ��Issuer
        ValidateAudience = false, // ��������ض���Audience����������Ϊtrue��ָ��Audience
        ValidateLifetime = true, // ��֤Token�Ĺ���ʱ��
        ClockSkew = TimeSpan.Zero // ����ʱ��ƫ��Ϊ0��ȷ�����ƹ���ʱ��׼ȷ
    };
});
// ��ӷ���
builder.Services.AddControllers(options =>
{
    // ע��ȫ�� Filter
    options.Filters.Add<LogActionFilter>();
});
// Ĭ��ʹ�� Console �� Debug ��־�ṩ����
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
//Serilog 
//Log.Logger = new LoggerConfiguration()
//    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day) // ÿ�������־�ļ�
//    .CreateLogger();

//builder.Host.UseSerilog();

var app = builder.Build();

app.UseAuthentication(); // ���ü�Ȩ
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
// ע���Զ����쳣���� Middleware



app.MapControllers();

app.Run();
