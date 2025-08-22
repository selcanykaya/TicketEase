using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketEase.WebApi.Filters;
using TicketEase.Business.Operations.Setting;

[Route("api/[controller]")]
[ApiController]
public class SettingsController : ControllerBase
{
    private readonly ISettingService _settingService;

    public SettingsController(ISettingService settingService)
    {
        _settingService = settingService;
    }

    [HttpPatch("toggle-maintenance")]
    [Authorize(Roles = "Admin")]
    [TimeControlFilter]
    public async Task<IActionResult> ToggleMaintenance()
    {
        var result = await _settingService.ToggleMaintenance();
        return Ok(result); 
    }
}
