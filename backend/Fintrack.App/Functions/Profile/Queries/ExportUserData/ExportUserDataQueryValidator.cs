using FluentValidation;

namespace Fintrack.App.Functions.Profile.Queries.ExportUserData;

public class ExportUserDataQueryValidator : AbstractValidator<ExportUserDataQuery>
{
    public ExportUserDataQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}