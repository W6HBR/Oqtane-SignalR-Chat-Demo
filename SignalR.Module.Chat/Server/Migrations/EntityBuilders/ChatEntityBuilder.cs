using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace SignalR.Module.Chat.Migrations.EntityBuilders
{
    public class ChatEntityBuilder : AuditableBaseEntityBuilder<ChatEntityBuilder>
    {
        private const string _entityTableName = "SignalRChat";
        private readonly PrimaryKey<ChatEntityBuilder> _primaryKey = new("PK_SignalRChat", x => x.ChatId);
        private readonly ForeignKey<ChatEntityBuilder> _moduleForeignKey = new("FK_SignalRChat_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public ChatEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override ChatEntityBuilder BuildTable(ColumnsBuilder table)
        {
            ChatId = AddAutoIncrementColumn(table,"ChatId");
            ModuleId = AddIntegerColumn(table,"ModuleId");
            Name = AddMaxStringColumn(table,"Name");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> ChatId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> Name { get; set; }
    }
}
