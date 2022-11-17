using Fintrack.App.Functions.Admin.Models;
using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Admin.Queries.GetLogs;

public class GetLogsQueryHandler : AdminBaseHandler, IRequestHandler<GetLogsQuery, IEnumerable<LogModel>>
{
    public GetLogsQueryHandler(DatabaseContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LogModel>> Handle(GetLogsQuery request, CancellationToken cancellationToken)
    {
        await CheckIsAdmin(request.UserId);

        var logs = await Context.Logs.ToListAsync(cancellationToken);

        return logs.Select(x => new LogModel
        {
            Id = x.Id,
            Level = x.Level,
            Message = x.Message,
            Timestamp = x.TimeStamp,
            Exception = x.Exception
        });
    }
}