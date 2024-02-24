using System.Reflection;
using Calculator.API.Repositories;
using Calculator.API.StateMachines;
using Calculator.API.Validators;
using Calculator.Common.Configurations;
using FluentValidation;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<UserInputValidator>();

var mongoSettings = builder.Configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>();
var rabbitSettings = builder.Configuration.GetSection(nameof(RabbitSettings)).Get<RabbitSettings>();

// builder.Services.RegisterMongo(mongoSettings);

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumers(Assembly.GetExecutingAssembly());
    x.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(rabbitSettings.Host, h =>
        {
            h.Username(rabbitSettings.Username);
            h.Password(rabbitSettings.Password);
        });
        configurator.UseInMemoryOutbox(context);
        configurator.ConfigureEndpoints(context);
    });

    x.AddSagaStateMachine<CalculateExpressionStateMachine, CalculateExpressionState>()
        .MongoDbRepository(r =>
        {
            r.Connection = mongoSettings.ConnectionString;
            r.DatabaseName = mongoSettings.SagaDatabaseName;
        });
});

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
