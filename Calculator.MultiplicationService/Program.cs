using System.Reflection;
using Calculator.Common.Configurations;
using Calculator.Common.Extensions;
using MultiplicationService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mongoSettings = builder.Configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>();
var rabbitSettings = builder.Configuration.GetSection(nameof(RabbitSettings)).Get<RabbitSettings>();

builder.Services.RegisterMongo(mongoSettings);
builder.Services.RegisterRabbit(rabbitSettings, Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddScoped<IMultiplicationOperationRepository, MultiplicationOperationRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();
