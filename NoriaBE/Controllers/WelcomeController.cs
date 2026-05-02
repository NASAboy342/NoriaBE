using System;
using Microsoft.AspNetCore.Mvc;

namespace NoriaBE.Controllers;

[ApiController]
[Route("/")]
public class WelcomeController: ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return $"Welcome to NoriaBE! Current server time is {DateTime.Now}. Timezone: {TimeZoneInfo.Local.DisplayName}";
    }
}
