using System.Text.RegularExpressions;
using Calculator.API.Enums;
using Calculator.API.Events;
using Calculator.API.Extensions;
using MassTransit;

namespace Calculator.API.Consumers;

public class ConvertExpressionConsumer : IConsumer<ConvertExpression>
{
    public async Task Consume(ConsumeContext<ConvertExpression> context)
    {
        try
        {
            var expressionParts = ParseExpression(context.Message.Expression);

            var parsedParts = expressionParts.Select(ParseOperand).ToList();
            parsedParts = HandleNegativeOperands(parsedParts);

            var objectsInPolishNotation = ConvertToPolishNotation(parsedParts);

            var conversionFinishedMessage = new ConversionCompleted{ObjectsInPolishNotation = objectsInPolishNotation, OperationId = context.Message.OperationId};
            await context.Publish(conversionFinishedMessage);
        }
        catch (Exception e)
        {
            var errorMessage = new ConversionFailed{ ErrorMessage = e.Message, OperationId = context.Message.OperationId};
            await context.Publish(errorMessage);
        }
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
            return operation.GetValue();
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

    private List<string> ConvertToPolishNotation(List<object> expressionParts)
    {
        var finalList = new List<string>();
        var temporaryList = new List<Operations>();

        foreach (var expressionPart in expressionParts)
        {
            if (expressionPart is double)
            {
                finalList.Add(expressionPart.ToString());
                continue;
            }

            ((string)expressionPart).TryGetOperation(out var operation);
            ProcessOperation(finalList, temporaryList, operation);
        }

        temporaryList.Reverse();
        finalList.AddRange(temporaryList.Select(x => x.GetValue()));

        return finalList;
    }

    private void ProcessOperation(List<string> finalList, List<Operations> temporaryList, Operations operation)
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

            finalList.Add(lastTempOperation.GetValue());
            temporaryList.RemoveAt(temporaryList.Count - 1);
        }
    }
}
