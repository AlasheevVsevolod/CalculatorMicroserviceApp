using Calculator.AdditionService.Models;
using Calculator.AdditionService.Repositories;
using Calculator.Common.Abstractions;
using Calculator.Common.Messaging;

namespace Calculator.AdditionService.Handlers;

public class RemoveAdditionCommandHandler(IAdditionExpressionRepository additionExpressionRepository)
    : ICommandHandler<RemoveAdditionCommand>
{
    public async Task<Result> Handle(RemoveAdditionCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await additionExpressionRepository.DeleteAsync(command.Id);
            return new Result();
        }
        catch (Exception e)
        {
            return new Result { Errors = [e.ToString()] };
        }
    }
}
