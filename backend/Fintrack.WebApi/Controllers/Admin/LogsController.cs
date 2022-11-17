using System.Collections.Generic;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.DeleteLogs;
using Fintrack.App.Functions.Admin.Models;
using Fintrack.App.Functions.Admin.Queries.GetLogs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Controllers.Admin;

[Route("api/admin/[controller]")]
public class LogsController : BaseController
{
    private readonly IMediator _mediator;

    public LogsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<LogModel>> GetLogs()
    {
        return await _mediator.Send(new GetLogsQuery { UserId = UserId });
    }

    [HttpDelete]
    public async Task DeleteLogs()
    {
        await _mediator.Send(new DeleteLogsCommand { UserId = UserId });
    }
}