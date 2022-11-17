using Fintrack.App.Functions.Admin.Models;
using MediatR;

namespace Fintrack.App.Functions.Admin.Queries.GetLogs;

public class GetLogsQuery : RequestBase, IRequest<IEnumerable<LogModel>>
{
}