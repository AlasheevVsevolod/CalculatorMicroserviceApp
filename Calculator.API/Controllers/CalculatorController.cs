using Calculator.API.Events;
using Calculator.API.Models;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.API.Controllers;

[ApiController]
[Route("api/calculator")]
public class CalculatorController(IBus bus, IRequestClient<CalculationStatusRequested> requestClient) : ControllerBase
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

    [HttpGet("operations/{operationId:guid}")]
    public async Task<IActionResult> GetCalculationStatus(Guid operationId, CancellationToken cancellationToken)
    {
        try
        {
            var message = new CalculationStatusRequested{ OperationId = operationId };
            var result = await requestClient.GetResponse<CalculationStatus>(message, cancellationToken);

            return Ok(result.Message);
        }
        catch (RequestFaultException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
