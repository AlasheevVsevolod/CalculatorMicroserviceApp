using Calculator.Common.Messaging;
using MediatR;

namespace Calculator.Common.Abstractions;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand;
