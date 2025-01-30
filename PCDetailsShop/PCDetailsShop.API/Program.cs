using Application.DependencyInjection;
using DataAccessLayer.Dependency_Injection;
using PCDetailsShop.API.DependencyInjection;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddSwaggerGen();
builder.Services.AddDtoMapping();
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();