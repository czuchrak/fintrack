using Fintrack.App.Functions.Admin.Models;
using MediatR;

namespace Fintrack.App.Functions.Admin.Queries.GetUsers;

public class GetUsersQuery : RequestBase, IRequest<IEnumerable<UserModel>>
{
}