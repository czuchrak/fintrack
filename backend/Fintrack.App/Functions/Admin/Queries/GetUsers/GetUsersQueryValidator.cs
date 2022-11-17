using FluentValidation;

namespace Fintrack.App.Functions.Admin.Queries.GetUsers;

public sealed class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}