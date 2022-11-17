using FluentValidation;

namespace Fintrack.App.Functions.NetWorth.Commands.AddNetWorthGoal;

public sealed class AddNetWorthGoalCommandValidator : AbstractValidator<AddNetWorthGoalCommand>
{
    public AddNetWorthGoalCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Model.Name).NotEmpty().MaximumLength(40);
        RuleFor(x => x.Model.Parts).NotEmpty();
        RuleFor(x => x.Model.Deadline).NotEmpty();
    }
}