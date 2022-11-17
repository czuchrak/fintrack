using FluentValidation;

namespace Fintrack.App.Functions.Profile.Commands.DeleteUser;

public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}