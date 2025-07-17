using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Worker.Queries.KeepApp;

public class KeepAppQueryHandler(DatabaseContext context) : IRequestHandler<KeepAppQuery, Unit>
{
    public async Task<Unit> Handle(KeepAppQuery request, CancellationToken cancellationToken)
    {
        await context.Logs.OrderBy(x => x.Id).FirstOrDefaultAsync(cancellationToken);

        return Unit.Value;
    }
}