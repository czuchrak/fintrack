using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Worker.Queries.KeepApp;
using Xunit;

namespace Fintrack.Tests.Handlers.Worker;

public class KeepAppQueryTests : TestBase
{
    [Fact]
    public async Task KeepAppQueryHandler_FinishesWithoutError()
    {
        await using var context = CreateContext();
        var handler = new KeepAppQueryHandler(context);

        await handler.Handle(new KeepAppQuery(), CancellationToken.None);
    }
}