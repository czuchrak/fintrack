using MediatR;

namespace Fintrack.App.Functions.Profile.Commands.SetMailVerificationSent;

public class SetMailVerificationSentCommand : RequestBase, IRequest<Unit>
{
}