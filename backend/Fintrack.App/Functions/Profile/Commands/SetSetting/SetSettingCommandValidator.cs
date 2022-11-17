using FluentValidation;

namespace Fintrack.App.Functions.Profile.Commands.SetSetting;

public sealed class SetSettingCommandValidator : AbstractValidator<SetSettingCommand>
{
    public SetSettingCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}