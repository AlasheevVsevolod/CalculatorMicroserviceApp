namespace Calculator.API.Services;

public interface ICalculatorService
{
    List<object> CalculateExpression(string expression);
}
