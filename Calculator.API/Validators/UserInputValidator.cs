using System.Text.RegularExpressions;
using Calculator.API.Models;
using FluentValidation;

namespace Calculator.API.Validators;

public class UserInputValidator : AbstractValidator<UserInput>
{
    public UserInputValidator()
    {
        RuleFor(x => x.Expression).NotEmpty();
        RuleFor(x => x.Expression).Must(BeValidExpression).WithMessage("Expression is not valid");
    }

    private bool BeValidExpression(string expression)
    {
        //remove whitespaces
        var inputWithoutSpaces = expression.Replace(" ", "");

        //-
        var yobaExpression = @"^(-{0,1}\d+(\.\d{1,3}){0,1})([+\-*\/]-{0,1}\d+(\.\d{1,3}){0,1}){1,9}$";
        return Regex.IsMatch(inputWithoutSpaces, yobaExpression);
    }
}
