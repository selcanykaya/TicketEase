using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TicketEase.Business.Operations.Setting;

public class MaintenanceModeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    // Bypass edilecek endpoint listesi
    private readonly string[] _bypassPaths = new[]
    {
        "/api/settings/toggle-maintenance",
        "/api/auth/login",
        "/api/auth/register"
    };

    public MaintenanceModeMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Eğer endpoint bypass listesinde ise direkt devam et
        foreach (var path in _bypassPaths)
        {
            if (context.Request.Path.StartsWithSegments(path, StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }
        }

        using (var scope = _serviceProvider.CreateScope())
        {
            var settingService = scope.ServiceProvider.GetRequiredService<ISettingService>();
            bool maintenance = await settingService.IsMaintenanceMode();

            if (maintenance)
            {
                // Admin kullanıcılar maintenance modunda bypass edebilir
                var userRole = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (userRole != "Admin")
                {
                    context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Message = "The system is currently in maintenance mode. Please try again later."
                    });
                    return;
                }
            }
        }

        await _next(context);
    }
}
