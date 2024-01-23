using System.Text.RegularExpressions;
using Calculator.API.Models;
using FluentValidation;

namespace Calculator.API.Validators;

public class UserInputValidator : AbstractValidator<UserInput>
{
    public UserInputValidator()
    {
        RuleFor(x => x.Expression).NotEmpty().WithMessage("Expression should not be null or empty");
        RuleFor(x => x.Expression).MaximumLength(50).WithMessage("Expression exceeds limit of 50 characters");
        RuleFor(x => x.Expression).Must(BeValidExpression).WithMessage("Expression is not valid. Please verify whether the sequence of operators is correct and contains only numbers, divided by dot and basic mathematical operators");
    }

    private bool BeValidExpression(string expression)
    {
        //remove whitespaces
        var inputWithoutSpaces = expression.Replace(" ", "");

        const string validExpressionPattern = @"^(-{0,1}\d+(\.\d+){0,1})([+\-*\/]-{0,1}\d+(\.\d+){0,1})+$";
        return Regex.IsMatch(inputWithoutSpaces, validExpressionPattern);
    }
}
