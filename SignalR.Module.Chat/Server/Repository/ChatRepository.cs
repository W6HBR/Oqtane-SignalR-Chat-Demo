using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;

namespace SignalR.Module.Chat.Repository
{
    public interface IChatRepository
    {
        IEnumerable<Models.Chat> GetChats(int ModuleId);
        Models.Chat GetChat(int ChatId);
        Models.Chat GetChat(int ChatId, bool tracking);
        Models.Chat AddChat(Models.Chat Chat);
        Models.Chat UpdateChat(Models.Chat Chat);
        void DeleteChat(int ChatId);
    }

    public class ChatRepository : IChatRepository, ITransientService
    {
        private readonly IDbContextFactory<ChatContext> _factory;

        public ChatRepository(IDbContextFactory<ChatContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.Chat> GetChats(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.Chat.Where(item => item.ModuleId == ModuleId).ToList();
        }

        public Models.Chat GetChat(int ChatId)
        {
            return GetChat(ChatId, true);
        }

        public Models.Chat GetChat(int ChatId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.Chat.Find(ChatId);
            }
            else
            {
                return db.Chat.AsNoTracking().FirstOrDefault(item => item.ChatId == ChatId);
            }
        }

        public Models.Chat AddChat(Models.Chat Chat)
        {
            using var db = _factory.CreateDbContext();
            db.Chat.Add(Chat);
            db.SaveChanges();
            return Chat;
        }

        public Models.Chat UpdateChat(Models.Chat Chat)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(Chat).State = EntityState.Modified;
            db.SaveChanges();
            return Chat;
        }

        public void DeleteChat(int ChatId)
        {
            using var db = _factory.CreateDbContext();
            Models.Chat Chat = db.Chat.Find(ChatId);
            db.Chat.Remove(Chat);
            db.SaveChanges();
        }
    }
}
