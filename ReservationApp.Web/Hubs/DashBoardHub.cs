using Microsoft.AspNetCore.SignalR;
using ReservationApp.Application.Common.utility;

namespace ReservationApp.Hubs;

public class DashBoardHub: Hub
{
    public async override Task OnConnectedAsync()
    {
        var user = Context.User;
        if (user.IsInRole(SD.Role_Admin))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, SD.Role_Admin);
        }
        else if (user.IsInRole(SD.Role_Owner))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, SD.Role_Owner);
        }
        await base.OnConnectedAsync();
    }
}