using Calculator.API.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.API.Controllers;

[ApiController]
[Route("api/calculator")]
public class CalculatorController : ControllerBase
{
    [HttpPost("calculate")]
    public IActionResult CalculateExpression(UserInput userInput, IValidator<UserInput> validator)
    {
        var validationResult = validator.Validate(userInput);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        return Ok(userInput.Expression);
    }
}
