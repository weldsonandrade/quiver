using Microsoft.AspNet.SignalR;

namespace Quiver.WebAPI
{
    public interface IUserIdProvider
    {
        string GetUserId(IRequest request);
    }
}