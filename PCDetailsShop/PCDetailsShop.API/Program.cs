using Application.DependencyInjection;
using Application.Jwt;
using DataAccessLayer.Dependency_Injection;
using Domain.Enums;
using PCDetailsShop.API.DependencyInjection;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddDtoMapping();
builder.Services.AddSwaggerGen();
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddApiAuthentication(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();