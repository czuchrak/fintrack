using FluentValidation;

namespace Fintrack.App.Functions.Profile.Commands.GetOrCreateUser;

public sealed class GetOrCreateUserCommandValidator : AbstractValidator<GetOrCreateUserCommand>
{
    public GetOrCreateUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
    }
}