using MediatR;

namespace Fintrack.App.Functions.Profile.Commands.SetSetting;

public class SetSettingCommand : RequestBase, IRequest<Unit>
{
    public string Name { get; set; }
    public bool Value { get; set; }
}