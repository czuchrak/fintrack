using FluentValidation;

namespace Fintrack.App.Functions.NetWorth.Commands.UpdateNetWorthPart;

public sealed class UpdateNetWorthPartCommandValidator : AbstractValidator<UpdateNetWorthPartCommand>
{
    public UpdateNetWorthPartCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Model.Id).NotEmpty();
        RuleFor(x => x.Model.Name).NotEmpty().MaximumLength(40);
        RuleFor(x => x.Model.Type).NotEmpty();
        RuleFor(x => x.Model.Currency).NotEmpty();
    }
}