using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Interfaces;
using Oqtane.Enums;
using Oqtane.Repository;
using SignalR.Module.Chat.Repository;
using System.Threading.Tasks;

namespace SignalR.Module.Chat.Manager
{
    public class ChatManager : MigratableModuleBase, IInstallable, IPortable, ISearchable
    {
        private readonly IChatRepository _ChatRepository;
        private readonly IDBContextDependencies _DBContextDependencies;

        public ChatManager(IChatRepository ChatRepository, IDBContextDependencies DBContextDependencies)
        {
            _ChatRepository = ChatRepository;
            _DBContextDependencies = DBContextDependencies;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(new ChatContext(_DBContextDependencies), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new ChatContext(_DBContextDependencies), tenant, MigrationType.Down);
        }

        public string ExportModule(Oqtane.Models.Module module)
        {
            string content = "";
            List<Models.Chat> Chats = _ChatRepository.GetChats(module.ModuleId).ToList();
            if (Chats != null)
            {
                content = JsonSerializer.Serialize(Chats);
            }
            return content;
        }

        public void ImportModule(Oqtane.Models.Module module, string content, string version)
        {
            List<Models.Chat> Chats = null;
            if (!string.IsNullOrEmpty(content))
            {
                Chats = JsonSerializer.Deserialize<List<Models.Chat>>(content);
            }
            if (Chats != null)
            {
                foreach(var Chat in Chats)
                {
                    _ChatRepository.AddChat(new Models.Chat { ModuleId = module.ModuleId, Name = Chat.Name });
                }
            }
        }

        public Task<List<SearchContent>> GetSearchContentsAsync(PageModule pageModule, DateTime lastIndexedOn)
        {
           var searchContentList = new List<SearchContent>();

           foreach (var Chat in _ChatRepository.GetChats(pageModule.ModuleId))
           {
               if (Chat.ModifiedOn >= lastIndexedOn)
               {
                   searchContentList.Add(new SearchContent
                   {
                       EntityName = "SignalRChat",
                       EntityId = Chat.ChatId.ToString(),
                       Title = Chat.Name,
                       Body = Chat.Name,
                       ContentModifiedBy = Chat.ModifiedBy,
                       ContentModifiedOn = Chat.ModifiedOn
                   });
               }
           }

           return Task.FromResult(searchContentList);
        }
    }
}
