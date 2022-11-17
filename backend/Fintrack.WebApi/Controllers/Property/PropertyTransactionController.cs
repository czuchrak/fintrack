using System;
using System.Threading.Tasks;
using Fintrack.App.Functions.Property.Commands.AddPropertyTransaction;
using Fintrack.App.Functions.Property.Commands.RemovePropertyTransaction;
using Fintrack.App.Functions.Property.Commands.UpdatePropertyTransaction;
using Fintrack.App.Functions.Property.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Controllers.Property;

public class PropertyTransactionController : BaseController
{
    private readonly IMediator _mediator;

    public PropertyTransactionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task Post(PropertyTransactionModel model)
    {
        await _mediator.Send(new AddPropertyTransactionCommand { Model = model, UserId = UserId });
    }

    [HttpPut]
    public async Task Put(PropertyTransactionModel model)
    {
        await _mediator.Send(new UpdatePropertyTransactionCommand { Model = model, UserId = UserId });
    }

    [HttpDelete]
    public async Task Delete(Guid propertyId, Guid transactionId)
    {
        await _mediator.Send(new RemovePropertyTransactionCommand
        {
            PropertyId = propertyId,
            TransactionId = transactionId,
            UserId = UserId
        });
    }
}