using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fintrack.App.Functions.Property.Commands.AddProperty;
using Fintrack.App.Functions.Property.Commands.RemoveProperty;
using Fintrack.App.Functions.Property.Commands.UpdateProperty;
using Fintrack.App.Functions.Property.Models;
using Fintrack.App.Functions.Property.Queries.GetProperties;
using Fintrack.App.Functions.Property.Queries.GetPropertyDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Controllers.Property;

public class PropertyController : BaseController
{
    private readonly IMediator _mediator;

    public PropertyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<PropertyModel>> Get()
    {
        return await _mediator.Send(new GetPropertiesQuery { UserId = UserId });
    }

    [HttpPost]
    public async Task Post(PropertyModel model)
    {
        await _mediator.Send(new AddPropertyCommand { Model = model, UserId = UserId });
    }

    [HttpPut]
    public async Task Put(PropertyModel model)
    {
        await _mediator.Send(new UpdatePropertyCommand { Model = model, UserId = UserId });
    }

    [HttpDelete]
    public async Task Delete(Guid id)
    {
        await _mediator.Send(new RemovePropertyCommand { PropertyId = id, UserId = UserId });
    }

    [HttpGet]
    [Route("details")]
    public async Task<PropertyDetailsModel> GetPropertyDetails(Guid id)
    {
        return await _mediator.Send(new GetPropertyDetailsQuery { PropertyId = id, UserId = UserId });
    }
}