using Fintrack.App.Functions.Profile.Models;
using MediatR;

namespace Fintrack.App.Functions.Profile.Commands.GetOrCreateUser;

public class GetOrCreateUserCommand : RequestBase, IRequest<UserModel>
{
    public string Email { get; set; }
}