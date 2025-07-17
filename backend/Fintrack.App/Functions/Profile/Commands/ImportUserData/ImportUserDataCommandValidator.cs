using FluentValidation;

namespace Fintrack.App.Functions.Profile.Commands.ImportUserData;

public class ImportUserDataCommandValidator : AbstractValidator<ImportUserDataCommand>
{
    public ImportUserDataCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FileContent).NotEmpty();
    }
}