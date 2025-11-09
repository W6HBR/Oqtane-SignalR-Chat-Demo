using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using SignalR.Module.Chat.Services;
using Oqtane.Controllers;
using System.Net;
using System.Threading.Tasks;

namespace SignalR.Module.Chat.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class ChatController : ModuleControllerBase
    {
        private readonly IChatService _ChatService;

        public ChatController(IChatService ChatService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _ChatService = ChatService;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.Chat>> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _ChatService.GetChatsAsync(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Chat Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Models.Chat> Get(int id, int moduleid)
        {
            Models.Chat Chat = await _ChatService.GetChatAsync(id, moduleid);
            if (Chat != null && IsAuthorizedEntityId(EntityNames.Module, Chat.ModuleId))
            {
                return Chat;
            }
            else
            { 
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Chat Get Attempt {ChatId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Chat> Post([FromBody] Models.Chat Chat)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, Chat.ModuleId))
            {
                Chat = await _ChatService.AddChatAsync(Chat);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Chat Post Attempt {Chat}", Chat);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                Chat = null;
            }
            return Chat;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Chat> Put(int id, [FromBody] Models.Chat Chat)
        {
            if (ModelState.IsValid && Chat.ChatId == id && IsAuthorizedEntityId(EntityNames.Module, Chat.ModuleId))
            {
                Chat = await _ChatService.UpdateChatAsync(Chat);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Chat Put Attempt {Chat}", Chat);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                Chat = null;
            }
            return Chat;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, int moduleid)
        {
            Models.Chat Chat = await _ChatService.GetChatAsync(id, moduleid);
            if (Chat != null && IsAuthorizedEntityId(EntityNames.Module, Chat.ModuleId))
            {
                await _ChatService.DeleteChatAsync(id, Chat.ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Chat Delete Attempt {ChatId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
