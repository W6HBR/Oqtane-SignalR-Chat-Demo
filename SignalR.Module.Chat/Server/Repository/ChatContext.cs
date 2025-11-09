using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Infrastructure;
using Oqtane.Repository.Databases.Interfaces;

namespace SignalR.Module.Chat.Repository
{
    public class ChatContext : DBContextBase, ITransientService, IMultiDatabase
    {
        public virtual DbSet<Models.Chat> Chat { get; set; }

        public ChatContext(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
            // ContextBase handles multi-tenant database connections
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Models.Chat>().ToTable(ActiveDatabase.RewriteName("SignalRChat"));
        }
    }
}
