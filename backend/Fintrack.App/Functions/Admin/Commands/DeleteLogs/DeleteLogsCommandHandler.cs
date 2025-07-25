using Fintrack.Database;
using MediatR;

namespace Fintrack.App.Functions.Admin.Commands.DeleteLogs;

public class DeleteLogsCommandHandler(DatabaseContext context)
    : AdminBaseHandler(context), IRequestHandler<DeleteLogsCommand, Unit>
{
    public async Task<Unit> Handle(DeleteLogsCommand request, CancellationToken cancellationToken)
    {
        await CheckIsAdmin(request.UserId);

        Context.Logs.RemoveRange(Context.Logs);
        await Context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}