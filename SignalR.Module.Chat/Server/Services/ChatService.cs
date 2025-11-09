using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Security;
using Oqtane.Shared;
using SignalR.Module.Chat.Repository;

namespace SignalR.Module.Chat.Services
{
    public class ServerChatService : IChatService
    {
        private readonly IChatRepository _ChatRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerChatService(IChatRepository ChatRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor)
        {
            _ChatRepository = ChatRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<Models.Chat>> GetChatsAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_ChatRepository.GetChats(ModuleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Chat Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<Models.Chat> GetChatAsync(int ChatId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_ChatRepository.GetChat(ChatId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Chat Get Attempt {ChatId} {ModuleId}", ChatId, ModuleId);
                return null;
            }
        }

        public Task<Models.Chat> AddChatAsync(Models.Chat Chat)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, Chat.ModuleId, PermissionNames.Edit))
            {
                Chat = _ChatRepository.AddChat(Chat);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Chat Added {Chat}", Chat);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Chat Add Attempt {Chat}", Chat);
                Chat = null;
            }
            return Task.FromResult(Chat);
        }

        public Task<Models.Chat> UpdateChatAsync(Models.Chat Chat)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, Chat.ModuleId, PermissionNames.Edit))
            {
                Chat = _ChatRepository.UpdateChat(Chat);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Chat Updated {Chat}", Chat);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Chat Update Attempt {Chat}", Chat);
                Chat = null;
            }
            return Task.FromResult(Chat);
        }

        public Task DeleteChatAsync(int ChatId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                _ChatRepository.DeleteChat(ChatId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Chat Deleted {ChatId}", ChatId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Chat Delete Attempt {ChatId} {ModuleId}", ChatId, ModuleId);
            }
            return Task.CompletedTask;
        }
    }
}
