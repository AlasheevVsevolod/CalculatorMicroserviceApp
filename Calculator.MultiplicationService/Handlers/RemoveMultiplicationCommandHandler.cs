using Calculator.Common.Abstractions;
using Calculator.Common.Messaging;
using MultiplicationService.Repositories;

namespace MultiplicationService.Handlers;

public class RemoveMultiplicationCommandHandler(IMultiplicationOperationRepository operationRepository)
    : ICommandHandler<RemoveOperationCommand>
{
    public async Task<Result> Handle(RemoveOperationCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await operationRepository.DeleteAsync(command.Id);
            return new Result();
        }
        catch (Exception e)
        {
            return new Result { Errors = [e.ToString()] };
        }
    }
}
