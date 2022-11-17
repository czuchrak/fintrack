using Fintrack.Database;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Admin;

public abstract class AdminBaseHandler
{
    protected readonly DatabaseContext Context;

    protected AdminBaseHandler(DatabaseContext context)
    {
        Context = context;
    }

    protected async Task CheckIsAdmin(string? userId)
    {
        var adminId = (await Context.Settings.SingleAsync(x => x.Name == "AdminId")).Value;
        if (userId != adminId)
            throw new UnauthorizedAccessException("Unauthorized");
    }
}