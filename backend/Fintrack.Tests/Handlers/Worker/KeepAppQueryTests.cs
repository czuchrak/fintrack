using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Worker.Queries.KeepApp;
using NUnit.Framework;

namespace Fintrack.Tests.Handlers.Worker;

[TestFixture]
public class KeepAppQueryTests : TestBase
{
    [Test]
    public async Task KeepAppQueryHandler_FinishesWithoutError()
    {
        await using var context = CreateContext();
        var handler = new KeepAppQueryHandler(context);

        await handler.Handle(new KeepAppQuery(), new CancellationToken());

        Assert.Pass();
    }
}