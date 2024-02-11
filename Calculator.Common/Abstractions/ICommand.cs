using Calculator.Common.Messaging;
using MediatR;

namespace Calculator.Common.Abstractions;

public interface ICommand : IRequest<Result>;
