using Fintrack.App.Functions.NetWorth.Models;
using MediatR;

namespace Fintrack.App.Functions.NetWorth.Queries.GetNetWorthModel;

public class GetNetWorthModelQuery : RequestBase, IRequest<NetWorthModel>
{
}