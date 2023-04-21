using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using Viber.Bot.NetCore.Infrastructure;
using Viber.Bot.NetCore.Models;
using Viber.Bot.NetCore.RestApi;
using Viber.Bot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;
using ViberApp.Models;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using ViberApp.Services.ViberConrolServices;
using NuGet.Protocol.Plugins;

namespace ViberApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ViberController : ControllerBase
    {
        private readonly IViberControlService _viberControlService;

        public ViberController(IViberControlService viberControlService)
        {
            _viberControlService = viberControlService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ViberCallbackData data)

        {
            // send first text message to the user
            if (data.Event == "conversation_started")
            {
                await _viberControlService.SendMessageAsync(data.User.Id, "Enter your IMEI:");
                return Ok();
            }
            else if (data.Event == "message")
            {
                var text = _viberControlService.GetTextFromMessage(data);

                if (text.Contains(".button"))
                {
                    string imei = _viberControlService.GetImeiFromButtonText(text);
                    string? top10 = await _viberControlService.GetTextForMessageWithTop10Values(imei);
                    if (top10 != null)
                    {
                        await _viberControlService.SendMessageAsync(data.Sender.Id, top10);
                        return Ok();
                    }
                    await _viberControlService.SendMessageAsync(data.Sender.Id, "Something went wrong!!!\r\nCan`t get Top 10");
                    return Ok();
                }
                else
                {
                    string message = await _viberControlService.GetTextForMessageWithTotalValues(text);
                    if (message != null)
                    {
                        await _viberControlService.SendMessageAsync(data.Sender.Id, message);

                        await _viberControlService.CreateButton(data.Sender.Id,
                            "If you want to see your TOP 10 walks press the button below",
                            "TOP 10",
                            text + ".button");
                        return Ok();
                    }
                    else
                    {
                        await _viberControlService.SendMessageAsync(data.Sender.Id, "Something went wrong!!!\r\nPlease try again!");
                        return Ok();
                    }
                }
                
            }
                return Ok();
        }
    }
}
