using Calculator.AdditionService.Repositories;
using Calculator.Common.Abstractions;
using Calculator.Common.Messaging;

namespace Calculator.AdditionService.Handlers;

public class RemoveAdditionCommandHandler(IAdditionOperationRepository additionOperationRepository)
    : ICommandHandler<RemoveOperationCommand>
{
    public async Task<Result> Handle(RemoveOperationCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await additionOperationRepository.DeleteAsync(command.Id);
            return new Result();
        }
        catch (Exception e)
        {
            return new Result { Errors = [e.ToString()] };
        }
    }
}
