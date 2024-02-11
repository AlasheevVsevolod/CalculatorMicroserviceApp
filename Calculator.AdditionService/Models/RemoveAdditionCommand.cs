using Calculator.Common.Abstractions;

namespace Calculator.AdditionService.Models;

public record RemoveAdditionCommand(Guid Id) : ICommand;
