using Calculator.Common.Abstractions;
using Calculator.Common.Messaging;
using Calculator.DivisionService.Repositories;

namespace Calculator.DivisionService.Handlers;

public class RemoveDivisionCommandHandler(IDivisionOperationRepository operationRepository)
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
