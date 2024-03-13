using Calculator.Common.Abstractions;

namespace Calculator.Common.Messaging;

public record RemoveOperationCommand(Guid Id) : ICommand;
