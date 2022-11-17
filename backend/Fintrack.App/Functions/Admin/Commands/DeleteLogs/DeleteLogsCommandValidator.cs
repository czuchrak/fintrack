using FluentValidation;

namespace Fintrack.App.Functions.Admin.Commands.DeleteLogs;

public sealed class DeleteLogsCommandValidator : AbstractValidator<DeleteLogsCommand>
{
    public DeleteLogsCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}