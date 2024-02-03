using Calculator.API.Controllers;
using Calculator.API.Repositories;
using Calculator.API.Services;
using Calculator.API.Validators;
using Calculator.Common.Configurations;
using Calculator.Common.Extensions;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<UserInputValidator>();

builder.Services.RegisterMongo(builder.Configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>());
builder.Services.RegisterRabbit(
    builder.Configuration.GetSection(nameof(RabbitSettings)).Get<RabbitSettings>(),
    typeof(CalculatorController).Assembly);

builder.Services.AddScoped<ICalculatorService, CalculatorService>();
builder.Services.AddScoped<IExpressionRepository, ExpressionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
