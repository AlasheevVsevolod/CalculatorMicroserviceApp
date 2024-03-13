using Calculator.Common.Abstractions;

namespace Calculator.Common.Messaging;

public record CalculateOperationCommand(double Operand1, double Operand2) : ICommand;
