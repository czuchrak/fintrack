using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Admin.Commands.UpdatePropertyCategory;

public class UpdatePropertyCategoryCommandHandler : AdminBaseHandler,
    IRequestHandler<UpdatePropertyCategoryCommand, Unit>
{
    public UpdatePropertyCategoryCommandHandler(DatabaseContext context) : base(context)
    {
    }

    public async Task<Unit> Handle(UpdatePropertyCategoryCommand request, CancellationToken cancellationToken)
    {
        await CheckIsAdmin(request.UserId);
        var model = request.Model;

        var category = await Context.PropertyCategories
            .SingleAsync(x => x.Id == model.Id, cancellationToken);

        category.Name = model.Name;
        category.Type = model.Type;
        category.IsCost = model.IsCost;

        await Context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}