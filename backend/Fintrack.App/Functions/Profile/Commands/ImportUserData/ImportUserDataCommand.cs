using MediatR;

namespace Fintrack.App.Functions.Profile.Commands.ImportUserData;

public class ImportUserDataCommand : RequestBase, IRequest<ImportUserDataResult>
{
    public byte[] FileContent { get; set; }
}