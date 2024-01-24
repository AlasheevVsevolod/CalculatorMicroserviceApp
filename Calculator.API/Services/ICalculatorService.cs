namespace Calculator.API.Services;

public interface ICalculatorService
{
    List<string> CalculateExpression(string expression);
}
