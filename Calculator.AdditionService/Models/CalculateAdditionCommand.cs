using Calculator.Common.Abstractions;

namespace Calculator.AdditionService.Models;

public record CalculateAdditionCommand(double Operand1, double Operand2) : ICommand;
