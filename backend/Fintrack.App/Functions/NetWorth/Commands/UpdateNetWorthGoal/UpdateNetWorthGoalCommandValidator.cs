using FluentValidation;

namespace Fintrack.App.Functions.NetWorth.Commands.UpdateNetWorthGoal;

public sealed class UpdateNetWorthGoalCommandValidator : AbstractValidator<UpdateNetWorthGoalCommand>
{
    public UpdateNetWorthGoalCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Model.Id).NotEmpty();
        RuleFor(x => x.Model.Name).NotEmpty().MaximumLength(40);
        RuleFor(x => x.Model.Parts).NotEmpty();
        RuleFor(x => x.Model.Deadline).NotEmpty();
    }
}