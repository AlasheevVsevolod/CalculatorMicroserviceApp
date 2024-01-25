using System.Text.RegularExpressions;
using Calculator.API.Enums;
using Calculator.API.Extensions;
using Calculator.API.Repositories;

namespace Calculator.API.Services;

public class CalculatorService(IExpressionRepository expressionRepository) : ICalculatorService
{
    public List<object> CalculateExpression(string expression)
    {
        var expressionParts = ParseExpression(expression);

        var parsedParts = expressionParts.Select(ParseOperand).ToList();

        var polishNotation = ConvertToPolishNotation(parsedParts);

        return polishNotation;
    }

    private List<string> ParseExpression(string expression)
    {
        var operandsPattern = @"[+\-*\/]|((\d+(\.\d+){0,1}))";
        var stringOperands = Regex.Matches(expression, operandsPattern).Select(m => m.Value).ToList();

        return stringOperands;
    }

    private object ParseOperand(string operand)
    {
        if (double.TryParse(operand, out var parsedValue))
        {
            return parsedValue;
        }

        if (operand.TryGetOperation(out var operation))
        {
            return operation;
        }

        throw new ArgumentException("Could not parse value {value} as a double or operation", operand);
    }

    private List<object> ConvertToPolishNotation(List<object> expressionParts)
    {
        var finalList = new List<object>();
        var temporaryList = new List<object>();

        foreach (var expressionPart in expressionParts)
        {
            if (expressionPart is double)
            {
                finalList.Add(expressionPart);
            }
            else
            {
                temporaryList.Add(expressionPart);
            }
        }

        finalList.AddRange(temporaryList);

        return finalList;
    }
}
