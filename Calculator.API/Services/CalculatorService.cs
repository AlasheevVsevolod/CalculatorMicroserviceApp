using System.Text.RegularExpressions;
using Calculator.AdditionService.Activities;
using Calculator.API.Enums;
using Calculator.API.Extensions;
using Calculator.API.Repositories;
using MassTransit;

namespace Calculator.API.Services;

public class CalculatorService(IBus bus, IEndpointNameFormatter endpointNameFormatter, IExpressionRepository expressionRepository) : ICalculatorService
{
    public async Task<double> CalculateExpression(string expression)
    {
        var expressionParts = ParseExpression(expression);

        var parsedParts = expressionParts.Select(ParseOperand).ToList();
        parsedParts = HandleNegativeOperands(parsedParts);

        var polishNotation = ConvertToPolishNotation(parsedParts);

        var result = await CalculateInPolishNotation(polishNotation);

        return result;
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

    private List<object> HandleNegativeOperands(List<object> parsedParts)
    {
        var updatedList = new List<object>();
        for (var i = 0; i < parsedParts.Count; i++)
        {
            var part = parsedParts[i];
            if (part is Operations.Subtract)
            {
                if (updatedList.Count == 0 || updatedList.Last() is Operations)
                {
                    part = -(double)parsedParts[i + 1];
                    i++;
                }
            }
            updatedList.Add(part);
        }

        return updatedList;
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
                continue;
            }

            var operation = (Operations)expressionPart;
            ProcessOperation(finalList, temporaryList, operation);
        }

        temporaryList.Reverse();
        finalList.AddRange(temporaryList);

        return finalList;
    }

    private void ProcessOperation(List<object> finalList, List<object> temporaryList, Operations operation)
    {
        while (true)
        {
            if (temporaryList.Count == 0)
            {
                temporaryList.Add(operation);
                return;
            }

            var lastTempOperation = temporaryList.Last();
            if (lastTempOperation is Operations.Add or Operations.Subtract && operation is Operations.Multiply or Operations.Divide)
            {
                temporaryList.Add(operation);
                return;
            }

            finalList.Add(lastTempOperation);
            temporaryList.RemoveAt(temporaryList.Count - 1);
        }
    }

    private async Task<double> CalculateInPolishNotation(List<object> polishNotation)
    {
        var builder = new RoutingSlipBuilder(Guid.NewGuid());
        builder.AddVariable("Result", polishNotation.First(x => x is double));

        var resultStack = new Stack<double>();
        foreach (var operand in polishNotation)
        {
            if (operand is double)
            {
                resultStack.Push((double)operand);
                continue;
            }

            resultStack.TryPop(out var op1);
            switch ((Operations)operand)
            {
                case Operations.Add:
                    builder.AddActivity(
                        "AdditionActivity",
                        new Uri($"exchange:{endpointNameFormatter.ExecuteActivity<AdditionActivity, AdditionArguments>()}"),
                        new { Operand1 = op1 });
                    break;
                case Operations.Subtract:
                    // resultStack.Push(op1 - op2);
                    break;
                case Operations.Multiply:
                    // resultStack.Push(op1 * op2);
                    break;
                case Operations.Divide:
                    // resultStack.Push(op1 / op2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        var routingSlip = builder.Build();
        await bus.Execute(routingSlip);

        return (double)(routingSlip.Variables["Result"]);
    }
}
