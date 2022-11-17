using System.Collections.Generic;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.AddPropertyCategory;
using Fintrack.App.Functions.Admin.Commands.UpdatePropertyCategory;
using Fintrack.App.Functions.Admin.Models;
using Fintrack.App.Functions.Admin.Queries.GetPropertyCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Controllers.Admin;

[Route("api/admin/[controller]")]
public class PropertyCategoryController : BaseController
{
    private readonly IMediator _mediator;

    public PropertyCategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<PropertyCategoryModel>> GetCategories()
    {
        return await _mediator.Send(new GetPropertyCategoriesQuery { UserId = UserId });
    }

    [HttpPost]
    public async Task Post(PropertyCategoryModel model)
    {
        await _mediator.Send(new AddPropertyCategoryCommand { Model = model, UserId = UserId });
    }

    [HttpPut]
    public async Task Put(PropertyCategoryModel model)
    {
        await _mediator.Send(new UpdatePropertyCategoryCommand { Model = model, UserId = UserId });
    }
}