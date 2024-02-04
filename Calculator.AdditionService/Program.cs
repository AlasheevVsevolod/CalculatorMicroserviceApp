using System.Reflection;
using Calculator.AdditionService.Repositories;
using Calculator.Common.Configurations;
using Calculator.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.RegisterMongo(builder.Configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>());
builder.Services.RegisterRabbit(
    builder.Configuration.GetSection(nameof(RabbitSettings)).Get<RabbitSettings>(),
    Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddScoped<IAdditionExpressionRepository, AdditionExpressionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();
