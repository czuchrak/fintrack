using MediatR;

namespace Fintrack.App.Functions.Admin.Commands.DeleteLogs;

public class DeleteLogsCommand : RequestBase, IRequest<Unit>
{
}