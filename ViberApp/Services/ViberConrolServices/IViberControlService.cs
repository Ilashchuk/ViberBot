using Viber.Bot.NetCore.Models;

namespace ViberApp.Services.ViberConrolServices
{
    public interface IViberControlService
    {
        public Task SendMessageAsync(string receiver, string str);
        public string GetTextFromMessage(ViberCallbackData data);
        public Task<string> GetTextForMessageWithTotalValues(string imei);
        public Task CreateButton(string receiver, string messageText, string buttonName, string actionBody);
        public string GetImeiFromButtonText(string str);
        public Task<string?> GetTextForMessageWithTop10Values(string imei);
    }
}
