using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;

namespace SignalR.Module.Chat.Services
{
    public interface IChatService 
    {
        Task<List<Models.Chat>> GetChatsAsync(int ModuleId);

        Task<Models.Chat> GetChatAsync(int ChatId, int ModuleId);

        Task<Models.Chat> AddChatAsync(Models.Chat Chat);

        Task<Models.Chat> UpdateChatAsync(Models.Chat Chat);

        Task DeleteChatAsync(int ChatId, int ModuleId);
    }

    public class ChatService : ServiceBase, IChatService
    {
        public ChatService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Chat");

        public async Task<List<Models.Chat>> GetChatsAsync(int ModuleId)
        {
            List<Models.Chat> Chats = await GetJsonAsync<List<Models.Chat>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), Enumerable.Empty<Models.Chat>().ToList());
            return Chats.OrderBy(item => item.Name).ToList();
        }

        public async Task<Models.Chat> GetChatAsync(int ChatId, int ModuleId)
        {
            return await GetJsonAsync<Models.Chat>(CreateAuthorizationPolicyUrl($"{Apiurl}/{ChatId}/{ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<Models.Chat> AddChatAsync(Models.Chat Chat)
        {
            return await PostJsonAsync<Models.Chat>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, Chat.ModuleId), Chat);
        }

        public async Task<Models.Chat> UpdateChatAsync(Models.Chat Chat)
        {
            return await PutJsonAsync<Models.Chat>(CreateAuthorizationPolicyUrl($"{Apiurl}/{Chat.ChatId}", EntityNames.Module, Chat.ModuleId), Chat);
        }

        public async Task DeleteChatAsync(int ChatId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{ChatId}/{ModuleId}", EntityNames.Module, ModuleId));
        }
    }
}
