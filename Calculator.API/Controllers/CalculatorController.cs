using Microsoft.AspNetCore.Mvc;

namespace Calculator.API.Controllers;

[ApiController]
[Route("api/{controller}")]
public class CalculatorController : ControllerBase
{
    [HttpGet]
    public IActionResult GetTest()
    {
        return Ok("123");
    }
}
