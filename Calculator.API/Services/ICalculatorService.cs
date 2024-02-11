namespace Calculator.API.Services;

public interface ICalculatorService
{
    Task<double> CalculateExpression(string expression);
}
