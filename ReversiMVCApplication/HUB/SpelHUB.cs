using Microsoft.AspNetCore.SignalR;

namespace ReversiMVCApplication.HUB;

public class SpelHUB: Hub
{
    public async Task SendGameState()
    {
        await Clients.All.SendAsync("ReceiveGameState");
    }
}