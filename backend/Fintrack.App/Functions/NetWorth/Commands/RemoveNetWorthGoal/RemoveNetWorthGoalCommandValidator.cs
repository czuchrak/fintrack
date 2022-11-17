using FluentValidation;

namespace Fintrack.App.Functions.NetWorth.Commands.RemoveNetWorthGoal;

public sealed class RemoveNetWorthGoalCommandValidator : AbstractValidator<RemoveNetWorthGoalCommand>
{
    public RemoveNetWorthGoalCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.GoalId).NotEmpty();
    }
}