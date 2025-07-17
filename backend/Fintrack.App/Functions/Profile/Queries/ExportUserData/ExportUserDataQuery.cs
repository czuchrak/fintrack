using Fintrack.App.Models;
using MediatR;

namespace Fintrack.App.Functions.Profile.Queries.ExportUserData;

public class ExportUserDataQuery : RequestBase, IRequest<FileModel>
{
}