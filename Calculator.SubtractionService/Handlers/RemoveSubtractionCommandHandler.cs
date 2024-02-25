using Calculator.Common.Abstractions;
using Calculator.Common.Messaging;
using SubtractionService.Repositories;

namespace SubtractionService.Handlers;

public class RemoveSubtractionCommandHandler(ISubtractionOperationRepository operationRepository)
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
