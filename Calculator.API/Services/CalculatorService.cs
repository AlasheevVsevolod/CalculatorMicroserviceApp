using System.Text.RegularExpressions;
using Calculator.API.Repositories;

namespace Calculator.API.Services;

public class CalculatorService(IExpressionRepository expressionRepository) : ICalculatorService
{
    public List<string> CalculateExpression(string expression)
    {
        var expressionParts = ParseExpression(expression);

        return expressionParts;
    }

    private List<string> ParseExpression(string expression)
    {
        var operandsPattern = @"[+\-*\/]|((\d+(\.\d+){0,1}))";
        var stringOperands = Regex.Matches(expression, operandsPattern).Select(m => m.Value).ToList();

        return stringOperands;
    }

    private double ParseOperand(string operand)
    {
        if (double.TryParse(operand, out var parsedValue))
        {
            return parsedValue;
        }

        throw new ArgumentException("Could not parse value {value} as a double", operand);
    }
}
