using Oqtane.Models;
using Oqtane.Modules;

namespace SignalR.Module.Chat
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Chat",
            Description = "Oqtane chat module based on MS SignalR Chat example.",
            Version = "1.0.0",
            ServerManagerType = "SignalR.Module.Chat.Manager.ChatManager, SignalR.Module.Chat.Server.Oqtane",
            ReleaseVersions = "1.0.0",
            Dependencies = "SignalR.Module.Chat.Shared.Oqtane",
            PackageName = "SignalR.Module.Chat" 
        };
    }
}
