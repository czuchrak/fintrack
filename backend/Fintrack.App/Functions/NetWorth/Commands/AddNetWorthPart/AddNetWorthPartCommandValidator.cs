using FluentValidation;

namespace Fintrack.App.Functions.NetWorth.Commands.AddNetWorthPart;

public sealed class AddNetWorthPartCommandValidator : AbstractValidator<AddNetWorthPartCommand>
{
    public AddNetWorthPartCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Model.Name).NotEmpty().MaximumLength(40);
        RuleFor(x => x.Model.Type).NotEmpty();
        RuleFor(x => x.Model.Currency).NotEmpty();
    }
}