using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Worker.Queries.KeepApp;

public class KeepAppQueryHandler : IRequestHandler<KeepAppQuery, Unit>
{
    private readonly DatabaseContext _context;

    public KeepAppQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(KeepAppQuery request, CancellationToken cancellationToken)
    {
        await _context.Logs.OrderBy(x => x.Id).FirstOrDefaultAsync(cancellationToken);

        return Unit.Value;
    }
}