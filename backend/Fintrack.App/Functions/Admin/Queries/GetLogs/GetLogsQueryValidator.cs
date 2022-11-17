using FluentValidation;

namespace Fintrack.App.Functions.Admin.Queries.GetLogs;

public sealed class GetLogsQueryValidator : AbstractValidator<GetLogsQuery>
{
    public GetLogsQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}