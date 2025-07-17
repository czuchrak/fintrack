using MediatR;

namespace Fintrack.App.Functions.Profile.Commands.DeleteUser;

public class DeleteUserCommand : RequestBase, IRequest<Unit>
{
}