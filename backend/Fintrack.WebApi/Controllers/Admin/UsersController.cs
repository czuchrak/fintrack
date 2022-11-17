using System.Collections.Generic;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Models;
using Fintrack.App.Functions.Admin.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Controllers.Admin;

[Route("api/admin/[controller]")]
public class UsersController : BaseController
{
    private readonly IMediator _mediator;

    public UsersController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<UserModel>> GetUsers()
    {
        return await _mediator.Send(new GetUsersQuery { UserId = UserId });
    }
}