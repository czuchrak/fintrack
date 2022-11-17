using Fintrack.App.Functions.Admin.Models;
using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Admin.Queries.GetUsers;

public class GetUsersQueryHandler : AdminBaseHandler, IRequestHandler<GetUsersQuery, IEnumerable<UserModel>>
{
    public GetUsersQueryHandler(DatabaseContext context) : base(context)
    {
    }

    public async Task<IEnumerable<UserModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        await CheckIsAdmin(request.UserId);

        var users = await Context.Users.ToListAsync(cancellationToken);
        var parts = await Context.NetWorthParts.ToListAsync(cancellationToken);
        var entries = await Context.NetWorthEntries.ToListAsync(cancellationToken);
        var properties = await Context.Properties.ToListAsync(cancellationToken);
        var goals = await Context.NetWorthGoals.ToListAsync(cancellationToken);

        return users.Select(x => new UserModel
            {
                Id = x.Id,
                Name = x.Email.Split('@').First(),
                LastActivity = x.LastActivity,
                CreationDate = x.CreationDate,
                NewMonthEmailEnabled = x.NewMonthEmailEnabled,
                NewsEmailEnabled = x.NewsEmailEnabled,
                PartsCount = parts.Count(y => y.UserId == x.Id),
                EntriesCount = entries.Count(y => y.UserId == x.Id),
                GoalsCount = goals.Count(y => y.UserId == x.Id),
                PropertiesCount = properties.Count(y => y.UserId == x.Id)
            }
        );
    }
}