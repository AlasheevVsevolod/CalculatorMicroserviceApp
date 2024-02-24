using Calculator.API.Events;
using Calculator.API.Models;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.API.Controllers;

[ApiController]
[Route("api/calculator")]
public class CalculatorController(IBus bus) : ControllerBase
{
    [HttpPost("calculate")]
    public async Task<IActionResult> CalculateExpression(UserInput userInput, IValidator<UserInput> validator)
    {
        var validationResult = validator.Validate(userInput);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        var message = new ExpressionReceived
        {
            Expression = userInput.Expression,
            OperationId = userInput.OperationId
        };
        await bus.Publish(message);

        return Ok(userInput.OperationId);
    }
}
