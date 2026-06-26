using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

[Authorize]
public class MetricsHub : Hub
{
    // Klienci tylko się łączą i słuchają.
    // Można dodować metody publiczne wywoływane z React
}