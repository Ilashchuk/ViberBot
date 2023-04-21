using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using Viber.Bot.NetCore.Infrastructure;
using Viber.Bot.NetCore.Models;
using Viber.Bot.NetCore.RestApi;
using ViberApp.Models;

namespace ViberApp.Services.ViberConrolServices
{
    public class ViberControlService : IViberControlService
    {
        private readonly ViberDbContext _context;
        private readonly IViberBotApi _viberBotApi;
        public ViberControlService(ViberDbContext context, IViberBotApi viberBotApi)
        {
            _context = context;
            _viberBotApi = viberBotApi;
        }

        public async Task SendMessageAsync(string receiver, string str)
        {
            var message = new ViberMessage.TextMessage()
            {
                Receiver = receiver,
                Sender = new ViberUser.User()
                {
                    Name = "ViberTestBot",
                    Avatar = ""
                },
                Text = str
            };
            var response = await _viberBotApi.SendMessageAsync<ViberResponse.SendMessageResponse>(message);
        }
        public string GetTextFromMessage(ViberCallbackData data)
        {
            var text = String.Empty;

            switch (data.Message.Type)
            {
                case ViberMessageType.Text:
                    {
                        var mess = data.Message as ViberMessage.TextMessage;
                        text = mess.Text;
                        break;
                    }
                default: break;
            }
            return text;
        }
        public async Task<string?> GetTextForMessageWithTotalValues(string imei)
        {
            string str = String.Empty;

            TotalWalk total = new TotalWalk();
            List<TotalWalk> list = new List<TotalWalk>();

            list = await _context.TotalWalks.FromSqlInterpolated($"DECLARE @imei varchar(50) SET @imei = {imei} EXEC GetTotalWalkForIMEI @imei").ToListAsync();
            total = list.FirstOrDefault();

            if (total != null)
            {
                str = "Count of your walks: " + total.walk_count +
                    "\r\nTotal distance: " + total.walk_distance + "km" +
                    "\r\nTotal time: " + total.walk_duration + "min";
            }
            else
            {
                str = null;
            }
            return str;
        }

        public async Task CreateButton(string receiver, string messageText, string buttonName, string actionBody)
        {
            var message = new ViberMessage.KeyboardMessage()
            {
                Receiver = receiver,
                Text = messageText,
                Keyboard = new ViberKeyboard
                {
                    Buttons = new List<ViberKeyboardButton>
                                                                {
                                                                new ViberKeyboardButton
                                                                    {
                                                                        Text = buttonName,
                                                                        ActionType = ViberKeyboardActionType.Reply,
                                                                        ActionBody = actionBody
                                                                    }
                                                                },
                    DefaultHeight = true
                }
            };
            var response = await _viberBotApi.SendMessageAsync<ViberResponse.SendMessageResponse>(message);
        }

        public string GetImeiFromButtonText(string str)
        {
            string[] substrings = str.Split('.');
            return substrings[0];
        }
        public async Task<string?> GetTextForMessageWithTop10Values(string imei)
        {
            string str = "|     Walk      |      km        |   min    |";

            List<ModelForTop10> list = new List<ModelForTop10>();

            list = await _context.ModelForTop10s.FromSqlInterpolated($"DECLARE @imei varchar(50) SET @imei = {imei} EXEC GetTop10WalksForIMEI @imei").ToListAsync();

            if (list != null)
            {
                int i = 1;
                foreach (ModelForTop10 model in list)
                {
                    str += $"\r\n|     walk{i}    | {Math.Round(model.walk_distance, 3)}     |   {model.walk_duration}  |";
                    i++;
                }
            }
            else
            {
                return null;
            }
            return str;
        }
    }
}
