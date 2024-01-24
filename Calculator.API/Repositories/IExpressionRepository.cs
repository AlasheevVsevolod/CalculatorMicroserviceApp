namespace Calculator.API.Repositories;

public interface IExpressionRepository
{
    void SaveExpressionResult(string expression, double result);
}
