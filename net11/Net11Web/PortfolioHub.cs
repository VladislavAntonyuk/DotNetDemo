using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Net11Web;

[Authorize]
public class PortfolioHub(ILogger<PortfolioHub> logger) : Hub
{
    public static volatile bool LastCalculationCancelled;

    public string WhoAmI() =>
        $"{Context.User?.Identity?.Name} (token issued {Context.User?.FindFirstValue("issued")})";

    // The client's CancellationToken now reaches this method for regular (non-streaming) invocations
    public async Task<string> CalculateRisk(int seconds, CancellationToken cancellationToken)
    {
        LastCalculationCancelled = false;
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds), cancellationToken);
            return "Risk: low";
        }
        catch (OperationCanceledException)
        {
            LastCalculationCancelled = true;
            logger.LogWarning("CalculateRisk cancelled by the client");
            return "Cancelled";
        }
    }

    public override Task OnAuthenticationRefreshedAsync()
    {
        logger.LogInformation("Authentication refreshed without reconnect: {User}", WhoAmI());
        return base.OnAuthenticationRefreshedAsync();
    }
}
